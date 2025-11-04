using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CivitaBack.Data.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
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
                    EstructuraId = table.Column<int>(type: "integer", nullable: false),
                    PartidaId = table.Column<int>(type: "integer", nullable: false),
                    Creado = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Editado = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tienda", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tienda_Estructura_EstructuraId",
                        column: x => x.EstructuraId,
                        principalTable: "Estructura",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "Mision",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Titulo = table.Column<string>(type: "text", nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: false),
                    Disponible = table.Column<bool>(type: "boolean", nullable: false),
                    CondicionId = table.Column<int>(type: "integer", nullable: false),
                    Tipo = table.Column<string>(type: "text", nullable: false),
                    Creado = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Editado = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mision", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mision_Condicion_CondicionId",
                        column: x => x.CondicionId,
                        principalTable: "Condicion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateTable(
                name: "MisionPartida",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FechaCompletado = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaEntrega = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Reclamado = table.Column<bool>(type: "boolean", nullable: false),
                    MisionId = table.Column<int>(type: "integer", nullable: false),
                    PartidaId = table.Column<int>(type: "integer", nullable: false),
                    Creado = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Editado = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MisionPartida", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MisionPartida_Mision_MisionId",
                        column: x => x.MisionId,
                        principalTable: "Mision",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MisionPartida_Partida_PartidaId",
                        column: x => x.PartidaId,
                        principalTable: "Partida",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "TipoEstructura",
                columns: new[] { "Id", "Capacidad", "Creado", "DineroPorCiclo", "Editado", "EnergiaPorCiclo", "Nombre", "Ocupacion" },
                values: new object[,]
                {
                    { 1, 0, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), -3, null, -2, "Vivienda", 0 },
                    { 2, 0, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), -7, null, 10, "Energia", 0 },
                    { 3, 0, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), 50, null, -4, "Industrial", 0 },
                    { 4, 0, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0, null, 0, "Inicial", 0 },
                    { 5, 0, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0, null, 0, "InicialCasa2", 0 },
                    { 6, 0, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0, null, 0, "InicialEdificio", 0 },
                    { 7, 0, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0, null, 0, "InicialBase", 0 }
                });

            migrationBuilder.InsertData(
                table: "TipoTip",
                columns: new[] { "Id", "Creado", "Descripcion", "Editado" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "info", null },
                    { 2, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "onboarding", null }
                });

            migrationBuilder.InsertData(
                table: "Estructura",
                columns: new[] { "Id", "ContaminacionCiclo", "CostoDinero", "CostoEnergia", "Creado", "Editado", "EsMejorable", "FelicidadCiclo", "Nombre", "RutaImagen", "TipoEstructuraId" },
                values: new object[,]
                {
                    { 1, 2, 25, 0, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, 2, "Casa", "/assets/mapa/casas.png", 1 },
                    { 2, -4, 50, 0, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, 2, "Turbina Eólica", "/assets/mapa/turbina.png", 2 },
                    { 3, 6, 40, 0, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, -4, "Fabrica", "/assets/mapa/fabrica.png", 3 },
                    { 4, 0, 0, 0, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, 0, "Iniciales", null, 4 },
                    { 5, 0, 0, 0, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, 0, "InicialesCasa2", null, 5 },
                    { 6, 0, 0, 0, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, 0, "InicialesEdificio", null, 6 },
                    { 7, 0, 0, 0, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, 0, "InicialesBase", null, 7 }
                });

            migrationBuilder.InsertData(
                table: "Tip",
                columns: new[] { "Id", "Creado", "Editado", "EfectoFiltro", "ElementoAdicional", "Expresion", "Mensaje", "TipoId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "", "vitaSaluda", "¡Hola!, soy Vita ", 1 },
                    { 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "", "PulgarArribaVita", "Te doy la bienvenida a Neocivita ", 1 },
                    { 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "", "vitaPregunta", "Antes de empezar, ¿cómo te llamás?", 1 },
                    { 4, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "", "vitaExplica2", "En Neocivita aprenderás sobre el impacto ecológico que tienen nuestras decisiones al construir y mantener una ciudad.", 1 },
                    { 5, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "", "vitaDecidida", "¿Creés que podés lograr el equilibrio entre economía, sociedad y ambiente?", 1 },
                    { 6, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, "ecoCoinBrillante", "vitaExplica2", "Estas son las EcoCoins, la moneda para adquirir construcciones en tu ciudad.", 1 },
                    { 7, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "ecoCoin", "vitaExplica", "Podés conseguirlas con edificaciones industriales o completando misiones. ¡Acompañame a ver las demás!", 1 },
                    { 8, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "energiaCoin", "vitaCansada", "Te presento la energía eléctrica: podés obtenerla con construcciones específicas que la generen, como un panel solar.", 1 },
                    { 9, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "contaminacionCoin", "vitaPensativa", "Ahora… la CONTAMINACIÓN. Este recurso destruye tu ciudad y el planeta. ¡Tené cuidado!", 1 },
                    { 10, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "felicidadCoin", "vitaDecidida", "La FELICIDAD refleja qué tan saludable y feliz está tu población. ¡Es muy importante!", 1 },
                    { 11, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "", "vitaFesteja", "Ahora que conocés los recursos del juego… ¡acompañame a jugar y empecemos a construir!", 1 },
                    { 12, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "", "vitaFesteja", "¡Registrate para empezar a construir nuestra ciudad!", 1 }
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
                name: "IX_Mision_CondicionId",
                table: "Mision",
                column: "CondicionId");

            migrationBuilder.CreateIndex(
                name: "IX_MisionPartida_MisionId",
                table: "MisionPartida",
                column: "MisionId");

            migrationBuilder.CreateIndex(
                name: "IX_MisionPartida_PartidaId",
                table: "MisionPartida",
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
                name: "IX_Tienda_EstructuraId",
                table: "Tienda",
                column: "EstructuraId");

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
                name: "MisionPartida");

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
                name: "Mision");

            migrationBuilder.DropTable(
                name: "Partida");

            migrationBuilder.DropTable(
                name: "Tip");

            migrationBuilder.DropTable(
                name: "TipoLogro");

            migrationBuilder.DropTable(
                name: "Condicion");

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
