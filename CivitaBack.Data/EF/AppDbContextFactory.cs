using CivitaBack.Data.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        // Perfil dev, opcional
        var devProfile = Environment.GetEnvironmentVariable("DEV_PROFILE") ?? "DevGonza";

        // Tomar la ruta absoluta del proyecto API
        var solutionDir = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "CivitaBack"));

        var configuration = new ConfigurationBuilder()
            .SetBasePath(solutionDir)
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile($"appsettings.{devProfile}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrEmpty(connectionString))
            throw new InvalidOperationException("No se encontró la cadena de conexión para este perfil.");

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new AppDbContext(optionsBuilder.Options);
    }
}

