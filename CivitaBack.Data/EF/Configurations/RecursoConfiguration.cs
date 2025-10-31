using CivitaBack.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CivitaBack.Data.EF.Configurations
{
    public class RecursoConfiguration : IEntityTypeConfiguration<Recurso>
    {
        public void Configure(EntityTypeBuilder<Recurso> builder)
        {
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Id).ValueGeneratedOnAdd();

            // Relación 1:1 con Partida
            builder.HasOne(r => r.Partida)
                   .WithOne(p => p.Recursos)
                   .HasForeignKey<Recurso>(r => r.PartidaId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
