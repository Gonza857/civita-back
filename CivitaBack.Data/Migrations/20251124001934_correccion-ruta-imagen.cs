using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CivitaBack.Data.Migrations
{
    /// <inheritdoc />
    public partial class correccionrutaimagen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 4,
                column: "RutaImagen",
                value: "/Assets/mapa/Estructura-Parque-Pequeño.png");

            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 5,
                column: "RutaImagen",
                value: "/Assets/mapa/Estructura-Hotel.png");

            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 6,
                column: "RutaImagen",
                value: "/Assets/mapa/Estructura-Fabrica-Nivel-2.png");

            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 7,
                column: "RutaImagen",
                value: "/Assets/mapa/Estructura-Panel-Solar.png");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 4,
                column: "RutaImagen",
                value: "Estructura-Parque-Pequeño.png");

            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 5,
                column: "RutaImagen",
                value: "Estructura-Hotel.png");

            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 6,
                column: "RutaImagen",
                value: "Estructura-Fabrica-Nivel-2.png");

            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 7,
                column: "RutaImagen",
                value: "Estructura-Panel-Solar.png");
        }
    }
}
