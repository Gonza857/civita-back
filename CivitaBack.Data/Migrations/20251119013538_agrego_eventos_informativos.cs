using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CivitaBack.Data.Migrations
{
    /// <inheritdoc />
    public partial class agrego_eventos_informativos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "EventoMaestro",
                keyColumn: "Id",
                keyValue: 2,
                column: "ContenidoPrincipal",
                value: "Tu nivel de contaminación es críticamente alto. Si excede el 80%, la Felicidad de tus ciudadanos caerá rápidamente. Intentá construir más estructuras que limpien el aire y sumar espacios verdes.");

            migrationBuilder.InsertData(
                table: "EventoMaestro",
                columns: new[] { "Id", "ContenidoPrincipal", "Creado", "Editado", "OpcionA_Texto", "OpcionB_Texto", "RespuestaCorrecta", "TipoEvento", "Titulo" },
                values: new object[,]
                {
                    { 3, "Tus reservas de energía están peligrosamente bajas. Si caen por debajo del 10%, varias estructuras dejarán de funcionar, afectando la Felicidad y el crecimiento de tu ciudad. Considerá construir más fuentes de energía renovable.", new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Entendido.", "", "A", 2, "¡Alerta de Energía Crítica!" },
                    { 4, "La Felicidad de tus ciudadanos está peligrosamente baja. Cuando esto ocurre, la población puede comenzar a disminuir y tu ciudad se vuelve menos estable. Revisá si hay suficiente refugio disponible y tratá de reducir los niveles de contaminación para mejorar el ánimo general de la población.", new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Entendido.", "", "A", 2, "¡Felicidad en Nivel Crítico!" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "EventoMaestro",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "EventoMaestro",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.UpdateData(
                table: "EventoMaestro",
                keyColumn: "Id",
                keyValue: 2,
                column: "ContenidoPrincipal",
                value: "Tu nivel de contaminación es críticamente alto. Si excede el 80%, la Felicidad de tus ciudadanos caerá rápidamente. Intentá construir más plantas de tratamiento de aire y espacios verdes.");
        }
    }
}
