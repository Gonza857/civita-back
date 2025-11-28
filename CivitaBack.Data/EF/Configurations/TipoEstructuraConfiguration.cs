using CivitaBack.Data.BO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CivitaBack.Data.EF.Configurations;

public class TipoEstructuraConfiguration : IEntityTypeConfiguration<TipoEstructuraEF>
{
    public void Configure(EntityTypeBuilder<TipoEstructuraEF> builder)
    {
        builder.HasKey(te => te.Id);
        builder.Property(te => te.Id).ValueGeneratedOnAdd();
        builder.Property(te => te.Nombre).HasMaxLength(200);

        // 1:N con Estructura
        builder.HasMany(te => te.Estructura)
            .WithOne(e => e.TipoEstructura)
            .HasForeignKey(e => e.TipoEstructuraId);
        
        var seedDate = new DateTime(2025, 11, 1, 0, 0, 0, DateTimeKind.Utc);
        
        builder.HasData(
            new TipoEstructuraEF
            {
                Id = 1, // ¡Id manual obligatorio!
                Nombre = "Vivienda",
                Ocupacion = 0,
                Capacidad = 5,
                EnergiaPorCiclo = -1,
                DineroPorCiclo = -10,
                Creado = seedDate,
            },
            new TipoEstructuraEF
            {
                Id = 2,
                Nombre = "Energia",
                Ocupacion = 0,
                Capacidad = 0,
                EnergiaPorCiclo = 5,
                DineroPorCiclo = -10,
                Creado = seedDate,
            },
            new TipoEstructuraEF
            {
                Id = 3,
                Nombre = "Industrial",
                Ocupacion = 0,
                Capacidad = 0,
                EnergiaPorCiclo = -2,
                DineroPorCiclo = 10,
                Creado = seedDate,
            },
            new TipoEstructuraEF
            {
                Id = 4,
                Nombre = "Verde",
                Ocupacion = 0,
                Capacidad = 0,
                EnergiaPorCiclo = -1,
                DineroPorCiclo = -2,
                Creado = seedDate,
            },
            new TipoEstructuraEF
            {
                Id = 5,
                Nombre = "Alojamiento",
                Ocupacion = 0,
                Capacidad = 50,
                EnergiaPorCiclo = -1,
                DineroPorCiclo = -5,
                Creado = seedDate,
            },
            new TipoEstructuraEF
            {
                Id = 6,
                Nombre = "Reciclaje",
                Ocupacion = 0,
                Capacidad = 0,
                EnergiaPorCiclo = -3,
                DineroPorCiclo = 50,
                Creado = seedDate,
            },
            new TipoEstructuraEF
            {
                Id = 7,
                Nombre = "Energia solar",
                Ocupacion = 0,
                Capacidad = 0,
                EnergiaPorCiclo = 1,
                DineroPorCiclo = -5,
                Creado = seedDate,
            }
        );
    }
}