using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CivitaBack.Data.Migrations
{
    /// <inheritdoc />
    public partial class tienda_refactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodigoArticulo",
                table: "Tienda");

            migrationBuilder.DropColumn(
                name: "NombreArticulo",
                table: "Tienda");

            migrationBuilder.AddColumn<int>(
                name: "EstructuraId",
                table: "Tienda",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tienda_EstructuraId",
                table: "Tienda",
                column: "EstructuraId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tienda_Estructura_EstructuraId",
                table: "Tienda",
                column: "EstructuraId",
                principalTable: "Estructura",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tienda_Estructura_EstructuraId",
                table: "Tienda");

            migrationBuilder.DropIndex(
                name: "IX_Tienda_EstructuraId",
                table: "Tienda");

            migrationBuilder.DropColumn(
                name: "EstructuraId",
                table: "Tienda");

            migrationBuilder.AddColumn<string>(
                name: "CodigoArticulo",
                table: "Tienda",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NombreArticulo",
                table: "Tienda",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);
        }
    }
}
