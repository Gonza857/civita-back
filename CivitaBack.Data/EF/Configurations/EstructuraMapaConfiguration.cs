using CivitaBack.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CivitaBack.Data.EF.Configurations
{
    public class EstructuraMapaConfiguration : IEntityTypeConfiguration<EstructuraMapa>
    {
        public void Configure(EntityTypeBuilder<EstructuraMapa> builder)
        {
            builder.HasKey(em => em.Id);
            builder.Property(em => em.Id).ValueGeneratedOnAdd();

            builder.HasOne(em => em.Partida)
                   .WithMany(p => p.EstructuraMapa)
                   .HasForeignKey(em => em.PartidaId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(em => em.Estructura)
                   .WithMany(e => e.EstructurasEnMapa)
                   .HasForeignKey(em => em.EstructuraId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
