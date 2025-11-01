using CivitaBack.Data.BO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CivitaBack.Data.EF.Configurations;

public class TipoEstructuraConfiguration : IEntityTypeConfiguration<TipoEstructuraEF>
{
    public void Configure(EntityTypeBuilder<TipoEstructuraEF> builder)
    {
        builder.HasKey(te => te.Id);
        builder.Property(te => te.Id).ValueGeneratedOnAdd();
        builder.Property(te => te.Nombre).HasMaxLength(200);

        // 1:N con Estructura
        builder.HasMany(te => te.Estructura)
            .WithOne(e => e.TipoEstructura)
            .HasForeignKey(e => e.TipoEstructuraId);
    }
}