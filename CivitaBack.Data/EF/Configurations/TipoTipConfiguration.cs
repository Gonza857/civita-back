using CivitaBack.Data.BO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CivitaBack.Data.EF.Configurations
{
    public class TipoTipConfiguration : IEntityTypeConfiguration<TipoTipEF>
    {
        public void Configure(EntityTypeBuilder<TipoTipEF> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).ValueGeneratedOnAdd();

            builder.Property(t => t.Descripcion).HasMaxLength(200);
        }
    }
}