using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CivitaBack.Data.Migrations
{
    /// <inheritdoc />
    public partial class ajustes_metricas_v4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ContaminacionCiclo", "CostoDinero" },
                values: new object[] { -4, 500 });

            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 4,
                column: "CostoDinero",
                value: 100);

            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ContaminacionCiclo", "FelicidadCiclo" },
                values: new object[] { 4, -1 });

            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 7,
                column: "CostoDinero",
                value: 100);

            migrationBuilder.UpdateData(
                table: "TipoEstructura",
                keyColumn: "Id",
                keyValue: 2,
                column: "DineroPorCiclo",
                value: -10);

            migrationBuilder.UpdateData(
                table: "TipoEstructura",
                keyColumn: "Id",
                keyValue: 7,
                column: "DineroPorCiclo",
                value: -5);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ContaminacionCiclo", "CostoDinero" },
                values: new object[] { -2, 300 });

            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 4,
                column: "CostoDinero",
                value: 30);

            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ContaminacionCiclo", "FelicidadCiclo" },
                values: new object[] { 2, 0 });

            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 7,
                column: "CostoDinero",
                value: 80);

            migrationBuilder.UpdateData(
                table: "TipoEstructura",
                keyColumn: "Id",
                keyValue: 2,
                column: "DineroPorCiclo",
                value: -5);

            migrationBuilder.UpdateData(
                table: "TipoEstructura",
                keyColumn: "Id",
                keyValue: 7,
                column: "DineroPorCiclo",
                value: -2);
        }
    }
}
