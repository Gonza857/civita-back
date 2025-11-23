using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CivitaBack.Data.Migrations
{
    /// <inheritdoc />
    public partial class tipsMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Orden",
                table: "Tip",
                type: "integer",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Tip",
                keyColumn: "Id",
                keyValue: 1,
                column: "Orden",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tip",
                keyColumn: "Id",
                keyValue: 2,
                column: "Orden",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tip",
                keyColumn: "Id",
                keyValue: 3,
                column: "Orden",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tip",
                keyColumn: "Id",
                keyValue: 4,
                column: "Orden",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tip",
                keyColumn: "Id",
                keyValue: 5,
                column: "Orden",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tip",
                keyColumn: "Id",
                keyValue: 6,
                column: "Orden",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tip",
                keyColumn: "Id",
                keyValue: 7,
                column: "Orden",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tip",
                keyColumn: "Id",
                keyValue: 8,
                column: "Orden",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tip",
                keyColumn: "Id",
                keyValue: 9,
                column: "Orden",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tip",
                keyColumn: "Id",
                keyValue: 10,
                column: "Orden",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tip",
                keyColumn: "Id",
                keyValue: 11,
                column: "Orden",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tip",
                keyColumn: "Id",
                keyValue: 12,
                column: "Orden",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Orden",
                table: "Tip");
        }
    }
}
