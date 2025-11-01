using CivitaBack.Data.BO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CivitaBack.Data.EF.Configurations
{
    public class EstructuraConfiguration : IEntityTypeConfiguration<EstructuraEF>
    {
        public void Configure(EntityTypeBuilder<EstructuraEF> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Property(e => e.Nombre)
                .HasMaxLength(200);

            builder.HasOne(e => e.TipoEstructura)
                .WithMany() // la otra entidad no tiene nav inversa
                .HasForeignKey(e => e.TipoEstructuraId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.EstructurasEnMapa)
                .WithOne(em => em.Estructura)
                .HasForeignKey(em => em.EstructuraId);

            var seedDate = new DateTime(2025, 11, 1, 0, 0, 0, DateTimeKind.Utc);

            builder.HasData(
                new EstructuraEF
                {
                    Id = 1, // ¡Id manual es obligatorio!
                    Nombre = "Casa",
                    EsMejorable = false,
                    RutaImagen = "/assets/mapa/casas.png",
                    CostoEnergia = 0,
                    CostoDinero = 25,
                    FelicidadCiclo = 2,
                    ContaminacionCiclo = 2,
                    TipoEstructuraId = 1,
                    Creado = seedDate,
                },
                new EstructuraEF
                {
                    Id = 2,
                    Nombre = "Turbina Eólica",
                    EsMejorable = false,
                    RutaImagen = "/assets/mapa/turbina.png",
                    CostoEnergia = 0,
                    CostoDinero = 50,
                    FelicidadCiclo = 2,
                    ContaminacionCiclo = -4,
                    TipoEstructuraId = 2,
                    Creado = seedDate,
                },
                new EstructuraEF
                {
                    Id = 3,
                    Nombre = "Fabrica",
                    EsMejorable = false,
                    RutaImagen = "/assets/mapa/fabrica.png",
                    CostoEnergia = 0,
                    CostoDinero = 40,
                    FelicidadCiclo = -4,
                    ContaminacionCiclo = 6,
                    TipoEstructuraId = 3,
                    Creado = seedDate,
                },
                new EstructuraEF
                {
                    Id = 4,
                    Nombre = "Iniciales",
                    EsMejorable = false,
                    RutaImagen = null, // SQL usaba 'null'
                    CostoEnergia = 0,
                    CostoDinero = 0,
                    FelicidadCiclo = 0,
                    ContaminacionCiclo = 0,
                    TipoEstructuraId = 4,
                    Creado = seedDate,
                },
                new EstructuraEF
                {
                    Id = 5,
                    Nombre = "InicialesCasa2",
                    EsMejorable = false,
                    RutaImagen = null,
                    CostoEnergia = 0,
                    CostoDinero = 0,
                    FelicidadCiclo = 0,
                    ContaminacionCiclo = 0,
                    TipoEstructuraId = 5,
                    Creado = seedDate,
                },
                new EstructuraEF
                {
                    Id = 6,
                    Nombre = "InicialesEdificio",
                    EsMejorable = false,
                    RutaImagen = null,
                    CostoEnergia = 0,
                    CostoDinero = 0,
                    FelicidadCiclo = 0,
                    ContaminacionCiclo = 0,
                    TipoEstructuraId = 6,
                    Creado = seedDate,
                },
                new EstructuraEF
                {
                    Id = 7,
                    Nombre = "InicialesBase",
                    EsMejorable = false,
                    RutaImagen = null,
                    CostoEnergia = 0,
                    CostoDinero = 0,
                    FelicidadCiclo = 0,
                    ContaminacionCiclo = 0,
                    TipoEstructuraId = 7,
                    Creado = seedDate,
                }
            );
        }
    }
}