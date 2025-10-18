using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CivitaBack.Data.Migrations
{
    /// <inheritdoc />
    public partial class elimina_columna_nombre_recurso : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nombre",
                table: "Recurso");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Nombre",
                table: "Recurso",
                type: "text",
                nullable: true);
        }
    }
}
