using CivitaBack.Data.BO;
using CivitaBack.Data.EF;
using CivitaBack.Data.Repositorio;
using CivitaBack.Logica;
using CivitaBack.Logica.Backgrounds;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CivitaBack.Domain.Enum;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Logica.Hubs;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Utils;
using Hangfire;
using Hangfire.PostgreSql;
using Hangfire.Common;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS para Vite Dev
builder.Services.AddCors(options =>
{
    // 1. Cambiá el nombre de la política (más claro)
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins(
                "http://localhost:5173",           // Tu localhost de Vite (Desarrollo)
                "https://front-civita.vercel.app"  // ¡LA SOLUCIÓN! (Producción Vercel)
            ) 
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); 
    });
});

var devProfile = Environment.GetEnvironmentVariable("DEV_PROFILE");
Console.WriteLine($"Perfil DEV_PROFILE: {devProfile ?? "no definido"}");

// Cargar configuración con perfiles
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    // .AddJsonFile($"appsettings.{devProfile}.json", optional: true)
    .AddEnvironmentVariables();

// Configurar DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseNpgsql(connectionString)
        .EnableSensitiveDataLogging(false) // opcional: evita mostrar valores de parámetros
        .EnableDetailedErrors(false);

});

builder.Services.AddHangfire((sp, config) =>
{
    config.UsePostgreSqlStorage(options =>
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        options.UseNpgsqlConnection(connectionString);
    });
});
builder.Services.AddHangfireServer();

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
builder.Services.AddScoped<ITipsRepositorio, TipRepositorio>();

builder.Services.AddScoped<ITipoTipLogica, TipoTipLogica>();
builder.Services.AddScoped<ITipoTipRepositorio, TipoTipRepositorio>();

builder.Services.AddScoped<IEstructuraLogica, EstructuraLogica>();
builder.Services.AddScoped<IEstructuraRepositorio, EstructuraRepositorio>();

builder.Services.AddScoped<ITipoEstructuraRepositorio, TipoEstructuraRepositorio>();
builder.Services.AddScoped<ITipoEstructuraLogica, TipoEstructuraLogica>();

builder.Services.AddScoped<IEventoLogica, EventoLogica>();
builder.Services.AddScoped<IEventoRepositorio, EventoRepositorio>();

builder.Services.AddScoped<ICondicionRepositorio, CondicionRepositorio>();
builder.Services.AddScoped<ICondicionLogica, CondicionLogica>();

builder.Services.AddScoped<IMisionLogica, MisionLogica>();
builder.Services.AddScoped<IMisionRepositorio, MisionRepositorio>();

builder.Services.AddScoped<IMisionPartidaLogica, MisionPartidaLogica>();
builder.Services.AddScoped<IMisionPartidaRepositorio, MisionPartidaRepositorio>();

builder.Services.AddScoped<IRecompensaLogica, RecompensaLogica>();
builder.Services.AddScoped<IRecompensaRepositorio, RecompensaRepositorio>();

builder.Services.AddScoped<IAuthLogica, AuthLogica>();
builder.Services.AddScoped<IInicialLogica, InicialLogica>();
builder.Services.AddScoped<ICicloLogica, CicloLogica>();
builder.Services.AddScoped<IUnidadDeTrabajo, UnidadDeTrabajo>();
builder.Services.AddScoped<IRecompensaLogica, RecompensaLogica>();

builder.Services.AddSingleton<BackgroundCicloLogica>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<BackgroundCicloLogica>());

builder.Services.AddAutoMapper(cfg =>
{
    
}, typeof(Program));

builder.Services.AddSignalR();

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

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.Migrate(); 
}

// 🔹 Job cada 30 segundos (para probar tu servicio)
using (var scope = app.Services.CreateScope())
{
    var recurringJobs = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
    
    recurringJobs.AddOrUpdate(
        "mision-diaria",
        Job.FromExpression<IMisionLogica>(servicio => servicio.ResetMisiones(TipoMision.Diaria)),
        // Cron.Daily(0, 0)
        "*/30 * * * * *" // <--- Modificado a 30 segundos
    );
    
    recurringJobs.AddOrUpdate(
        "mision-semanal",
        Job.FromExpression<IMisionLogica>(servicio => servicio.ResetMisiones(TipoMision.Semanal)),
        Cron.Weekly(DayOfWeek.Monday, 0, 0)
    );
    
    recurringJobs.AddOrUpdate(
        "mision-mensual",
        Job.FromExpression<IMisionLogica>(servicio => servicio.ResetMisiones(TipoMision.Mensual)),
        Cron.Monthly(1, 0, 0)
    );
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHangfireDashboard();
}

app.MapHub<CicloHub>("/cicloHub");
app.MapHub<EventoHub>("/eventoHub");
app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigins");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
