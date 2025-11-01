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
        }
    }
}
