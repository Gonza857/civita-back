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
                    Id = 1, 
                    Nombre = "Casa",
                    EsMejorable = false,
                    RutaImagen = "/Assets/mapa/Estructura-Edificio-3.png",
                    CostoEnergia = 0,
                    CostoDinero = 40,
                    FelicidadCiclo = 1,
                    ContaminacionCiclo = 0,
                    TipoEstructuraId = 1,
                    Creado = seedDate,
                },
                new EstructuraEF
                {
                    Id = 2,
                    Nombre = "Turbina Eólica",
                    EsMejorable = false,
                    RutaImagen = "/Assets/mapa/Estructura-Molino.png",
                    CostoEnergia = 0,
                    CostoDinero = 300,
                    FelicidadCiclo = -1,
                    ContaminacionCiclo = -2,
                    TipoEstructuraId = 2,
                    Creado = seedDate,
                },
                new EstructuraEF
                {
                    Id = 3,
                    Nombre = "Fábrica",
                    EsMejorable = false,
                    RutaImagen = "/Assets/mapa/Estructura-Fabrica-Nivel-1.png",
                    CostoEnergia = 0,
                    CostoDinero = 50,
                    FelicidadCiclo = -1,
                    ContaminacionCiclo = 2,
                    TipoEstructuraId = 3,
                    Creado = seedDate,
                },
                new EstructuraEF
                {
                    Id = 4,
                    Nombre = "Parque",
                    EsMejorable = false,
                    RutaImagen = "/Assets/mapa/Estructura-Parque-Pequeño.png", // SQL usaba 'null'
                    CostoEnergia = 0,
                    CostoDinero = 30,
                    FelicidadCiclo = 1,
                    ContaminacionCiclo = 0,
                    TipoEstructuraId = 4,
                    Creado = seedDate,
                },
                new EstructuraEF
                {
                    Id = 5,
                    Nombre = "Hotel",
                    EsMejorable = false,
                    RutaImagen = "/Assets/mapa/Estructura-Hotel.png",
                    CostoEnergia = 0,
                    CostoDinero = 150,
                    FelicidadCiclo = 2,
                    ContaminacionCiclo = 1,
                    TipoEstructuraId = 5,
                    Creado = seedDate,
                },
                new EstructuraEF
                {
                    Id = 6,
                    Nombre = "Planta Neocorp",
                    EsMejorable = false,
                    RutaImagen = "/Assets/mapa/Estructura-Fabrica-Nivel-2.png",
                    CostoEnergia = 0,
                    CostoDinero = 400,
                    FelicidadCiclo = 2,
                    ContaminacionCiclo = -2,
                    TipoEstructuraId = 6,
                    Creado = seedDate,
                },
                new EstructuraEF
                {
                    Id = 7,
                    Nombre = "Panel solar",
                    EsMejorable = false,
                    RutaImagen = "/Assets/mapa/Estructura-Panel-Solar.png",
                    CostoEnergia = 0,
                    CostoDinero = 80,
                    FelicidadCiclo = 0,
                    ContaminacionCiclo = -1,
                    TipoEstructuraId = 7,
                    Creado = seedDate,
                }
            );
        }
    }
}