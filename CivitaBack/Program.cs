using CivitaBack.Data.EF;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowViteDev", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // URL del Vite dev server
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Mostrar todas las variables de entorno
foreach (System.Collections.DictionaryEntry env in Environment.GetEnvironmentVariables())
{
    Console.WriteLine($"{env.Key} = {env.Value}");
}

// Mostrar solo DEV_PROFILE
Console.WriteLine("DEV_PROFILE = " + Environment.GetEnvironmentVariable("DEV_PROFILE"));

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("DEV_PROFILE")}.json", optional: true)
    .AddEnvironmentVariables();

Console.WriteLine("Perfil DEV_PROFILE: " + Environment.GetEnvironmentVariable("DEV_PROFILE"));


builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseNpgsql(connectionString);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors("AllowViteDev");

app.Run();
