using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CivitaBack.Data.Migrations
{
    /// <inheritdoc />
    public partial class tip_refactor : Migration
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
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tip_TipoTip_TipoTipId",
                table: "Tip",
                column: "TipoTipId",
                principalTable: "TipoTip",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddForeignKey(
                name: "FK_Tip_TipoTip_TipoTipId",
                table: "Tip",
                column: "TipoTipId",
                principalTable: "TipoTip",
                principalColumn: "Id");
        }
    }
}
