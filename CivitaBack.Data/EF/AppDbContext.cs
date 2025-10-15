using CivitaBack.Data.BO;
using Microsoft.EntityFrameworkCore;

namespace CivitaBack.Data.EF;

public partial class AppDbContext : DbContext
{
    public DbSet<Partida> Partida { get; set; }
    public DbSet<Usuario> Usuario { get; set; }
    public DbSet<Estructura> Estructura { get; set; }
    public DbSet<TipoEstructura> TipoEstructura { get; set; }
    public DbSet<Recurso> Recurso { get; set; }
    public DbSet<Evento> Evento { get; set; }
    public DbSet<EventoMaestro> EventoMaestro { get; set; }
    public DbSet<Tienda> Tienda { get; set; }
    public DbSet<EstructuraMapa> EstructuraMapa { get; set; }
    public DbSet<Tip> Tip { get; set; }
    public DbSet<TipEnPartida> TipEnPartida { get; set; }
    public DbSet<TipoTip> TipoTip { get; set; }
    public DbSet<Condicion> Condicion { get; set; }
    public DbSet<TipoLogro> TipoLogro { get; set; }
    public DbSet<Logro> Logro { get; set; }
    public DbSet<LogroPartida> LogroPartida { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configurar tabla de unión
        modelBuilder.Entity<LogroPartida>()
            .HasKey(lp => new { lp.LogroId, lp.PartidaId }); // PK compuesta

        modelBuilder.Entity<LogroPartida>()
            .HasOne(lp => lp.Logro)
            .WithMany(l => l.LogroPartidas)
            .HasForeignKey(lp => lp.LogroId);

        modelBuilder.Entity<LogroPartida>()
            .HasOne(lp => lp.Partida)
            .WithMany(p => p.LogroPartidas)
            .HasForeignKey(lp => lp.PartidaId);
    }


}
