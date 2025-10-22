using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CivitaBack.Data.Migrations
{
    /// <inheritdoc />
    public partial class recompensa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EsRecompensa",
                table: "Condicion",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RecompensaId",
                table: "Condicion",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Condicion_RecompensaId",
                table: "Condicion",
                column: "RecompensaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Condicion_Condicion_RecompensaId",
                table: "Condicion",
                column: "RecompensaId",
                principalTable: "Condicion",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Condicion_Condicion_RecompensaId",
                table: "Condicion");

            migrationBuilder.DropIndex(
                name: "IX_Condicion_RecompensaId",
                table: "Condicion");

            migrationBuilder.DropColumn(
                name: "EsRecompensa",
                table: "Condicion");

            migrationBuilder.DropColumn(
                name: "RecompensaId",
                table: "Condicion");
        }
    }
}
