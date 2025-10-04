using CivitaBack.Data.BO;
using Microsoft.EntityFrameworkCore;

namespace CivitaBack.Data.EF;

public partial class AppDbContext : DbContext
{
    public DbSet<Test> Usuarios { get; set; }
    public DbSet<TestModelado> NombreTabla { get; set; }

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
}
