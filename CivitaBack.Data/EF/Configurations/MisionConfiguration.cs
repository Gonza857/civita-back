using CivitaBack.Data.BO;
using CivitaBack.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CivitaBack.Data.EF.Configurations;

public class MisionConfiguration : IEntityTypeConfiguration<MisionEF>
{
    public void Configure(EntityTypeBuilder<MisionEF> builder)
    {
        builder.ToTable("Mision"); // Nueva tabla
        builder.HasKey(m => m.Id);
        
        // Relación con Condicion (IDÉNTICA a la de Logro)
        builder.HasOne(m => m.Condicion)
            .WithMany() // Una condición puede ser usada por muchas misiones
            .HasForeignKey(m => m.CondicionId);
        
        // Guarda el Enum como string (ej. "Diaria", "Semanal")
        builder.Property(m => m.Tipo).HasConversion<string>();

        var seedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        builder.HasData(
            new MisionEF
            {
                Id = 3, // ID de esta misión
                Titulo = "¡Construye 1 casa!",
                Descripcion = "Nuestros ciudadanos necesitan lugar para vivir. Construí 1 casa.",
                Disponible = true,
                Tipo = TipoMision.Diaria,
                CondicionId = 1, 
                Creado = seedDate,
                Editado = seedDate
            },
            new MisionEF
            {
                Id = 2, // ID de esta misión
                Titulo = "¡Construye 1 fabrica!",
                Descripcion = "La ciudad necesita generar ingresos.",
                Disponible = true,
                Tipo = TipoMision.Diaria,
                CondicionId = 2, 
                Creado = seedDate,
                Editado = seedDate
            }
        );
    }
}