using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CivitaBack.Data.Migrations
{
    /// <inheritdoc />
    public partial class condicion_logro_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Condicion_Estructura_EstructuraId",
                table: "Condicion");

            migrationBuilder.AlterColumn<int>(
                name: "EstructuraId",
                table: "Condicion",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Condicion_Estructura_EstructuraId",
                table: "Condicion",
                column: "EstructuraId",
                principalTable: "Estructura",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Condicion_Estructura_EstructuraId",
                table: "Condicion");

            migrationBuilder.AlterColumn<int>(
                name: "EstructuraId",
                table: "Condicion",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Condicion_Estructura_EstructuraId",
                table: "Condicion",
                column: "EstructuraId",
                principalTable: "Estructura",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
