using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MyBoardss.Entities;

var builder = WebApplication.CreateBuilder(args);

// Konfiguracja usług
builder.Services.AddEndpointsApiExplorer();
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "MyBoards API",
        Version = "v1"
    });
});


builder.Services.AddDbContext<MyBoardsContext>(options =>
{
    options.UseLazyLoadingProxies();
    options.UseMySql(
        builder.Configuration.GetConnectionString("MyBoardsConnectionString"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MyBoardsConnectionString"))
    );
});


var app = builder.Build();


//Konfiguracja Middleware
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyBoards API v1");
    c.RoutePrefix = string.Empty;
});

await SeedIfNecessary(app);

// Przykładowy endpoint GET
app.MapGet("/data", async (MyBoardsContext db) =>
{
    var onHold = await db.Epics
        .Where(e => e.State.State == "On Hold")
        .OrderBy(e => e.Priority)
        .ToListAsync();

    var mostCommentsUser = await db.Comments
        .GroupBy(c => c.AuthorId)
        .Select(g => new { AuthorId = g.Key, Count = g.Count() })
        .OrderByDescending(x => x.Count)
        .FirstOrDefaultAsync();

    if (mostCommentsUser != null)
    {
        var user = await db.Users
            .Where(u => u.Id == mostCommentsUser.AuthorId)
            .Select(u => new
            {
                u.FullName,
                u.Email,
                u.Address
            })
            .FirstOrDefaultAsync();

        return user;
    }

    return null;
});

// Przykładowy endpoint POST (update)
app.MapPost("/update", async (MyBoardsContext db) =>
{
    var epic = await db.Epics.FirstAsync(epic => epic.Id == 1);
    var onHoldState = await db.WorkStates.FirstAsync(w => w.State == "On Hold");

    epic.StateId = onHoldState.Id;
    epic.Area = "Updated Area";
    epic.Priority = 1;
    epic.StartDate = new DateTime(2025, 10, 31);

    await db.SaveChangesAsync();
});

app.MapPost("create", async (MyBoardsContext db) =>
{
    Tag mvcTag = new Tag()
    {
        Value = "MVC"
    };
    Tag aspTag = new Tag()
    {
        Value = "ASP"
    };
    var tagsList = new List<Tag>() { mvcTag, aspTag };
    
    await db.AddRangeAsync(tagsList);;
    await db.SaveChangesAsync();   
    return new
    {
        mvcTag,
        aspTag
    };
});

//POST z dodawaniem relacji 1:1
app.MapPost("create_entity", async (MyBoardsContext db) =>
{
    var address = new Address
    {
        City = "Warsaw",
        Country = "Poland",
        PostalCode = "00-001",
        Street = "Bracka",
        User = null,
    };

    var user = new User()
    {
        FullName = "Matthew McConaughey",
        Email = "mConaughey@gmail.com",
        Address = address,
    };
    await db.AddAsync(user);
    await db.SaveChangesAsync();
});

app.MapPost("add_WorkItem", async (MyBoardsContext db) =>
{
    // Sprawdź, czy użytkownik istnieje
    var existingUser = await db.Users.FirstOrDefaultAsync(u => u.Id == Guid.Parse("08dd8b18-f027-44f5-8063-3d7c7e77a5a5"));
    if (existingUser == null)
    {
        // Dodaj użytkownika, jeśli nie istnieje
        existingUser = new User
        {
            Id = Guid.Parse("08dd8b18-f027-44f5-8063-3d7c7e77a5a5"),
            FullName = "John Doe",
            Email = "johndoe@example.com"
        };
        await db.Users.AddAsync(existingUser);
        await db.SaveChangesAsync();
    }

    // Dodaj WorkItem
    var workItem = new MyBoardss.Entities.Task
    {
        Area = "Area 1",
        IterationPath = "Iteration 1",
        Priority = 1,
        Type = "Task",
        Activity = "Activity 1",
        RemaningWork = 5,
        StateId = (await db.WorkStates.FirstAsync()).Id, 
        AuthorId = existingUser.Id 
    };

    await db.AddAsync(workItem);
    await db.SaveChangesAsync();

    return Results.Ok(workItem);
});

