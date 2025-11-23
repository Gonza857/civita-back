using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CivitaBack.Data.Migrations
{
    /// <inheritdoc />
    public partial class refactor_eventos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Evento_EventoMaestro_EventoMaestroId",
                table: "Evento");

            migrationBuilder.DropColumn(
                name: "ContaminacionAceptar",
                table: "EventoMaestro");

            migrationBuilder.DropColumn(
                name: "ContaminacionRechazar",
                table: "EventoMaestro");

            migrationBuilder.DropColumn(
                name: "EcoCoinsAceptar",
                table: "EventoMaestro");

            migrationBuilder.DropColumn(
                name: "FelicidadAceptar",
                table: "EventoMaestro");

            migrationBuilder.DropColumn(
                name: "Nombre",
                table: "EventoMaestro");

            migrationBuilder.DropColumn(
                name: "TextoAceptar",
                table: "EventoMaestro");

            migrationBuilder.DropColumn(
                name: "TextoRechazar",
                table: "EventoMaestro");

            migrationBuilder.DropColumn(
                name: "TextoAceptar",
                table: "Evento");

            migrationBuilder.DropColumn(
                name: "TextoRechazar",
                table: "Evento");

            migrationBuilder.RenameColumn(
                name: "TextoDescripcion",
                table: "EventoMaestro",
                newName: "ContenidoPrincipal");

            migrationBuilder.RenameColumn(
                name: "FelicidadRechazar",
                table: "EventoMaestro",
                newName: "TipoEvento");

            migrationBuilder.RenameColumn(
                name: "TextoDescripcion",
                table: "Evento",
                newName: "Contenido");

            migrationBuilder.RenameColumn(
                name: "FelicidadRechazar",
                table: "Evento",
                newName: "FelicidadAplicada");

            migrationBuilder.RenameColumn(
                name: "FelicidadAceptar",
                table: "Evento",
                newName: "ExperienciaAplicada");

            migrationBuilder.RenameColumn(
                name: "EcoCoinsAceptar",
                table: "Evento",
                newName: "EnergiaAplicada");

            migrationBuilder.RenameColumn(
                name: "ContaminacionRechazar",
                table: "Evento",
                newName: "EcoCoinsAplicada");

            migrationBuilder.RenameColumn(
                name: "ContaminacionAceptar",
                table: "Evento",
                newName: "ContaminacionAplicada");

            migrationBuilder.AddColumn<string>(
                name: "OpcionA_Texto",
                table: "EventoMaestro",
                type: "character varying(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OpcionB_Texto",
                table: "EventoMaestro",
                type: "character varying(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RespuestaCorrecta",
                table: "EventoMaestro",
                type: "character varying(1)",
                maxLength: 1,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Titulo",
                table: "EventoMaestro",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RespuestaJugador",
                table: "Evento",
                type: "character varying(1)",
                maxLength: 1,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EfectoEvento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EventoMaestroId = table.Column<int>(type: "integer", nullable: false),
                    TipoResultado = table.Column<int>(type: "integer", nullable: false),
                    EcoCoins = table.Column<int>(type: "integer", nullable: false),
                    Felicidad = table.Column<int>(type: "integer", nullable: false),
                    Contaminacion = table.Column<int>(type: "integer", nullable: false),
                    Energia = table.Column<int>(type: "integer", nullable: false),
                    Experiencia = table.Column<int>(type: "integer", nullable: false),
                    Creado = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Editado = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EfectoEvento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EfectoEvento_EventoMaestro_EventoMaestroId",
                        column: x => x.EventoMaestroId,
                        principalTable: "EventoMaestro",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "EfectoEvento",
                columns: new[] { "Id", "Contaminacion", "Creado", "EcoCoins", "Editado", "Energia", "EventoMaestroId", "Experiencia", "Felicidad", "TipoResultado" },
                values: new object[,]
                {
                    { 1, -4, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), 50, null, 0, 1, 50, 3, 1 },
                    { 2, 3, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), -20, null, 0, 1, 0, -2, 2 }
                });

            migrationBuilder.UpdateData(
                table: "EventoMaestro",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ContenidoPrincipal", "OpcionA_Texto", "OpcionB_Texto", "RespuestaCorrecta", "TipoEvento", "Titulo" },
                values: new object[] { "Si tenés que recorrer 5 km en la ciudad, ¿qué opción genera la MENOR cantidad de emisiones de CO₂ (Dióxido de carbono) por persona?", "A) Ir en colectivo con 30 personas más.", "B) Usar un auto moderno, solo con el conductor.", "A", 1, "¿Cómo me muevo hoy?" });

            migrationBuilder.CreateIndex(
                name: "IX_EfectoEvento_EventoMaestroId",
                table: "EfectoEvento",
                column: "EventoMaestroId");

            migrationBuilder.AddForeignKey(
                name: "FK_Evento_EventoMaestro_EventoMaestroId",
                table: "Evento",
                column: "EventoMaestroId",
                principalTable: "EventoMaestro",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Evento_EventoMaestro_EventoMaestroId",
                table: "Evento");

            migrationBuilder.DropTable(
                name: "EfectoEvento");

            migrationBuilder.DropColumn(
                name: "OpcionA_Texto",
                table: "EventoMaestro");

            migrationBuilder.DropColumn(
                name: "OpcionB_Texto",
                table: "EventoMaestro");

            migrationBuilder.DropColumn(
                name: "RespuestaCorrecta",
                table: "EventoMaestro");

            migrationBuilder.DropColumn(
                name: "Titulo",
                table: "EventoMaestro");

            migrationBuilder.DropColumn(
                name: "RespuestaJugador",
                table: "Evento");

            migrationBuilder.RenameColumn(
                name: "TipoEvento",
                table: "EventoMaestro",
                newName: "FelicidadRechazar");

            migrationBuilder.RenameColumn(
                name: "ContenidoPrincipal",
                table: "EventoMaestro",
                newName: "TextoDescripcion");

            migrationBuilder.RenameColumn(
                name: "FelicidadAplicada",
                table: "Evento",
                newName: "FelicidadRechazar");

            migrationBuilder.RenameColumn(
                name: "ExperienciaAplicada",
                table: "Evento",
                newName: "FelicidadAceptar");

            migrationBuilder.RenameColumn(
                name: "EnergiaAplicada",
                table: "Evento",
                newName: "EcoCoinsAceptar");

            migrationBuilder.RenameColumn(
                name: "EcoCoinsAplicada",
                table: "Evento",
                newName: "ContaminacionRechazar");

            migrationBuilder.RenameColumn(
                name: "Contenido",
                table: "Evento",
                newName: "TextoDescripcion");

            migrationBuilder.RenameColumn(
                name: "ContaminacionAplicada",
                table: "Evento",
                newName: "ContaminacionAceptar");

            migrationBuilder.AddColumn<int>(
                name: "ContaminacionAceptar",
                table: "EventoMaestro",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ContaminacionRechazar",
                table: "EventoMaestro",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EcoCoinsAceptar",
                table: "EventoMaestro",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FelicidadAceptar",
                table: "EventoMaestro",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Nombre",
                table: "EventoMaestro",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TextoAceptar",
                table: "EventoMaestro",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TextoRechazar",
                table: "EventoMaestro",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TextoAceptar",
                table: "Evento",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TextoRechazar",
                table: "Evento",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "EventoMaestro",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ContaminacionAceptar", "ContaminacionRechazar", "EcoCoinsAceptar", "FelicidadAceptar", "FelicidadRechazar", "Nombre", "TextoAceptar", "TextoDescripcion", "TextoRechazar" },
                values: new object[] { -10, 20, -100, 10, -10, "Separación de residuos", "En Argentina, solo el 3% de los residuos se reciclan. Separar la basura reduce rellenos sanitarios y emisiones de metano.", "Los vecinos solicitan un sistema de reciclaje en la ciudad debido a la alta contaminación.", "Cuando no se recicla, los rellenos sanitarios crecen y emiten metano, un gas 28 veces peor que el CO₂ para el clima." });

            migrationBuilder.AddForeignKey(
                name: "FK_Evento_EventoMaestro_EventoMaestroId",
                table: "Evento",
                column: "EventoMaestroId",
                principalTable: "EventoMaestro",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
