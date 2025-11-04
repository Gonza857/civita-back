using CivitaBack.Data.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace CivitaBack.Data;

/// <summary>
/// Esta clase es utilizada EXCLUSIVAMENTE por las herramientas de EF Core
/// (como 'Add-Migration' o 'Update-Database') en tiempo de diseño.
/// Le enseña a las herramientas cómo obtener el connection string
/// desde appsettings.json, appsettings.Development.json y User Secrets.
/// </summary>
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        // 1. Obtenemos la ruta al proyecto de API (donde están los appsettings)
        // Asume que este proyecto (Data) está en una carpeta paralela a la API
        var apiProjectPath = Path.Combine(Directory.GetCurrentDirectory(),
            "..", "CivitaBack"); // <-- CAMBIÁ "CivitaBack" por el nombre de tu proyecto de API

        // 2. Construimos la configuración manualmente
        var config = new ConfigurationBuilder()
            .SetBasePath(apiProjectPath) // Apunta a la carpeta de la API
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile("appsettings.Development.json", optional: true) // Carga el .json de desarrollo
            .AddUserSecrets<AppDbContextFactory>() // <-- ¡Carga tu secrets.json!
            .AddEnvironmentVariables()
            .Build();

        // 3. Obtenemos el connection string (que ahora SÍ va a encontrar)
        var connectionString = config.GetConnectionString("DefaultConnection");

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("El Connection String 'DefaultConnection' no se encontró.");
        }

        // 4. Creamos y devolvemos el DbContext
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new AppDbContext(optionsBuilder.Options);
    }
}