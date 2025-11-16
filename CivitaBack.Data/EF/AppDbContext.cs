using CivitaBack.Data.BO;
using Microsoft.EntityFrameworkCore;

namespace CivitaBack.Data.EF;

public partial class AppDbContext : DbContext
{
    public DbSet<PartidaEF> Partida { get; set; }
    public DbSet<UsuarioEF> Usuario { get; set; }
    public DbSet<EstructuraEF> Estructura { get; set; }
    public DbSet<TipoEstructuraEF> TipoEstructura { get; set; }
    public DbSet<RecursoEF> Recurso { get; set; }
    public DbSet<EventoEF> Evento { get; set; }
    public DbSet<EventoMaestroEF> EventoMaestro { get; set; }
    public DbSet<TiendaEF> Tienda { get; set; }
    public DbSet<EstructuraMapaEF> EstructuraMapa { get; set; }
    public DbSet<TipEF> Tip { get; set; }
    public DbSet<TipEnPartidaEF> TipEnPartida { get; set; }
    public DbSet<TipoTipEF> TipoTip { get; set; }
    public DbSet<CondicionEF> Condicion { get; set; }
    public DbSet<TipoLogroEF> TipoLogro { get; set; }
    public DbSet<LogroEF> Logro { get; set; }
    public DbSet<LogroPartidaEF> LogroPartida { get; set; }
    
    public DbSet<MisionEF> Mision { get; set; }
    
    public DbSet<MisionPartidaEF> MisionPartida { get; set; }

    public DbSet<EfectoEventoEF> EfectoEvento { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

}
