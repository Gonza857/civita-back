using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CivitaBack.Data.Migrations
{
    /// <inheritdoc />
    public partial class datos_con_fecha_seed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "TipoEstructura",
                columns: new[] { "Id", "Capacidad", "Creado", "DineroPorCiclo", "Editado", "EnergiaPorCiclo", "Nombre", "Ocupacion" },
                values: new object[,]
                {
                    { 1, 0, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), -3, null, -2, "Vivienda", 0 },
                    { 2, 0, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), -7, null, 10, "Energia", 0 },
                    { 3, 0, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), 50, null, -4, "Industrial", 0 },
                    { 4, 0, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0, null, 0, "Inicial", 0 },
                    { 5, 0, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0, null, 0, "InicialCasa2", 0 },
                    { 6, 0, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0, null, 0, "InicialEdificio", 0 },
                    { 7, 0, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0, null, 0, "InicialBase", 0 }
                });

            migrationBuilder.InsertData(
                table: "TipoTip",
                columns: new[] { "Id", "Creado", "Descripcion", "Editado" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "info", null },
                    { 2, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "onboarding", null }
                });

            migrationBuilder.InsertData(
                table: "Estructura",
                columns: new[] { "Id", "ContaminacionCiclo", "CostoDinero", "CostoEnergia", "Creado", "Editado", "EsMejorable", "FelicidadCiclo", "Nombre", "RutaImagen", "TipoEstructuraId" },
                values: new object[,]
                {
                    { 1, 2, 25, 0, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, 2, "Casa", "/assets/mapa/casas.png", 1 },
                    { 2, -4, 50, 0, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, 2, "Turbina Eólica", "/assets/mapa/turbina.png", 2 },
                    { 3, 6, 40, 0, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, -4, "Fabrica", "/assets/mapa/fabrica.png", 3 },
                    { 4, 0, 0, 0, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, 0, "Iniciales", null, 4 },
                    { 5, 0, 0, 0, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, 0, "InicialesCasa2", null, 5 },
                    { 6, 0, 0, 0, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, 0, "InicialesEdificio", null, 6 },
                    { 7, 0, 0, 0, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, 0, "InicialesBase", null, 7 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "TipoTip",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TipoTip",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TipoEstructura",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TipoEstructura",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TipoEstructura",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "TipoEstructura",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "TipoEstructura",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "TipoEstructura",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "TipoEstructura",
                keyColumn: "Id",
                keyValue: 7);
        }
    }
}
