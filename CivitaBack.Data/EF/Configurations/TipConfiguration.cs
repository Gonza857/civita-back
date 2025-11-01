using CivitaBack.Data.BO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CivitaBack.Data.EF.Configurations
{
    public class TipConfiguration : IEntityTypeConfiguration<TipEF>
    {
        public void Configure(EntityTypeBuilder<TipEF> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).ValueGeneratedOnAdd();

            builder.Property(t => t.Mensaje).HasMaxLength(500);
            builder.Property(t => t.Expresion).HasMaxLength(200);
            builder.Property(t => t.ElementoAdicional).HasMaxLength(200);

            // Relación con TipoTip
            builder.HasOne(t => t.TipoTip)
                   .WithMany()
                   .HasForeignKey(t => t.TipoId)
                   .OnDelete(DeleteBehavior.Restrict);
            
            var seedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        builder.HasData(
            new TipEF
            {
                Id = 1,
                Mensaje = "¡Hola!, soy Vita ",
                TipoId = 1,
                EfectoFiltro = false,
                ElementoAdicional = "",
                Expresion = "vitaSaluda",
                Creado = seedDate,
            },
            new TipEF
            {
                Id = 2,
                Mensaje = "Te doy la bienvenida a Neocivita ",
                TipoId = 1,
                EfectoFiltro = false,
                ElementoAdicional = "",
                Expresion = "PulgarArribaVita",
                Creado = seedDate,
            },
            new TipEF
            {
                Id = 3,
                Mensaje = "Antes de empezar, ¿cómo te llamás?",
                TipoId = 1,
                EfectoFiltro = false,
                ElementoAdicional = "",
                Expresion = "vitaPregunta",
                Creado = seedDate,
            },
            new TipEF
            {
                Id = 4,
                Mensaje = "En Neocivita aprenderás sobre el impacto ecológico que tienen nuestras decisiones al construir y mantener una ciudad.",
                TipoId = 1,
                EfectoFiltro = false,
                ElementoAdicional = "",
                Expresion = "vitaExplica2",
                Creado = seedDate,
            },
            new TipEF
            {
                Id = 5,
                Mensaje = "¿Creés que podés lograr el equilibrio entre economía, sociedad y ambiente?",
                TipoId = 1,
                EfectoFiltro = false,
                ElementoAdicional = "",
                Expresion = "vitaDecidida",
                Creado = seedDate,
            },
            new TipEF
            {
                Id = 6,
                Mensaje = "Estas son las EcoCoins, la moneda para adquirir construcciones en tu ciudad.",
                TipoId = 1,
                EfectoFiltro = true, // El SQL decía TRUE
                ElementoAdicional = "ecoCoinBrillante",
                Expresion = "vitaExplica2",
                Creado = seedDate,
            },
            new TipEF
            {
                Id = 7,
                Mensaje = "Podés conseguirlas con edificaciones industriales o completando misiones. ¡Acompañame a ver las demás!",
                TipoId = 1,
                EfectoFiltro = false,
                ElementoAdicional = "ecoCoin",
                Expresion = "vitaExplica",
                Creado = seedDate,
            },
            new TipEF
            {
                Id = 8,
                Mensaje = "Te presento la energía eléctrica: podés obtenerla con construcciones específicas que la generen, como un panel solar.",
                TipoId = 1,
                EfectoFiltro = false,
                ElementoAdicional = "energiaCoin",
                Expresion = "vitaCansada",
                Creado = seedDate,
            },
            new TipEF
            {
                Id = 9,
                Mensaje = "Ahora… la CONTAMINACIÓN. Este recurso destruye tu ciudad y el planeta. ¡Tené cuidado!",
                TipoId = 1,
                EfectoFiltro = false,
                ElementoAdicional = "contaminacionCoin",
                Expresion = "vitaPensativa",
                Creado = seedDate,
            },
            new TipEF
            {
                Id = 10,
                Mensaje = "La FELICIDAD refleja qué tan saludable y feliz está tu población. ¡Es muy importante!",
                TipoId = 1,
                EfectoFiltro = false,
                ElementoAdicional = "felicidadCoin",
                Expresion = "vitaDecidida",
                Creado = seedDate,
            },
            new TipEF
            {
                Id = 11,
                Mensaje = "Ahora que conocés los recursos del juego… ¡acompañame a jugar y empecemos a construir!",
                TipoId = 1,
                EfectoFiltro = false,
                ElementoAdicional = "",
                Expresion = "vitaFesteja",
                Creado = seedDate,
            },
            new TipEF
            {
                Id = 12,
                Mensaje = "¡Registrate para empezar a construir nuestra ciudad!",
                TipoId = 1,
                EfectoFiltro = false,
                ElementoAdicional = "",
                Expresion = "vitaFesteja",
                Creado = seedDate,
            }
        );
        }
    }
}
