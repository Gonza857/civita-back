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
                table: "EstructuraMapa",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Width",
                table: "EstructuraMapa",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "X",
                table: "EstructuraMapa",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Y",
                table: "EstructuraMapa",
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
                table: "EstructuraMapa");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "EstructuraMapa");

            migrationBuilder.DropColumn(
                name: "X",
                table: "EstructuraMapa");

            migrationBuilder.DropColumn(
                name: "Y",
                table: "EstructuraMapa");
            
            migrationBuilder.CreateIndex(
                name: "IX_Partida_UsuarioId",
                table: "Partida",
                column: "UsuarioId");
        }
    }
}
