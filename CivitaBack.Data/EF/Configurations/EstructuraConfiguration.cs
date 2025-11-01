using CivitaBack.Data.BO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CivitaBack.Data.EF.Configurations
{
    public class EstructuraConfiguration : IEntityTypeConfiguration<EstructuraEF>
    {
        public void Configure(EntityTypeBuilder<EstructuraEF> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Property(e => e.Nombre)
                   .HasMaxLength(200);

            builder.HasOne(e => e.TipoEstructura)
                   .WithMany() // la otra entidad no tiene nav inversa
                   .HasForeignKey(e => e.TipoEstructuraId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.EstructurasEnMapa)
                   .WithOne(em => em.Estructura)
                   .HasForeignKey(em => em.EstructuraId);
        }
    }
}
