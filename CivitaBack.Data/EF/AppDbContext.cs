using CivitaBack.Data.BO;
using Microsoft.EntityFrameworkCore;

namespace CivitaBack.Data.EF;

public partial class AppDbContext : DbContext
{
    public DbSet<Test> Usuarios { get; set; }
    public DbSet<TestModelado> NombreTabla { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Database=tpi-neocivita;Username=postgres;Password=admin");
    }
}
