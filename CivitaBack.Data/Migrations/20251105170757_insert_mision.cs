using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CivitaBack.Data.Migrations
{
    /// <inheritdoc />
    public partial class insert_mision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Mision",
                columns: new[] { "Id", "CondicionId", "Creado", "Descripcion", "Disponible", "Editado", "Tipo", "Titulo" },
                values: new object[] { 1, 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Nuestros ciudadanos necesitan lugar para vivir. Construye 1 casa.", true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Diaria", "¡Construye 1 casa!" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Mision",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
