using CivitaBack.Data.BO;
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
    }
}