using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CivitaBack.Data.Migrations
{
    /// <inheritdoc />
    public partial class logros3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Condicion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Cantidad = table.Column<int>(type: "integer", nullable: false),
                    EstructuraId = table.Column<int>(type: "integer", nullable: false),
                    RecursoId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Condicion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Condicion_Estructura_EstructuraId",
                        column: x => x.EstructuraId,
                        principalTable: "Estructura",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Condicion_Recurso_RecursoId",
                        column: x => x.RecursoId,
                        principalTable: "Recurso",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TipoLogro",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoLogro", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Logro",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Titulo = table.Column<string>(type: "text", nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: false),
                    TipoLogroId = table.Column<int>(type: "integer", nullable: false),
                    CondicionId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logro", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Logro_Condicion_CondicionId",
                        column: x => x.CondicionId,
                        principalTable: "Condicion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Logro_TipoLogro_TipoLogroId",
                        column: x => x.TipoLogroId,
                        principalTable: "TipoLogro",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LogroPartida",
                columns: table => new
                {
                    LogroId = table.Column<int>(type: "integer", nullable: false),
                    PartidaId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogroPartida", x => new { x.LogroId, x.PartidaId });
                    table.ForeignKey(
                        name: "FK_LogroPartida_Logro_LogroId",
                        column: x => x.LogroId,
                        principalTable: "Logro",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LogroPartida_Partida_PartidaId",
                        column: x => x.PartidaId,
                        principalTable: "Partida",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Condicion_EstructuraId",
                table: "Condicion",
                column: "EstructuraId");

            migrationBuilder.CreateIndex(
                name: "IX_Condicion_RecursoId",
                table: "Condicion",
                column: "RecursoId");

            migrationBuilder.CreateIndex(
                name: "IX_Logro_CondicionId",
                table: "Logro",
                column: "CondicionId");

            migrationBuilder.CreateIndex(
                name: "IX_Logro_TipoLogroId",
                table: "Logro",
                column: "TipoLogroId");

            migrationBuilder.CreateIndex(
                name: "IX_LogroPartida_PartidaId",
                table: "LogroPartida",
                column: "PartidaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogroPartida");

            migrationBuilder.DropTable(
                name: "Logro");

            migrationBuilder.DropTable(
                name: "Condicion");

            migrationBuilder.DropTable(
                name: "TipoLogro");
        }
    }
}
