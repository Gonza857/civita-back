using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CivitaBack.Data.Migrations
{
    /// <inheritdoc />
    public partial class inserts_misiones_condicion_recompensas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Condicion",
                columns: new[] { "Id", "Cantidad", "Creado", "Editado", "EstructuraId", "NombreColumna" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null },
                    { 2, 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, null }
                });

            migrationBuilder.UpdateData(
                table: "Mision",
                keyColumn: "Id",
                keyValue: 1,
                column: "CondicionId",
                value: 1);

            migrationBuilder.InsertData(
                table: "Recompensa",
                columns: new[] { "Id", "Cantidad", "Creado", "Editado", "EstructuraId", "NombreColumna" },
                values: new object[,]
                {
                    { 100, 50, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "EcoCoins" },
                    { 101, 50, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Experiencia" },
                    { 102, 60, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Experiencia" },
                    { 103, 15, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Energia" },
                    { 104, 500, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "EcoCoins" }
                });

            migrationBuilder.InsertData(
                table: "CondicionRecompensa",
                columns: new[] { "CondicionesId", "RecompensasId" },
                values: new object[,]
                {
                    { 1, 100 },
                    { 1, 101 },
                    { 2, 103 },
                    { 2, 104 }
                });

            migrationBuilder.InsertData(
                table: "Mision",
                columns: new[] { "Id", "CondicionId", "Creado", "Descripcion", "Disponible", "Editado", "Tipo", "Titulo" },
                values: new object[] { 2, 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "La ciudad necesita generar ingresos.", true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Diaria", "¡Construye 1 fabrica!" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CondicionRecompensa",
                keyColumns: new[] { "CondicionesId", "RecompensasId" },
                keyValues: new object[] { 1, 100 });

            migrationBuilder.DeleteData(
                table: "CondicionRecompensa",
                keyColumns: new[] { "CondicionesId", "RecompensasId" },
                keyValues: new object[] { 1, 101 });

            migrationBuilder.DeleteData(
                table: "CondicionRecompensa",
                keyColumns: new[] { "CondicionesId", "RecompensasId" },
                keyValues: new object[] { 2, 103 });

            migrationBuilder.DeleteData(
                table: "CondicionRecompensa",
                keyColumns: new[] { "CondicionesId", "RecompensasId" },
                keyValues: new object[] { 2, 104 });

            migrationBuilder.DeleteData(
                table: "Mision",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Recompensa",
                keyColumn: "Id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "Condicion",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Condicion",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Recompensa",
                keyColumn: "Id",
                keyValue: 100);

            migrationBuilder.DeleteData(
                table: "Recompensa",
                keyColumn: "Id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "Recompensa",
                keyColumn: "Id",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "Recompensa",
                keyColumn: "Id",
                keyValue: 104);

            migrationBuilder.UpdateData(
                table: "Mision",
                keyColumn: "Id",
                keyValue: 1,
                column: "CondicionId",
                value: 2);
        }
    }
}
