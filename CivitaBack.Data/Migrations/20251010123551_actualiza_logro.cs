using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CivitaBack.Data.Migrations
{
    /// <inheritdoc />
    public partial class actualiza_logro : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Logro_Condicion_CondicionId",
                table: "Logro");

            migrationBuilder.DropIndex(
                name: "IX_Partida_UsuarioId",
                table: "Partida");

            migrationBuilder.DropColumn(
                name: "Contaminacion",
                table: "Recurso");

            migrationBuilder.DropColumn(
                name: "Felicidad",
                table: "Recurso");

            migrationBuilder.AlterColumn<int>(
                name: "CondicionId",
                table: "Logro",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateIndex(
                name: "IX_Partida_UsuarioId",
                table: "Partida",
                column: "UsuarioId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Logro_Condicion_CondicionId",
                table: "Logro",
                column: "CondicionId",
                principalTable: "Condicion",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Logro_Condicion_CondicionId",
                table: "Logro");

            migrationBuilder.DropIndex(
                name: "IX_Partida_UsuarioId",
                table: "Partida");

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

            migrationBuilder.AlterColumn<int>(
                name: "CondicionId",
                table: "Logro",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Partida_UsuarioId",
                table: "Partida",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Logro_Condicion_CondicionId",
                table: "Logro",
                column: "CondicionId",
                principalTable: "Condicion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
