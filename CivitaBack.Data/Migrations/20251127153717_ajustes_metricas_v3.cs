using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CivitaBack.Data.Migrations
{
    /// <inheritdoc />
    public partial class ajustes_metricas_v3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                keyValue: 2,
                column: "FelicidadCiclo",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ContaminacionCiclo", "FelicidadCiclo" },
                values: new object[] { 3, -2 });

            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ContaminacionCiclo", "FelicidadCiclo" },
                values: new object[] { -1, 3 });

            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ContaminacionCiclo", "FelicidadCiclo" },
                values: new object[] { 2, 0 });

            migrationBuilder.UpdateData(
                table: "TipoEstructura",
                keyColumn: "Id",
                keyValue: 1,
                column: "DineroPorCiclo",
                value: -10);

            migrationBuilder.UpdateData(
                table: "TipoEstructura",
                keyColumn: "Id",
                keyValue: 2,
                column: "EnergiaPorCiclo",
                value: 4);

            migrationBuilder.UpdateData(
                table: "TipoEstructura",
                keyColumn: "Id",
                keyValue: 4,
                column: "EnergiaPorCiclo",
                value: -1);

            migrationBuilder.UpdateData(
                table: "TipoEstructura",
                keyColumn: "Id",
                keyValue: 5,
                column: "DineroPorCiclo",
                value: -5);

            migrationBuilder.UpdateData(
                table: "TipoEstructura",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "DineroPorCiclo", "EnergiaPorCiclo" },
                values: new object[] { 50, -3 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                keyValue: 2,
                column: "FelicidadCiclo",
                value: -1);

            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ContaminacionCiclo", "FelicidadCiclo" },
                values: new object[] { 2, -1 });

            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ContaminacionCiclo", "FelicidadCiclo" },
                values: new object[] { 0, 1 });

            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ContaminacionCiclo", "FelicidadCiclo" },
                values: new object[] { -2, 2 });

            migrationBuilder.UpdateData(
                table: "TipoEstructura",
                keyColumn: "Id",
                keyValue: 1,
                column: "DineroPorCiclo",
                value: 1);

            migrationBuilder.UpdateData(
                table: "TipoEstructura",
                keyColumn: "Id",
                keyValue: 2,
                column: "EnergiaPorCiclo",
                value: 2);

            migrationBuilder.UpdateData(
                table: "TipoEstructura",
                keyColumn: "Id",
                keyValue: 4,
                column: "EnergiaPorCiclo",
                value: 1);

            migrationBuilder.UpdateData(
                table: "TipoEstructura",
                keyColumn: "Id",
                keyValue: 5,
                column: "DineroPorCiclo",
                value: 5);

            migrationBuilder.UpdateData(
                table: "TipoEstructura",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "DineroPorCiclo", "EnergiaPorCiclo" },
                values: new object[] { -10, 0 });
        }
    }
}
