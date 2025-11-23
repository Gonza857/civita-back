using CivitaBack.Data.BO;
using CivitaBack.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CivitaBack.Data.EF.Configurations
{
    public class CondicionConfiguration : IEntityTypeConfiguration<CondicionEF>
    {
        public void Configure(EntityTypeBuilder<CondicionEF> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();

            // Relación con Estructura (FK opcional)
            builder.HasOne(c => c.Estructura)
                .WithMany()
                .HasForeignKey(c => c.EstructuraId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.Recompensas) 
                .WithMany(r => r.Condiciones);
        }
    }
}