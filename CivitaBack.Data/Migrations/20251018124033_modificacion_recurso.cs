using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CivitaBack.Data.Migrations
{
    /// <inheritdoc />
    public partial class modificacion_recurso : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Recurso_PartidaId",
                table: "Recurso");

            migrationBuilder.RenameColumn(
                name: "Cantidad",
                table: "Recurso",
                newName: "Poblacion");

            migrationBuilder.AddColumn<int>(
                name: "Contaminacion",
                table: "Recurso",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EcoCoins",
                table: "Recurso",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Energia",
                table: "Recurso",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Felicidad",
                table: "Recurso",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Recurso_PartidaId",
                table: "Recurso",
                column: "PartidaId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Recurso_PartidaId",
                table: "Recurso");

            migrationBuilder.DropColumn(
                name: "Contaminacion",
                table: "Recurso");

            migrationBuilder.DropColumn(
                name: "EcoCoins",
                table: "Recurso");

            migrationBuilder.DropColumn(
                name: "Energia",
                table: "Recurso");

            migrationBuilder.DropColumn(
                name: "Felicidad",
                table: "Recurso");

            migrationBuilder.RenameColumn(
                name: "Poblacion",
                table: "Recurso",
                newName: "Cantidad");

            migrationBuilder.CreateIndex(
                name: "IX_Recurso_PartidaId",
                table: "Recurso",
                column: "PartidaId");
        }
    }
}
