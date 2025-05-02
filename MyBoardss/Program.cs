using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MyBoardss.Entities;

var builder = WebApplication.CreateBuilder(args);

// Dodaj usługi do kontenera DI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "MyBoards API",
        Version = "v1"
    });
});

builder.Services.AddDbContext<MyBoardsContext>(
    option => option.UseMySql(
        builder.Configuration.GetConnectionString("MyBoardsConnectionString"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MyBoardsConnectionString"))
    )
);

var app = builder.Build();

// Konfiguracja middleware
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyBoards API v1");
    c.RoutePrefix = string.Empty; // Otwiera Swaggera od razu na stronie głównej
});



using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<MyBoardsContext>();

var pendingMigrations = dbContext.Database.GetPendingMigrations();
if (pendingMigrations.Any())
{
    dbContext.Database.Migrate();
    Console.WriteLine("Migracje zastosowane.");
}


var users = dbContext.Users.ToList();
if (!users.Any())
{
    var user1 = new User()
    {
        FullName = "Jan Kowalski",
        Email = "jKowalski@test.com",
        
        Address = new Address()
        {
            Country = "Poland",
            City = "Warszawa",
            PostalCode = "00-001",
            Street = "Krakowska 12"
        }

    };
    
    var user2 = new User()
    {
        FullName = "Matthew Murdock",
        Email = "mMurdock@test.com",
        Address = new Address()
        {
            Country = "United States",
            PostalCode = "10001",
            City = "New York",
            Street = "5th Avenue 12"
        }

    };
    
    dbContext.Users.AddRange(user1, user2);
    dbContext.SaveChanges();
}


app.MapGet("data", async(MyBoardsContext db) =>
{
    // var epic = db.Epics.FirstOrDefault();
    // var user = db.Users.FirstOrDefault(u => u.FullName == "Lola Hauck");
    // return new
    // {
    //     epic,
    //     user
    // };
    // var newComments = await db.Comments
    //     .Where(c => c.CreatedDate > new DateTime(2022, 7, 23))
    //     .ToListAsync();
    //
    // return newComments;

    // var top5comments = await db.Comments
    //     .OrderByDescending(c => c.CreatedDate)
    //     .Take(5)
    //     .ToListAsync();
    // return top5comments;

    // var statesCount = await db.WorkItems
    //     .GroupBy(x => x.StateId)
    //     .Select(g => new
    //     {
    //         stateId = g.Key,
    //         count = g.Count()
    //     })
    //     .ToListAsync();
    // return statesCount;

    var onHold = await db.Epics
        .Where(e => e.State.State == "On Hold")
        .OrderBy(e => e.Priority)
        .ToListAsync();

    var mostCommentsUser = await db.Comments
        .GroupBy(c => c.AuthorId)
        .Select(g => new
        {
            AuthorId = g.Key,
            Count = g.Count()
        })
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
})
.WithName("GetTags")
.WithTags("Tags")
.Produces<List<Tag>>(StatusCodes.Status200OK);

// Uruchom aplikację
app.Run();