using CivitaBack.Data.BO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CivitaBack.Data.EF.Configurations;

public class TipoLogroConfiguration : IEntityTypeConfiguration<TipoLogroEF>
{
    public void Configure(EntityTypeBuilder<TipoLogroEF> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).ValueGeneratedOnAdd();
        builder.Property(t => t.Nombre).HasMaxLength(200);
    }
}
