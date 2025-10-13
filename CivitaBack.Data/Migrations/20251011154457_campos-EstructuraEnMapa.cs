using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CivitaBack.Data.Migrations
{
    /// <inheritdoc />
    public partial class camposEstructuraEnMapa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Partida_UsuarioId",
                table: "Partida");

            migrationBuilder.DropColumn(
                name: "Contaminacion",
                table: "Recurso");

            migrationBuilder.DropColumn(
                name: "Felicidad",
                table: "Recurso");

            migrationBuilder.AddColumn<int>(
                name: "Height",
                table: "EstructuraEnMapa",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Width",
                table: "EstructuraEnMapa",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "X",
                table: "EstructuraEnMapa",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Y",
                table: "EstructuraEnMapa",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Partida_UsuarioId",
                table: "Partida",
                column: "UsuarioId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Partida_UsuarioId",
                table: "Partida");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "EstructuraEnMapa");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "EstructuraEnMapa");

            migrationBuilder.DropColumn(
                name: "X",
                table: "EstructuraEnMapa");

            migrationBuilder.DropColumn(
                name: "Y",
                table: "EstructuraEnMapa");

            migrationBuilder.AddColumn<int>(
                name: "Contaminacion",
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
                name: "IX_Partida_UsuarioId",
                table: "Partida",
                column: "UsuarioId");
        }
    }
}
