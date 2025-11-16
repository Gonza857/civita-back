using CivitaBack.Data.BO;
using CivitaBack.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class EfectoEventoConfiguration : IEntityTypeConfiguration<EfectoEventoEF>
{
    public void Configure(EntityTypeBuilder<EfectoEventoEF> builder)
    {
        builder.HasKey(ee => ee.Id);
        builder.Property(ee => ee.Id).ValueGeneratedOnAdd();

        builder.HasOne(ee => ee.EventoMaestro)
               .WithMany(em => em.Efectos)
               .HasForeignKey(ee => ee.EventoMaestroId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.Property(ee => ee.TipoResultado).HasConversion<int>();

        var seedDate = new DateTime(2025, 11, 1, 0, 0, 0, DateTimeKind.Utc);

        builder.HasData(
            new EfectoEventoEF
            {
                Id = 1,
                EventoMaestroId = 1,
                TipoResultado = TipoResultado.ACIERTO,
                EcoCoins = 50,
                Felicidad = 3,
                Contaminacion = -4,
                Energia = 0,
                Experiencia = 50,
                Creado = seedDate
            },
            new EfectoEventoEF
            {
                Id = 2,
                EventoMaestroId = 1,
                TipoResultado = TipoResultado.FALLO,
                EcoCoins = -20,
                Felicidad = -2,
                Contaminacion = 3,
                Energia = 0,
                Experiencia = 0,
                Creado = seedDate
            }
        );
    }
}