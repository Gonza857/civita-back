using CivitaBack.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CivitaBack.Data.EF.Configurations
{
    public class TipoTipConfiguration : IEntityTypeConfiguration<TipoTip>
    {
        public void Configure(EntityTypeBuilder<TipoTip> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).ValueGeneratedOnAdd();

            builder.Property(t => t.Descripcion).HasMaxLength(200);
        }
    }
}
