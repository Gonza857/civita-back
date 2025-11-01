using CivitaBack.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using CivitaBack.Data.BO;

namespace CivitaBack.Data.EF.Configurations
{
    public class LogroPartidaConfiguration : IEntityTypeConfiguration<LogroPartidaEF>
    {
        public void Configure(EntityTypeBuilder<LogroPartidaEF> builder)
        {
            // PK compuesta
            builder.HasKey(lp => new { lp.LogroId, lp.PartidaId });

            builder.HasOne(lp => lp.Logro)
                   .WithMany(l => l.LogroPartidas)
                   .HasForeignKey(lp => lp.LogroId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(lp => lp.Partida)
                   .WithMany(p => p.LogroPartidas)
                   .HasForeignKey(lp => lp.PartidaId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
