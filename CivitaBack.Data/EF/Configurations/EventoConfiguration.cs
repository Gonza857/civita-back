using CivitaBack.Data.BO;
using CivitaBack.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CivitaBack.Data.EF.Configurations
{
    public class EventoConfiguration : IEntityTypeConfiguration<EventoEF>
    {
        public void Configure(EntityTypeBuilder<EventoEF> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Property(e => e.Contenido).HasMaxLength(500).IsRequired();
            builder.Property(e => e.RespuestaJugador).HasMaxLength(1); 

            // Relación con EventoMaestro (1:N)
            builder.HasOne(e => e.EventoMaestro)
                    .WithMany(em => em.Evento) 
                    .HasForeignKey(e => e.EventoMaestroId)
                    .OnDelete(DeleteBehavior.Restrict);

            // Relación con Partida
            builder.HasOne(e => e.Partida)
                    .WithMany(p => p.Evento)
                    .HasForeignKey(e => e.PartidaId)
                    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
