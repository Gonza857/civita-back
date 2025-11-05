using CivitaBack.Data.BO;
using CivitaBack.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CivitaBack.Data.EF.Configurations
{
    public class CondicionConfiguration : IEntityTypeConfiguration<CondicionEF>
    {
        public void Configure(EntityTypeBuilder<CondicionEF> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();

            // Relación con Estructura (FK opcional)
            builder.HasOne(c => c.Estructura)
                .WithMany()
                .HasForeignKey(c => c.EstructuraId)
                .OnDelete(DeleteBehavior.Restrict);

            // Self-reference de Recompensa
            builder.HasOne(c => c.Recompensa)
                .WithMany(c => c.CondicionesAsociadas)
                .HasForeignKey(c => c.RecompensaId)
                .OnDelete(DeleteBehavior.Restrict);
            
            var seedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            
            builder.HasData(
                // --- RECOMPENSAS ---
                new CondicionEF
                {
                    Id = 1,
                    EsRecompensa = true,
                    NombreColumna = "EcoCoins",
                    Cantidad = 4444,
                    Creado = seedDate,
                    Editado = seedDate
                },
                // --- CONDICIONES ---
                // Ahora defines las condiciones y usas los IDs de arriba
                new CondicionEF
                {
                    Id = 2, // Nuevo ID para esta entidad
                    EsRecompensa = false,
                    NombreColumna = null,
                    EstructuraId = 1,
                    Cantidad = 1,
                    RecompensaId = 1, 
                    Creado = seedDate,
                    Editado = seedDate
                }
            );
        }
    }
}