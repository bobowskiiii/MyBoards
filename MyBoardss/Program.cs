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
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyBoards API v1");
        c.RoutePrefix = string.Empty; // Otwiera Swaggera od razu na stronie głównej
    });
}


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
    


// Uruchom aplikację
app.Run();