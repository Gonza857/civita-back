using CivitaBack.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace CivitaBack.Data.EF.Configurations
{
    public class LogroPartidaConfiguration : IEntityTypeConfiguration<LogroPartida>
    {
        public void Configure(EntityTypeBuilder<LogroPartida> builder)
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
