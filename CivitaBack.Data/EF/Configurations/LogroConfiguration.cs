using CivitaBack.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CivitaBack.Data.EF.Configurations
{
    public class LogroConfiguration : IEntityTypeConfiguration<Logro>
    {
        public void Configure(EntityTypeBuilder<Logro> builder)
        {
            builder.HasKey(l => l.Id);
            builder.Property(l => l.Id).ValueGeneratedOnAdd();

            builder.Property(l => l.Titulo).HasMaxLength(200);
            builder.Property(l => l.Descripcion).HasMaxLength(500);

            // Relación con TipoLogro
            builder.HasOne(l => l.TipoLogro)
                   .WithMany()
                   .OnDelete(DeleteBehavior.Restrict);

            // Relación con Condicion
            builder.HasOne(l => l.Condicion)
                   .WithMany()
                   .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
