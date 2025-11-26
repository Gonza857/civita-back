using AutoMapper;
using CivitaBack.Data.BO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CivitaBack.Data.EF.Configurations;

public class RecompensaConfiguration : IEntityTypeConfiguration<RecompensaEF>
{
    public void Configure(EntityTypeBuilder<RecompensaEF> builder)
    {
        builder.HasKey(r => r.Id);

        // Configuración de Propiedades
        builder.Property(r => r.Cantidad).IsRequired(); // Asumimos que la cantidad siempre es requerida
        builder.Property(r => r.NombreColumna).HasMaxLength(100); // Buena práctica limitar strings. Ajusta el largo.

        // Una Recompensa TIENE MUCHAS Condiciones
        // Una Condicion TIENE MUCHAS Recompensas (asumiendo)
        builder.HasMany(r => r.Condiciones)
            .WithMany(c => c.Recompensas) // Asumo que Condicion tiene: ICollection<Recompensa> Recompensas
            .UsingEntity(j => j.ToTable("CondicionRecompensa")
            
                // 3. ¡AQUÍ ESTÁ EL SEEDING! Insertamos las filas de la relación.
                // EF sabe que 'CondicionId' y 'RecompensaId' son las claves foráneas.
                .HasData(
                    new { CondicionesId = 1, RecompensasId = 100 }, 
                    new { CondicionesId = 1, RecompensasId = 101 },
                    new { CondicionesId = 2, RecompensasId = 103 },
                    new { CondicionesId = 2, RecompensasId = 104 }
                )
            );
        
        var seedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        builder.HasData(
            new RecompensaEF
            {
                Id = 100, 
                Cantidad = 50,
                NombreColumna = "EcoCoins",
                EstructuraId = null,
                Creado = seedDate,
                Editado = seedDate
            },
            new RecompensaEF
            {
                Id = 101, 
                Cantidad = 50,
                NombreColumna = "Experiencia", 
                EstructuraId = null,
                Creado = seedDate,
                Editado = seedDate
            },
            new RecompensaEF
            {
                Id = 102, 
                Cantidad = 60,
                NombreColumna = "Experiencia", 
                EstructuraId = null,
                Creado = seedDate,
                Editado = seedDate
            },
            new RecompensaEF
            {
                Id = 103,
                Cantidad = 15,
                NombreColumna = "Energia", 
                EstructuraId = null,
                Creado = seedDate,
                Editado = seedDate
            },
            new RecompensaEF
            {
                Id = 104,
                Cantidad = 500,
                NombreColumna = "EcoCoins", 
                EstructuraId = null,
                Creado = seedDate,
                Editado = seedDate
            }
        );
        
    }
}