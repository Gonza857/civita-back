using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CivitaBack.Data.Migrations
{
    /// <inheritdoc />
    public partial class auditable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Creado",
                table: "TipoLogro",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Editado",
                table: "TipoLogro",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Creado",
                table: "TipoEstructura",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Editado",
                table: "TipoEstructura",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Creado",
                table: "TipEnPartida",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Editado",
                table: "TipEnPartida",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Creado",
                table: "Tip",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Editado",
                table: "Tip",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Creado",
                table: "Tienda",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Editado",
                table: "Tienda",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Creado",
                table: "Recurso",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Editado",
                table: "Recurso",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Creado",
                table: "Partida",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Editado",
                table: "Partida",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Creado",
                table: "LogroPartida",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Editado",
                table: "LogroPartida",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Creado",
                table: "Logro",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Editado",
                table: "Logro",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Creado",
                table: "EventoMaestro",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Editado",
                table: "EventoMaestro",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Creado",
                table: "Evento",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Editado",
                table: "Evento",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Creado",
                table: "EstructuraMapa",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Editado",
                table: "EstructuraMapa",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Creado",
                table: "Estructura",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Editado",
                table: "Estructura",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Creado",
                table: "Condicion",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Editado",
                table: "Condicion",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Creado",
                table: "TipoLogro");

            migrationBuilder.DropColumn(
                name: "Editado",
                table: "TipoLogro");

            migrationBuilder.DropColumn(
                name: "Creado",
                table: "TipoEstructura");

            migrationBuilder.DropColumn(
                name: "Editado",
                table: "TipoEstructura");

            migrationBuilder.DropColumn(
                name: "Creado",
                table: "TipEnPartida");

            migrationBuilder.DropColumn(
                name: "Editado",
                table: "TipEnPartida");

            migrationBuilder.DropColumn(
                name: "Creado",
                table: "Tip");

            migrationBuilder.DropColumn(
                name: "Editado",
                table: "Tip");

            migrationBuilder.DropColumn(
                name: "Creado",
                table: "Tienda");

            migrationBuilder.DropColumn(
                name: "Editado",
                table: "Tienda");

            migrationBuilder.DropColumn(
                name: "Creado",
                table: "Recurso");

            migrationBuilder.DropColumn(
                name: "Editado",
                table: "Recurso");

            migrationBuilder.DropColumn(
                name: "Creado",
                table: "Partida");

            migrationBuilder.DropColumn(
                name: "Editado",
                table: "Partida");

            migrationBuilder.DropColumn(
                name: "Creado",
                table: "LogroPartida");

            migrationBuilder.DropColumn(
                name: "Editado",
                table: "LogroPartida");

            migrationBuilder.DropColumn(
                name: "Creado",
                table: "Logro");

            migrationBuilder.DropColumn(
                name: "Editado",
                table: "Logro");

            migrationBuilder.DropColumn(
                name: "Creado",
                table: "EventoMaestro");

            migrationBuilder.DropColumn(
                name: "Editado",
                table: "EventoMaestro");

            migrationBuilder.DropColumn(
                name: "Creado",
                table: "Evento");

            migrationBuilder.DropColumn(
                name: "Editado",
                table: "Evento");

            migrationBuilder.DropColumn(
                name: "Creado",
                table: "EstructuraMapa");

            migrationBuilder.DropColumn(
                name: "Editado",
                table: "EstructuraMapa");

            migrationBuilder.DropColumn(
                name: "Creado",
                table: "Estructura");

            migrationBuilder.DropColumn(
                name: "Editado",
                table: "Estructura");

            migrationBuilder.DropColumn(
                name: "Creado",
                table: "Condicion");

            migrationBuilder.DropColumn(
                name: "Editado",
                table: "Condicion");
        }
    }
}
