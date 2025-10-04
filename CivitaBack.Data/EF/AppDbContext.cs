using CivitaBack.Data.BO;
using Microsoft.EntityFrameworkCore;

namespace CivitaBack.Data.EF;

public partial class AppDbContext : DbContext
{
    public DbSet<Test> Usuarios { get; set; }
    public DbSet<TestModelado> NombreTabla { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}
