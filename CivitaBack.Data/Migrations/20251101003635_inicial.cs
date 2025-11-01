using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CivitaBack.Data.Migrations
{
    /// <inheritdoc />
    public partial class inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventoMaestro",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    TextoDescripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    TextoAceptar = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    TextoRechazar = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    EcoCoinsAceptar = table.Column<int>(type: "integer", nullable: false),
                    FelicidadAceptar = table.Column<int>(type: "integer", nullable: false),
                    ContaminacionAceptar = table.Column<int>(type: "integer", nullable: false),
                    FelicidadRechazar = table.Column<int>(type: "integer", nullable: false),
                    ContaminacionRechazar = table.Column<int>(type: "integer", nullable: false),
                    Creado = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Editado = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventoMaestro", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoEstructura",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Ocupacion = table.Column<int>(type: "integer", nullable: false),
                    Capacidad = table.Column<int>(type: "integer", nullable: false),
                    EnergiaPorCiclo = table.Column<int>(type: "integer", nullable: false),
                    DineroPorCiclo = table.Column<int>(type: "integer", nullable: false),
                    Creado = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Editado = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoEstructura", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoLogro",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Creado = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Editado = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoLogro", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoTip",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Descripcion = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Creado = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Editado = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoTip", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NombreUsuario = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Mail = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    HashDeContrasena = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Creado = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Editado = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Estructura",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    EsMejorable = table.Column<bool>(type: "boolean", nullable: false),
                    RutaImagen = table.Column<string>(type: "text", nullable: true),
                    CostoEnergia = table.Column<int>(type: "integer", nullable: false),
                    CostoDinero = table.Column<int>(type: "integer", nullable: false),
                    FelicidadCiclo = table.Column<int>(type: "integer", nullable: false),
                    ContaminacionCiclo = table.Column<int>(type: "integer", nullable: false),
                    TipoEstructuraId = table.Column<int>(type: "integer", nullable: false),
                    Creado = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Editado = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estructura", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Estructura_TipoEstructura_TipoEstructuraId",
                        column: x => x.TipoEstructuraId,
                        principalTable: "TipoEstructura",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tip",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Mensaje = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Expresion = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ElementoAdicional = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    EfectoFiltro = table.Column<bool>(type: "boolean", nullable: false),
                    TipoId = table.Column<int>(type: "integer", nullable: false),
                    Creado = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Editado = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tip", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tip_TipoTip_TipoId",
                        column: x => x.TipoId,
                        principalTable: "TipoTip",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Partida",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UltimaVez = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    JsonMapa = table.Column<string>(type: "text", nullable: true),
                    UsuarioId = table.Column<int>(type: "integer", nullable: false),
                    Creado = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Editado = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partida", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Partida_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Condicion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Cantidad = table.Column<int>(type: "integer", nullable: false),
                    NombreColumna = table.Column<string>(type: "text", nullable: true),
                    EstructuraId = table.Column<int>(type: "integer", nullable: true),
                    EsRecompensa = table.Column<bool>(type: "boolean", nullable: false),
                    RecompensaId = table.Column<int>(type: "integer", nullable: true),
                    Creado = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Editado = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Condicion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Condicion_Condicion_RecompensaId",
                        column: x => x.RecompensaId,
                        principalTable: "Condicion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Condicion_Estructura_EstructuraId",
                        column: x => x.EstructuraId,
                        principalTable: "Estructura",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EstructuraMapa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PartidaId = table.Column<int>(type: "integer", nullable: false),
                    EstructuraId = table.Column<int>(type: "integer", nullable: false),
                    X = table.Column<int>(type: "integer", nullable: false),
                    Y = table.Column<int>(type: "integer", nullable: false),
                    Width = table.Column<int>(type: "integer", nullable: false),
                    Height = table.Column<int>(type: "integer", nullable: false),
                    Creado = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Editado = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstructuraMapa", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EstructuraMapa_Estructura_EstructuraId",
                        column: x => x.EstructuraId,
                        principalTable: "Estructura",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EstructuraMapa_Partida_PartidaId",
                        column: x => x.PartidaId,
                        principalTable: "Partida",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Evento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TextoDescripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    TextoAceptar = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    TextoRechazar = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    EcoCoinsAceptar = table.Column<int>(type: "integer", nullable: false),
                    FelicidadAceptar = table.Column<int>(type: "integer", nullable: false),
                    ContaminacionAceptar = table.Column<int>(type: "integer", nullable: false),
                    FelicidadRechazar = table.Column<int>(type: "integer", nullable: false),
                    ContaminacionRechazar = table.Column<int>(type: "integer", nullable: false),
                    SeDisparo = table.Column<bool>(type: "boolean", nullable: false),
                    Resuelto = table.Column<bool>(type: "boolean", nullable: false),
                    EventoMaestroId = table.Column<int>(type: "integer", nullable: false),
                    PartidaId = table.Column<int>(type: "integer", nullable: false),
                    Creado = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Editado = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Evento_EventoMaestro_EventoMaestroId",
                        column: x => x.EventoMaestroId,
                        principalTable: "EventoMaestro",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Evento_Partida_PartidaId",
                        column: x => x.PartidaId,
                        principalTable: "Partida",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Recurso",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Energia = table.Column<int>(type: "integer", nullable: false),
                    Contaminacion = table.Column<int>(type: "integer", nullable: false),
                    Felicidad = table.Column<int>(type: "integer", nullable: false),
                    EcoCoins = table.Column<int>(type: "integer", nullable: false),
                    Poblacion = table.Column<int>(type: "integer", nullable: false),
                    PartidaId = table.Column<int>(type: "integer", nullable: false),
                    Creado = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Editado = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recurso", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recurso_Partida_PartidaId",
                        column: x => x.PartidaId,
                        principalTable: "Partida",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tienda",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NombreArticulo = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CodigoArticulo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    PartidaId = table.Column<int>(type: "integer", nullable: false),
                    Creado = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Editado = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tienda", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tienda_Partida_PartidaId",
                        column: x => x.PartidaId,
                        principalTable: "Partida",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TipEnPartida",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PartidaId = table.Column<int>(type: "integer", nullable: false),
                    TipId = table.Column<int>(type: "integer", nullable: false),
                    Creado = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Editado = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipEnPartida", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TipEnPartida_Partida_PartidaId",
                        column: x => x.PartidaId,
                        principalTable: "Partida",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TipEnPartida_Tip_TipId",
                        column: x => x.TipId,
                        principalTable: "Tip",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Logro",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Titulo = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    TipoLogroId = table.Column<int>(type: "integer", nullable: false),
                    CondicionId = table.Column<int>(type: "integer", nullable: false),
                    Creado = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Editado = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logro", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Logro_Condicion_CondicionId",
                        column: x => x.CondicionId,
                        principalTable: "Condicion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Logro_TipoLogro_TipoLogroId",
                        column: x => x.TipoLogroId,
                        principalTable: "TipoLogro",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LogroPartida",
                columns: table => new
                {
                    LogroId = table.Column<int>(type: "integer", nullable: false),
                    PartidaId = table.Column<int>(type: "integer", nullable: false),
                    FechaCompletado = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Creado = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Editado = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
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
                name: "IX_Condicion_RecompensaId",
                table: "Condicion",
                column: "RecompensaId");

            migrationBuilder.CreateIndex(
                name: "IX_Estructura_TipoEstructuraId",
                table: "Estructura",
                column: "TipoEstructuraId");

            migrationBuilder.CreateIndex(
                name: "IX_EstructuraMapa_EstructuraId",
                table: "EstructuraMapa",
                column: "EstructuraId");

            migrationBuilder.CreateIndex(
                name: "IX_EstructuraMapa_PartidaId",
                table: "EstructuraMapa",
                column: "PartidaId");

            migrationBuilder.CreateIndex(
                name: "IX_Evento_EventoMaestroId",
                table: "Evento",
                column: "EventoMaestroId");

            migrationBuilder.CreateIndex(
                name: "IX_Evento_PartidaId",
                table: "Evento",
                column: "PartidaId");

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

            migrationBuilder.CreateIndex(
                name: "IX_Partida_UsuarioId",
                table: "Partida",
                column: "UsuarioId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Recurso_PartidaId",
                table: "Recurso",
                column: "PartidaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tienda_PartidaId",
                table: "Tienda",
                column: "PartidaId");

            migrationBuilder.CreateIndex(
                name: "IX_Tip_TipoId",
                table: "Tip",
                column: "TipoId");

            migrationBuilder.CreateIndex(
                name: "IX_TipEnPartida_PartidaId",
                table: "TipEnPartida",
                column: "PartidaId");

            migrationBuilder.CreateIndex(
                name: "IX_TipEnPartida_TipId",
                table: "TipEnPartida",
                column: "TipId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EstructuraMapa");

            migrationBuilder.DropTable(
                name: "Evento");

            migrationBuilder.DropTable(
                name: "LogroPartida");

            migrationBuilder.DropTable(
                name: "Recurso");

            migrationBuilder.DropTable(
                name: "Tienda");

            migrationBuilder.DropTable(
                name: "TipEnPartida");

            migrationBuilder.DropTable(
                name: "EventoMaestro");

            migrationBuilder.DropTable(
                name: "Logro");

            migrationBuilder.DropTable(
                name: "Partida");

            migrationBuilder.DropTable(
                name: "Tip");

            migrationBuilder.DropTable(
                name: "Condicion");

            migrationBuilder.DropTable(
                name: "TipoLogro");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "TipoTip");

            migrationBuilder.DropTable(
                name: "Estructura");

            migrationBuilder.DropTable(
                name: "TipoEstructura");
        }
    }
}
