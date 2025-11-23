using AutoMapper;
using CivitaBack.Data.BO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CivitaBack.Data.EF.Configurations;

public class RecompensaConfiguration : IEntityTypeConfiguration<RecompensaEF>
{
    public void Configure(EntityTypeBuilder<RecompensaEF> builder)
    {
        builder.HasKey(r => r.Id);

        // 3. Configuración de Propiedades
        builder.Property(r => r.Cantidad).IsRequired(); // Asumimos que la cantidad siempre es requerida
        builder.Property(r => r.NombreColumna).HasMaxLength(100); // Buena práctica limitar strings. Ajusta el largo.

        // Una Recompensa TIENE MUCHAS Condiciones
        // Una Condicion TIENE MUCHAS Recompensas (asumiendo)
        builder.HasMany(r => r.Condiciones)
            .WithMany(c => c.Recompensas) // Asumo que Condicion tiene: ICollection<Recompensa> Recompensas
            .UsingEntity(j => j.ToTable("CondicionRecompensa")); // Nombre explícito para la tabla intermedia
        
    }
}