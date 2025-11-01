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
                Capacidad = 0,
                EnergiaPorCiclo = -2,
                DineroPorCiclo = -3,
                Creado = seedDate,
            },
            new TipoEstructuraEF
            {
                Id = 2,
                Nombre = "Energia",
                Ocupacion = 0,
                Capacidad = 0,
                EnergiaPorCiclo = 10,
                DineroPorCiclo = -7,
                Creado = seedDate,
            },
            new TipoEstructuraEF
            {
                Id = 3,
                Nombre = "Industrial",
                Ocupacion = 0,
                Capacidad = 0,
                EnergiaPorCiclo = -4,
                DineroPorCiclo = 50,
                Creado = seedDate,
            },
            new TipoEstructuraEF
            {
                Id = 4,
                Nombre = "Inicial",
                Ocupacion = 0,
                Capacidad = 0,
                EnergiaPorCiclo = 0,
                DineroPorCiclo = 0,
                Creado = seedDate,
            },
            new TipoEstructuraEF
            {
                Id = 5,
                Nombre = "InicialCasa2",
                Ocupacion = 0,
                Capacidad = 0,
                EnergiaPorCiclo = 0,
                DineroPorCiclo = 0,
                Creado = seedDate,
            },
            new TipoEstructuraEF
            {
                Id = 6,
                Nombre = "InicialEdificio",
                Ocupacion = 0,
                Capacidad = 0,
                EnergiaPorCiclo = 0,
                DineroPorCiclo = 0,
                Creado = seedDate,
            },
            new TipoEstructuraEF
            {
                Id = 7,
                Nombre = "InicialBase",
                Ocupacion = 0,
                Capacidad = 0,
                EnergiaPorCiclo = 0,
                DineroPorCiclo = 0,
                Creado = seedDate,
            }
        );
    }
}