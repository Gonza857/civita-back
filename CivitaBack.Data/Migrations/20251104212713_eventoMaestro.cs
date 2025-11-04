using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CivitaBack.Data.Migrations
{
    /// <inheritdoc />
    public partial class eventoMaestro : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "EventoMaestro",
                columns: new[] { "Id", "ContaminacionAceptar", "ContaminacionRechazar", "Creado", "EcoCoinsAceptar", "Editado", "FelicidadAceptar", "FelicidadRechazar", "Nombre", "TextoAceptar", "TextoDescripcion", "TextoRechazar" },
                values: new object[] { 1, -10, 20, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), -100, null, 10, -10, "Separación de residuos", "En Argentina, solo el 3% de los residuos se reciclan. Separar la basura reduce rellenos sanitarios y emisiones de metano.", "Los vecinos solicitan un sistema de reciclaje en la ciudad debido a la alta contaminación.", "Cuando no se recicla, los rellenos sanitarios crecen y emiten metano, un gas 28 veces peor que el CO₂ para el clima." });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "EventoMaestro",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
