using CivitaBack.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CivitaBack.Data.EF.Configurations
{
    public class CondicionConfiguration : IEntityTypeConfiguration<Condicion>
    {
        public void Configure(EntityTypeBuilder<Condicion> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();

            // Relación con Estructura (FK opcional)
            builder.HasOne(c => c.Estructura)
                   .WithMany()
                   .HasForeignKey(c => c.EstructuraId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Self-reference de Recompensa
            builder.HasOne(c => c.Recompensa)
                   .WithMany(c => c.CondicionesAsociadas)
                   .HasForeignKey(c => c.RecompensaId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
