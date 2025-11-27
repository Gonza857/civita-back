using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CivitaBack.Data.Migrations
{
    /// <inheritdoc />
    public partial class ajustes_metricas_v5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ContaminacionCiclo", "FelicidadCiclo" },
                values: new object[] { 0, 2 });

            migrationBuilder.UpdateData(
                table: "TipoEstructura",
                keyColumn: "Id",
                keyValue: 2,
                column: "EnergiaPorCiclo",
                value: 5);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ContaminacionCiclo", "FelicidadCiclo" },
                values: new object[] { -1, 3 });

            migrationBuilder.UpdateData(
                table: "TipoEstructura",
                keyColumn: "Id",
                keyValue: 2,
                column: "EnergiaPorCiclo",
                value: 4);
        }
    }
}
