using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CivitaBack.Data.Migrations
{
    /// <inheritdoc />
    public partial class correccion_misiones_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Mision",
                keyColumn: "Id",
                keyValue: 1,
                column: "Descripcion",
                value: "Nuestros ciudadanos necesitan lugar para vivir. Construí 1 casa.");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Mision",
                keyColumn: "Id",
                keyValue: 1,
                column: "Descripcion",
                value: "Nuestros ciudadanos necesitan lugar para vivir. Construye 1 casa.");
        }
    }
}
