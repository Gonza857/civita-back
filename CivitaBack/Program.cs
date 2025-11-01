using CivitaBack.Data.BO;
using CivitaBack.Data.EF;
using CivitaBack.Data.Repositorio;
using CivitaBack.Logica;
using CivitaBack.Logica.Backgrounds;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Logica.Hubs;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS para Vite Dev
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowViteDev", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // credenciales
    });
});

var devProfile = Environment.GetEnvironmentVariable("DEV_PROFILE");
Console.WriteLine($"Perfil DEV_PROFILE: {devProfile ?? "no definido"}");

// Cargar configuración con perfiles
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    //.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddJsonFile($"appsettings.{devProfile}.json", optional: true)
    .AddEnvironmentVariables();

// Configurar DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseNpgsql(connectionString);
});

builder.Services.AddScoped<IPartidaLogica, PartidaLogica>();
builder.Services.AddScoped<IPartidaRepositorio, PartidaRepositorio>();

builder.Services.AddScoped<ITipoLogroLogica, TipoLogroLogica>();
builder.Services.AddScoped<ITipoLogroRepositorio, TipoLogroRepositorio>();

builder.Services.AddScoped<ILogroLogica, LogroLogica>();
builder.Services.AddScoped<ILogroRepositorio, LogroRepositorio>();

builder.Services.AddScoped<IRecursoLogica, RecursoLogica>();
builder.Services.AddScoped<IRecursoRepositorio, RecursoRepositorio>();

builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
builder.Services.AddScoped<IUsuarioLogica, UsuarioLogica>();

builder.Services.AddScoped<ILogroPartidaRepositorio, LogroPartidaRepositorio>();
builder.Services.AddScoped<ILogroPartidaLogica, LogroPartidaLogica>();

builder.Services.AddScoped<IEstructuraMapaLogica, EstructuraMapaLogica>();
builder.Services.AddScoped<IEstructuraMapaRepositorio, EstructuraMapaRepositorio>();

builder.Services.AddScoped<ITipLogica, TipLogica>();
builder.Services.AddScoped<ITipsRepositorio, TipsRepositorio>();


builder.Services.AddScoped<ITipoTipLogica, TipoTipLogica>();
builder.Services.AddScoped<ITipoTipRepositorio, TipoTipRepositorio>();

builder.Services.AddScoped<IEstructuraLogica, EstructuraLogica>();
builder.Services.AddScoped<IEstructuraRepositorio, EstructuraRepositorio>();

builder.Services.AddScoped<ITipoEstructuraRepositorio, TipoEstructuraRepositorio>();
builder.Services.AddScoped<ITipoEstructuraLogica, TipoEstructuraLogica>();

builder.Services.AddScoped<IEventoLogica, EventoLogica>();
builder.Services.AddScoped<IEventoRepositorio, EventoRepositorio>();

builder.Services.AddScoped<IAuthLogica, AuthLogica>();
builder.Services.AddScoped<ICicloLogica, CicloLogica>();
builder.Services.AddScoped<IUnidadDeTrabajo, UnidadDeTrabajo>();

builder.Services.AddSingleton<BackgroundCicloLogica>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<BackgroundCicloLogica>());

builder.Services.AddAutoMapper(cfg => 
{
    // Aquí adentro podrías agregar configuraciones globales
    // si las necesitaras, pero para tu caso, lo dejamos vacío.
    
}, typeof(Program));

builder.Services.AddSignalR();
builder.Services.AddScoped<ICondicionRepositorio, CondicionRepositorio>();
builder.Services.AddScoped<ICondicionLogica, CondicionLogica>();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

var app = builder.Build();

// Aquí, después de construir la app, aseguramos que la DB exista
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.Migrate(); // Aplica solo las migraciones pendientes
}

// Pipeline 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHub<CicloHub>("/cicloHub");
app.MapHub<EventoHub>("/eventoHub");
app.UseHttpsRedirection();
app.UseCors("AllowViteDev");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
