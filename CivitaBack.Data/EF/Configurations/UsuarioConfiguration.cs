using CivitaBack.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CivitaBack.Data.EF.Configurations
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).ValueGeneratedOnAdd();

            builder.Property(u => u.NombreUsuario).HasMaxLength(100);
            builder.Property(u => u.Mail).HasMaxLength(150);
            builder.Property(u => u.HashDeContrasena).HasMaxLength(250);

            // 1:1 con Partida
            builder.HasOne(u => u.Partida)
            .WithOne(p => p.Usuario)
            .HasForeignKey<Partida>(p => p.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
