using CivitaBack.Data.BO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CivitaBack.Domain.Enum;

namespace CivitaBack.Data.EF.Configurations;

public class EventoMaestroConfiguration : IEntityTypeConfiguration<EventoMaestroEF>
{
    public void Configure(EntityTypeBuilder<EventoMaestroEF> builder)
    {
        builder.HasKey(em => em.Id);
        builder.Property(em => em.Id).ValueGeneratedOnAdd();

        builder.Property(em => em.TipoEvento).HasConversion<int>(); 
        builder.Property(em => em.Titulo).HasMaxLength(100).IsRequired();
        builder.Property(em => em.ContenidoPrincipal).HasMaxLength(500).IsRequired();
        builder.Property(em => em.OpcionA_Texto).HasMaxLength(250).IsRequired();
        builder.Property(em => em.OpcionB_Texto).HasMaxLength(250).IsRequired();
        builder.Property(em => em.RespuestaCorrecta).HasMaxLength(1).IsRequired();

        builder.HasMany(em => em.Efectos)
               .WithOne(ee => ee.EventoMaestro)
               .HasForeignKey(ee => ee.EventoMaestroId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(em => em.Evento)
               .WithOne(e => e.EventoMaestro)
               .HasForeignKey(e => e.EventoMaestroId)
               .OnDelete(DeleteBehavior.Restrict);

        var seedDate = new DateTime(2025, 11, 1, 0, 0, 0, DateTimeKind.Utc);

        builder.HasData(
            new EventoMaestroEF
            {
                Id = 1,
                TipoEvento = TipoEvento.PREGUNTA,
                Titulo = "¿Cómo me muevo hoy?",
                ContenidoPrincipal = "Si tenés que recorrer 5 km en la ciudad, ¿qué opción genera la MENOR cantidad de emisiones de CO₂ (Dióxido de carbono) por persona?",
                OpcionA_Texto = "A) Ir en colectivo con 30 personas más.",
                OpcionB_Texto = "B) Usar un auto moderno, solo con el conductor.",
                RespuestaCorrecta = "A",
                Creado = seedDate,
            },
            new EventoMaestroEF
            {
                Id = 2,
                TipoEvento = TipoEvento.TIP_INFORMATIVO,
                Titulo = "¡Alerta Roja de Contaminación!",
                ContenidoPrincipal = "Tu nivel de contaminación es críticamente alto. Si excede el 80%, la Felicidad de tus ciudadanos caerá rápidamente. Intentá construir más plantas de tratamiento de aire y espacios verdes.",
                OpcionA_Texto = "Entendido.", 
                OpcionB_Texto = "", 
                RespuestaCorrecta = "A",
                Creado = seedDate,
            }
        );
    }
}
