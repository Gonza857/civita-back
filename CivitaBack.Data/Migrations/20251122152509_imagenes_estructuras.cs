using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CivitaBack.Data.Migrations
{
    /// <inheritdoc />
    public partial class imagenes_estructuras : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 1,
                column: "RutaImagen",
                value: "/Assets/mapa/Estructura-Edificio-3.png");

            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 2,
                column: "RutaImagen",
                value: "/Assets/mapa/Estructura-Molino.png");

            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 3,
                column: "RutaImagen",
                value: "/Assets/mapa/Estructura-Fabrica-Nivel-1.png");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 1,
                column: "RutaImagen",
                value: "/assets/mapa/casas.png");

            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 2,
                column: "RutaImagen",
                value: "/assets/mapa/turbina.png");

            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 3,
                column: "RutaImagen",
                value: "/assets/mapa/fabrica.png");
        }
    }
}
