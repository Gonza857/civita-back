using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CivitaBack.Data.Migrations
{
    /// <inheritdoc />
    public partial class condicion_logro : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Condicion_Recurso_RecursoId",
                table: "Condicion");

            migrationBuilder.DropForeignKey(
                name: "FK_Logro_Condicion_CondicionId",
                table: "Logro");

            migrationBuilder.DropIndex(
                name: "IX_Condicion_RecursoId",
                table: "Condicion");

            migrationBuilder.DropColumn(
                name: "RecursoId",
                table: "Condicion");

            migrationBuilder.AlterColumn<int>(
                name: "CondicionId",
                table: "Logro",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NombreColumna",
                table: "Condicion",
                type: "text",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Logro_Condicion_CondicionId",
                table: "Logro",
                column: "CondicionId",
                principalTable: "Condicion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Logro_Condicion_CondicionId",
                table: "Logro");

            migrationBuilder.DropColumn(
                name: "NombreColumna",
                table: "Condicion");

            migrationBuilder.AlterColumn<int>(
                name: "CondicionId",
                table: "Logro",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "RecursoId",
                table: "Condicion",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Condicion_RecursoId",
                table: "Condicion",
                column: "RecursoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Condicion_Recurso_RecursoId",
                table: "Condicion",
                column: "RecursoId",
                principalTable: "Recurso",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Logro_Condicion_CondicionId",
                table: "Logro",
                column: "CondicionId",
                principalTable: "Condicion",
                principalColumn: "Id");
        }
    }
}
