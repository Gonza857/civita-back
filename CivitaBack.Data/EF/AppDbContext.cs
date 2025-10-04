using CivitaBack.Data.BO;
using Microsoft.EntityFrameworkCore;

namespace CivitaBack.Data.EF;

public partial class AppDbContext : DbContext
{
    public DbSet<Test> Usuarios { get; set; }
    public DbSet<TestModelado> NombreTabla { get; set; }

<<<<<<< HEAD
    public DbSet<Partida> Partida { get; set; }
    public DbSet<Usuario> Usuario { get; set; }
    public DbSet<Estructura> Estructura { get; set; }
    public DbSet<TipoEstructura> TipoEstructura { get; set; }
    public DbSet<Recurso> Recurso { get; set; }
    public DbSet<Evento> Evento { get; set; }

    public DbSet<EventoMaestro> EventoMaestro { get; set; }

    public DbSet<Tienda> Tienda { get; set; }
    public DbSet<EstructuraEnMapa> EstructuraEnMapa { get; set; }
    public DbSet<Tip> Tip { get; set; }
    public DbSet<TipEnPartida> TipEnPartida { get; set; }
    public DbSet<TipoTip> TipoTip { get; set; }
    


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Database=tpi-neocivita;Username=postgres;Password=postgresql");
    }
=======
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
>>>>>>> decbba690bc8c1f0a2da542d5f09b579b894d5a7
}
