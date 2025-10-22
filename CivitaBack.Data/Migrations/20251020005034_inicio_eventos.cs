using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CivitaBack.Data.Migrations
{
    /// <inheritdoc />
    public partial class inicio_eventos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescripcionEvento",
                table: "Evento");

            migrationBuilder.RenameColumn(
                name: "Descripcion",
                table: "EventoMaestro",
                newName: "Nombre");

            migrationBuilder.RenameColumn(
                name: "TiempoParaHacerlo",
                table: "Evento",
                newName: "FelicidadRechazar");

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

            migrationBuilder.AddColumn<int>(
                name: "FelicidadRechazar",
                table: "EventoMaestro",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TextoAceptar",
                table: "EventoMaestro",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TextoDescripcion",
                table: "EventoMaestro",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TextoRechazar",
                table: "EventoMaestro",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ContaminacionAceptar",
                table: "Evento",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ContaminacionRechazar",
                table: "Evento",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EcoCoinsAceptar",
                table: "Evento",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FelicidadAceptar",
                table: "Evento",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Resuelto",
                table: "Evento",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SeDisparo",
                table: "Evento",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TextoAceptar",
                table: "Evento",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TextoDescripcion",
                table: "Evento",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TextoRechazar",
                table: "Evento",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "FelicidadRechazar",
                table: "EventoMaestro");

            migrationBuilder.DropColumn(
                name: "TextoAceptar",
                table: "EventoMaestro");

            migrationBuilder.DropColumn(
                name: "TextoDescripcion",
                table: "EventoMaestro");

            migrationBuilder.DropColumn(
                name: "TextoRechazar",
                table: "EventoMaestro");

            migrationBuilder.DropColumn(
                name: "ContaminacionAceptar",
                table: "Evento");

            migrationBuilder.DropColumn(
                name: "ContaminacionRechazar",
                table: "Evento");

            migrationBuilder.DropColumn(
                name: "EcoCoinsAceptar",
                table: "Evento");

            migrationBuilder.DropColumn(
                name: "FelicidadAceptar",
                table: "Evento");

            migrationBuilder.DropColumn(
                name: "Resuelto",
                table: "Evento");

            migrationBuilder.DropColumn(
                name: "SeDisparo",
                table: "Evento");

            migrationBuilder.DropColumn(
                name: "TextoAceptar",
                table: "Evento");

            migrationBuilder.DropColumn(
                name: "TextoDescripcion",
                table: "Evento");

            migrationBuilder.DropColumn(
                name: "TextoRechazar",
                table: "Evento");

            migrationBuilder.RenameColumn(
                name: "Nombre",
                table: "EventoMaestro",
                newName: "Descripcion");

            migrationBuilder.RenameColumn(
                name: "FelicidadRechazar",
                table: "Evento",
                newName: "TiempoParaHacerlo");

            migrationBuilder.AddColumn<string>(
                name: "DescripcionEvento",
                table: "Evento",
                type: "text",
                nullable: true);
        }
    }
}
