using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CivitaBack.Data.Migrations
{
    /// <inheritdoc />
    public partial class recompensas01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Condicion_Condicion_RecompensaId",
                table: "Condicion");

            migrationBuilder.DropIndex(
                name: "IX_Condicion_RecompensaId",
                table: "Condicion");

            migrationBuilder.DeleteData(
                table: "Condicion",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Condicion",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "EsRecompensa",
                table: "Condicion");

            migrationBuilder.DropColumn(
                name: "RecompensaId",
                table: "Condicion");

            migrationBuilder.AddColumn<int>(
                name: "Experiencia",
                table: "Partida",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Nivel",
                table: "Partida",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Recompensa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Cantidad = table.Column<int>(type: "integer", nullable: false),
                    NombreColumna = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    EstructuraId = table.Column<int>(type: "integer", nullable: true),
                    Creado = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Editado = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recompensa", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recompensa_Estructura_EstructuraId",
                        column: x => x.EstructuraId,
                        principalTable: "Estructura",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CondicionRecompensa",
                columns: table => new
                {
                    CondicionesId = table.Column<int>(type: "integer", nullable: false),
                    RecompensasId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CondicionRecompensa", x => new { x.CondicionesId, x.RecompensasId });
                    table.ForeignKey(
                        name: "FK_CondicionRecompensa_Condicion_CondicionesId",
                        column: x => x.CondicionesId,
                        principalTable: "Condicion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CondicionRecompensa_Recompensa_RecompensasId",
                        column: x => x.RecompensasId,
                        principalTable: "Recompensa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CondicionRecompensa_RecompensasId",
                table: "CondicionRecompensa",
                column: "RecompensasId");

            migrationBuilder.CreateIndex(
                name: "IX_Recompensa_EstructuraId",
                table: "Recompensa",
                column: "EstructuraId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CondicionRecompensa");

            migrationBuilder.DropTable(
                name: "Recompensa");

            migrationBuilder.DropColumn(
                name: "Experiencia",
                table: "Partida");

            migrationBuilder.DropColumn(
                name: "Nivel",
                table: "Partida");

            migrationBuilder.AddColumn<bool>(
                name: "EsRecompensa",
                table: "Condicion",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "RecompensaId",
                table: "Condicion",
                type: "integer",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Condicion",
                columns: new[] { "Id", "Cantidad", "Creado", "Editado", "EsRecompensa", "EstructuraId", "NombreColumna", "RecompensaId" },
                values: new object[,]
                {
                    { 1, 4444, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, null, "EcoCoins", null },
                    { 2, 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false, 1, null, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Condicion_RecompensaId",
                table: "Condicion",
                column: "RecompensaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Condicion_Condicion_RecompensaId",
                table: "Condicion",
                column: "RecompensaId",
                principalTable: "Condicion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
