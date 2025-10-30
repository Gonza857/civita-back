using CivitaBack.Data.BO;
using Microsoft.EntityFrameworkCore;
using CivitaBack.Domain.Entities;
using Evento = CivitaBack.Domain.Entities.Evento;
using EventoMaestro = CivitaBack.Domain.Entities.EventoMaestro;
using Recurso = CivitaBack.Domain.Entities.Recurso;
using Tienda = CivitaBack.Domain.Entities.Tienda;
using Tip = CivitaBack.Domain.Entities.Tip;
using TipEnPartida = CivitaBack.Domain.Entities.TipEnPartida;
using TipoEstructura = CivitaBack.Domain.Entities.TipoEstructura;
using TipoLogro = CivitaBack.Domain.Entities.TipoLogro;
using TipoTip = CivitaBack.Domain.Entities.TipoTip;
using Usuario = CivitaBack.Domain.Entities.Usuario;

namespace CivitaBack.Data.EF;

public partial class AppDbContext : DbContext
{
    public DbSet<PartidaEF> Partida { get; set; }
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
    public DbSet<LogroEF> Logro { get; set; }
    public DbSet<LogroPartidaEF> LogroPartida { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

}
