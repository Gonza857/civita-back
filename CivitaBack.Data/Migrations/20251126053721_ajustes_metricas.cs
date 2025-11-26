using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CivitaBack.Data.Migrations
{
    /// <inheritdoc />
    public partial class ajustes_metricas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CostoDinero", "FelicidadCiclo" },
                values: new object[] { 40, 1 });

            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ContaminacionCiclo", "FelicidadCiclo" },
                values: new object[] { -2, -1 });

            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CostoDinero", "FelicidadCiclo" },
                values: new object[] { 50, -2 });

            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CostoDinero", "FelicidadCiclo" },
                values: new object[] { 30, 1 });

            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ContaminacionCiclo", "FelicidadCiclo" },
                values: new object[] { 2, 2 });

            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 6,
                column: "ContaminacionCiclo",
                value: -3);

            migrationBuilder.UpdateData(
                table: "Tip",
                keyColumn: "Id",
                keyValue: 1,
                column: "Orden",
                value: 1);

            migrationBuilder.UpdateData(
                table: "TipoEstructura",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DineroPorCiclo", "EnergiaPorCiclo" },
                values: new object[] { -5, 3 });

            migrationBuilder.UpdateData(
                table: "TipoEstructura",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DineroPorCiclo", "EnergiaPorCiclo" },
                values: new object[] { 10, -2 });

            migrationBuilder.UpdateData(
                table: "TipoEstructura",
                keyColumn: "Id",
                keyValue: 4,
                column: "DineroPorCiclo",
                value: -2);

            migrationBuilder.UpdateData(
                table: "TipoEstructura",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "DineroPorCiclo", "EnergiaPorCiclo" },
                values: new object[] { 5, -1 });

            migrationBuilder.UpdateData(
                table: "TipoEstructura",
                keyColumn: "Id",
                keyValue: 7,
                column: "DineroPorCiclo",
                value: -2);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CostoDinero", "FelicidadCiclo" },
                values: new object[] { 25, 2 });

            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ContaminacionCiclo", "FelicidadCiclo" },
                values: new object[] { -3, 0 });

            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CostoDinero", "FelicidadCiclo" },
                values: new object[] { 40, -4 });

            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CostoDinero", "FelicidadCiclo" },
                values: new object[] { 20, 2 });

            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ContaminacionCiclo", "FelicidadCiclo" },
                values: new object[] { 1, 5 });

            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 6,
                column: "ContaminacionCiclo",
                value: -5);

            migrationBuilder.UpdateData(
                table: "Tip",
                keyColumn: "Id",
                keyValue: 1,
                column: "Orden",
                value: null);

            migrationBuilder.UpdateData(
                table: "TipoEstructura",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DineroPorCiclo", "EnergiaPorCiclo" },
                values: new object[] { -12, 5 });

            migrationBuilder.UpdateData(
                table: "TipoEstructura",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DineroPorCiclo", "EnergiaPorCiclo" },
                values: new object[] { 30, -4 });

            migrationBuilder.UpdateData(
                table: "TipoEstructura",
                keyColumn: "Id",
                keyValue: 4,
                column: "DineroPorCiclo",
                value: -3);

            migrationBuilder.UpdateData(
                table: "TipoEstructura",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "DineroPorCiclo", "EnergiaPorCiclo" },
                values: new object[] { 10, -2 });

            migrationBuilder.UpdateData(
                table: "TipoEstructura",
                keyColumn: "Id",
                keyValue: 7,
                column: "DineroPorCiclo",
                value: -5);
        }
    }
}
