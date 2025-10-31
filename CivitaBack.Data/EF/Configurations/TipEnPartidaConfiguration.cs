using CivitaBack.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CivitaBack.Data.EF.Configurations
{
    public class TipEnPartidaConfiguration : IEntityTypeConfiguration<TipEnPartida>
    {
        public void Configure(EntityTypeBuilder<TipEnPartida> builder)
        {
            builder.HasKey(tp => tp.Id);
            builder.Property(tp => tp.Id).ValueGeneratedOnAdd();

            // Relación con Partida
            builder.HasOne(tp => tp.Partida)
                   .WithMany(p => p.TipEnPartida)
                   .HasForeignKey(tp => tp.PartidaId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Relación con Tip
            builder.HasOne(tp => tp.Tip)
                   .WithMany(t => t.TipEnPartida)
                   .HasForeignKey(tp => tp.TipId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
