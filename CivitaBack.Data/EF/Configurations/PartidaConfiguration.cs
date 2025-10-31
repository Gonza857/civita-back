using CivitaBack.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CivitaBack.Data.EF.Configurations
{
    public class PartidaConfiguration : IEntityTypeConfiguration<Partida>
    {
        public void Configure(EntityTypeBuilder<Partida> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.JsonMapa)
                   .HasColumnType("text"); // si es JSON grande conviene TEXT

            // Relación con Usuario
            builder.HasOne(p => p.Usuario)
            .WithOne(u => u.Partida)      
            .HasForeignKey<Partida>(p => p.UsuarioId) 
            .OnDelete(DeleteBehavior.Cascade);

            // Recurso 1:1 opcional
            builder.HasOne(p => p.Recursos)
                   .WithOne(r => r.Partida)
                   .HasForeignKey<Recurso>(r => r.PartidaId)
                   .OnDelete(DeleteBehavior.Cascade);

            // 1:N con EstructuraMapa
            builder.HasMany(p => p.EstructuraMapa)
                   .WithOne(em => em.Partida)
                   .HasForeignKey(em => em.PartidaId);
        }
    }
}
