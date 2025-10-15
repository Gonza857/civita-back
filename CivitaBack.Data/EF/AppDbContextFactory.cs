using CivitaBack.Data.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        // Leer el perfil actual (por ejemplo: Gonza)
        var devProfile = Environment.GetEnvironmentVariable("DEV_PROFILE");
        Console.WriteLine($"Perfil DEV_PROFILE detectado: {devProfile ?? "no definido"}");

        // Buscar el directorio donde está el proyecto .API
        var basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "CivitaBack.API");

        // Construir configuración desde el .API
        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile($"appsettings.{devProfile}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        // Obtener la cadena de conexión
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrEmpty(connectionString))
            throw new InvalidOperationException("No se encontró la cadena de conexión para este perfil.");

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        Console.WriteLine($"Conectando a: {connectionString}");
        return new AppDbContext(optionsBuilder.Options);
    }
}
