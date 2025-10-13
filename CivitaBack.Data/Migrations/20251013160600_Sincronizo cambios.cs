using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CivitaBack.Data.Migrations
{
    /// <inheritdoc />
    public partial class Sincronizocambios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Logro_Condicion_CondicionId",
                table: "Logro");

            migrationBuilder.AlterColumn<int>(
                name: "CondicionId",
                table: "Logro",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

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

            migrationBuilder.AlterColumn<int>(
                name: "CondicionId",
                table: "Logro",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

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
