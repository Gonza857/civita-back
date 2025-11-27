using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CivitaBack.Data.Migrations
{
    /// <inheritdoc />
    public partial class ajustes_metricas_v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 1,
                column: "ContaminacionCiclo",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ContaminacionCiclo", "FelicidadCiclo" },
                values: new object[] { 2, -1 });

            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 5,
                column: "ContaminacionCiclo",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ContaminacionCiclo", "FelicidadCiclo" },
                values: new object[] { -2, 2 });

            migrationBuilder.UpdateData(
                table: "TipoEstructura",
                keyColumn: "Id",
                keyValue: 2,
                column: "EnergiaPorCiclo",
                value: 2);

            migrationBuilder.UpdateData(
                table: "TipoEstructura",
                keyColumn: "Id",
                keyValue: 7,
                column: "EnergiaPorCiclo",
                value: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 1,
                column: "ContaminacionCiclo",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ContaminacionCiclo", "FelicidadCiclo" },
                values: new object[] { 3, -2 });

            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 5,
                column: "ContaminacionCiclo",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ContaminacionCiclo", "FelicidadCiclo" },
                values: new object[] { -3, 3 });

            migrationBuilder.UpdateData(
                table: "TipoEstructura",
                keyColumn: "Id",
                keyValue: 2,
                column: "EnergiaPorCiclo",
                value: 3);

            migrationBuilder.UpdateData(
                table: "TipoEstructura",
                keyColumn: "Id",
                keyValue: 7,
                column: "EnergiaPorCiclo",
                value: 2);
        }
    }
}
