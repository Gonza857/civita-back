using CivitaBack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CivitaBack.Data.EF.Configurations;

public class EventoMaestroConfiguration : IEntityTypeConfiguration<EventoMaestro>
{
    public void Configure(EntityTypeBuilder<EventoMaestro> builder)
    {
        builder.HasKey(em => em.Id);
        builder.Property(em => em.Id).ValueGeneratedOnAdd();
        builder.Property(em => em.Nombre).HasMaxLength(200);
        builder.Property(em => em.TextoDescripcion).HasMaxLength(500);
        builder.Property(em => em.TextoAceptar).HasMaxLength(200);
        builder.Property(em => em.TextoRechazar).HasMaxLength(200);

        // 1:N con Evento
        builder.HasMany(em => em.Evento)
               .WithOne(e => e.EventoMaestro)
               .HasForeignKey(e => e.EventoMaestroId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
