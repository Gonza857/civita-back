using CivitaBack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CivitaBack.Data.EF.Configurations
{
    public class EventoConfiguration : IEntityTypeConfiguration<Evento>
    {
        public void Configure(EntityTypeBuilder<Evento> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Property(e => e.TextoDescripcion).HasMaxLength(500);
            builder.Property(e => e.TextoAceptar).HasMaxLength(200);
            builder.Property(e => e.TextoRechazar).HasMaxLength(200);

            // Relación con EventoMaestro
            builder.HasOne(e => e.EventoMaestro)
                   .WithMany() // si EventoMaestro no tiene colección de Eventos
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
