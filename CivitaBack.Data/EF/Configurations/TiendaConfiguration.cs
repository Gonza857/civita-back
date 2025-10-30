using CivitaBack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CivitaBack.Data.EF.Configurations
{
    public class TiendaConfiguration : IEntityTypeConfiguration<Tienda>
    {
        public void Configure(EntityTypeBuilder<Tienda> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).ValueGeneratedOnAdd();

            builder.Property(t => t.NombreArticulo).HasMaxLength(200);
            builder.Property(t => t.CodigoArticulo).HasMaxLength(100);

            // Relación con Partida
            builder.HasOne(t => t.Partida)
                   .WithMany(p => p.Tienda)
                   .HasForeignKey(t => t.PartidaId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
