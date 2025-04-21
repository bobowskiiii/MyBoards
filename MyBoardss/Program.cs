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


// Uruchom aplikację
app.Run();