//POST - comments
app.MapPost("add_comments", async (MyBoardsContext db) =>
{
    var user = new User()
    {
        FullName = "New User",
        Email = "newUser@gmail.com",
    };
    var comment = new Comment()
    {
        Message = "No elo elo",
        CreatedDate = new DateTime(2005, 04, 15).AddDays(5),
        UpdatedDate = DateTime.Now,
        WorkItemId = 2,
        AuthorId = Guid.Parse("08dd8b18-f027-44f5-8063-3d7c7e77a5a5"),
    };

    await db.AddAsync(comment);
    await db.SaveChangesAsync();
});
    
//GET author data

// DELETE workItem where WorkItemId = 2

 app.MapDelete("delete", async (MyBoardsContext db) =>
 {
     var workItemTags = await db.WorkItemTags
         .Where(t => t.WorkItemId == 3)
         .ToListAsync();

     db.RemoveRange(workItemTags);
     await db.SaveChangesAsync();
 });

//DELETE user & userComments where UserId = 1e1539f4-29c9-11f0-8204-41ce48fb126a
 app.MapDelete("delete_user_id", async (MyBoardsContext db) =>
 {
     var user = await db.Users
         .FirstOrDefaultAsync(u => u.Id == Guid.Parse("1e1539f4-29c9-11f0-8204-41ce48fb126a"));

     var userComments = await db.Comments
         .Where(c => c.AuthorId == user.Id)
         .ToListAsync();
     
     db.RemoveRange(userComments);
     await db.SaveChangesAsync();
     
     db.Users.Remove(user);
     await db.SaveChangesAsync();
 });

//DELETE CLientCascade po stronie EF
 app.MapDelete("delete_client_cascade", async (MyBoardsContext db) =>
 {
     var user = await db.Users
         .Include(u => u.Comments)
         .FirstAsync(u => u.Id == Guid.Parse("1e153882-29c9-11f0-8204-41ce48fb126a"));

     db.Remove(user);
     await db.SaveChangesAsync();

 });

//GET raw SQL
 app.MapGet("raw", async (MyBoardsContext db) =>
 {
     var minWorkItemsCount = 85;
     var states = await db.WorkStates
         .FromSqlInterpolated($@"
        SELECT WS.Id, WS.State, COUNT(*) AS WorkItemCount
        FROM myboards.WorkStates WS
        JOIN myboards.WorkItems WI ON WS.Id = WI.StateId
        GROUP BY WS.Id, WS.State;
        ")
         .ToListAsync();

     await db.Database.ExecuteSqlRawAsync($"UPDATE myboards.Comments SET UpdatedDate = CURRENT_TIMESTAMP WHERE AuthorId = '1e153882-29c9-11f0-8204-41ce48fb126a';");
     return states;
 });

//Data from View to List async
 app.MapGet("ViewData", async (MyBoardsContext db) =>
 {
     var topAuthors = await db.ViewTopAuthors.ToListAsync();
     return topAuthors;

 });

//Lazy Loading
 app.MapGet("Lazy", async (MyBoardsContext db) =>
 {
     var withAddress = true;
     var user = await db.Users
         .FirstOrDefaultAsync(u => u.Id == Guid.Parse("1e1533a0-29c9-11f0-8204-41ce48fb126a"));

     if (withAddress)
     {
         var result = new
         {
             FullName = user?.FullName,
             Address = $"{user?.Address.City} {user?.Address.Street}"
             
         };
         return result;
     }

     return new { FullName = user?.FullName, Address = "-" };
 });


app.Run();




static async System.Threading.Tasks.Task SeedIfNecessary(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<MyBoardsContext>();

    var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
    if (pendingMigrations.Any())
    {
        await dbContext.Database.MigrateAsync();
        Console.WriteLine("Applied pending migrations.");
    }
    
    // SeedData.SeedDatabase(dbContext);
}
public class WorkStateStats
{
    public int Id { get; set; }
    public string StateName { get; set; }
    public int WorkItemCount { get; set; }
}