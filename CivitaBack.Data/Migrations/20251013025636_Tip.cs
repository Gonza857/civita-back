using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CivitaBack.Data.Migrations
{
    /// <inheritdoc />
    public partial class Tip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tip_TipoTip_TipoTipId",
                table: "Tip");

            migrationBuilder.AlterColumn<int>(
                name: "TipoTipId",
                table: "Tip",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Mensaje",
                table: "Tip",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EfectoFiltro",
                table: "Tip",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ElementoAdicional",
                table: "Tip",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Expresion",
                table: "Tip",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TipoId",
                table: "Tip",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Tip_TipoTip_TipoTipId",
                table: "Tip",
                column: "TipoTipId",
                principalTable: "TipoTip",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tip_TipoTip_TipoTipId",
                table: "Tip");

            migrationBuilder.DropColumn(
                name: "EfectoFiltro",
                table: "Tip");

            migrationBuilder.DropColumn(
                name: "ElementoAdicional",
                table: "Tip");

            migrationBuilder.DropColumn(
                name: "Expresion",
                table: "Tip");

            migrationBuilder.DropColumn(
                name: "TipoId",
                table: "Tip");

            migrationBuilder.AlterColumn<int>(
                name: "TipoTipId",
                table: "Tip",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Mensaje",
                table: "Tip",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_Tip_TipoTip_TipoTipId",
                table: "Tip",
                column: "TipoTipId",
                principalTable: "TipoTip",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
