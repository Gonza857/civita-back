using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CivitaBack.Data.Migrations
{
    /// <inheritdoc />
    public partial class balance_estructuras : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 3,
                column: "Nombre",
                value: "Fábrica");

            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 3,
                column: "ContaminacionCiclo",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 5,
                column: "ContaminacionCiclo",
                value: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 3,
                column: "Nombre",
                value: "Fabrica");

            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 3,
                column: "ContaminacionCiclo",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 5,
                column: "ContaminacionCiclo",
                value: 3);
        }
    }
}
