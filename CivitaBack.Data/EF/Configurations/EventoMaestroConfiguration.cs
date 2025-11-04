using CivitaBack.Data.BO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CivitaBack.Data.EF.Configurations;

public class EventoMaestroConfiguration : IEntityTypeConfiguration<EventoMaestroEF>
{
    public void Configure(EntityTypeBuilder<EventoMaestroEF> builder)
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

        var seedDate = new DateTime(2025, 11, 1, 0, 0, 0, DateTimeKind.Utc);

        builder.HasData(
                new EventoMaestroEF
                {
                    Id = 1, // ¡Id manual es obligatorio!
                    Nombre = "Separación de residuos",
                    TextoDescripcion = "Los vecinos solicitan un sistema de reciclaje en la ciudad debido a la alta contaminación.",
                    TextoAceptar = "En Argentina, solo el 3% de los residuos se reciclan. Separar la basura reduce rellenos sanitarios y emisiones de metano.",
                    TextoRechazar = "Cuando no se recicla, los rellenos sanitarios crecen y emiten metano, un gas 28 veces peor que el CO₂ para el clima.",
                    EcoCoinsAceptar = -100,
                    FelicidadAceptar = 10,
                    ContaminacionAceptar = -10,
                    FelicidadRechazar = -10,
                    ContaminacionRechazar = 20,
                    Creado = seedDate,
                });
    }
}
