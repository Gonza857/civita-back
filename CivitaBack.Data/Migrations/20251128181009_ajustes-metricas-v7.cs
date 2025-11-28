using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CivitaBack.Data.Migrations
{
    /// <inheritdoc />
    public partial class ajustesmetricasv7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 2,
                column: "ContaminacionCiclo",
                value: -2);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 2,
                column: "ContaminacionCiclo",
                value: -4);
        }
    }
}
