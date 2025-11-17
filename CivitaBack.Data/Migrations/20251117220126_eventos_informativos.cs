using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CivitaBack.Data.Migrations
{
    /// <inheritdoc />
    public partial class eventos_informativos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "EventoMaestro",
                columns: new[] { "Id", "ContenidoPrincipal", "Creado", "Editado", "OpcionA_Texto", "OpcionB_Texto", "RespuestaCorrecta", "TipoEvento", "Titulo" },
                values: new object[] { 2, "Tu nivel de contaminación es críticamente alto. Si excede el 80%, la Felicidad de tus ciudadanos caerá rápidamente. Intentá construir más plantas de tratamiento de aire y espacios verdes.", new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Entendido.", "", "A", 2, "¡Alerta Roja de Contaminación!" });

            migrationBuilder.InsertData(
                table: "EfectoEvento",
                columns: new[] { "Id", "Contaminacion", "Creado", "EcoCoins", "Editado", "Energia", "EventoMaestroId", "Experiencia", "Felicidad", "TipoResultado" },
                values: new object[] { 3, 0, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0, null, 0, 2, 0, 0, 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "EfectoEvento",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "EventoMaestro",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
