using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CivitaBack.Data.Migrations
{
    /// <inheritdoc />
    public partial class refactor2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Tip",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Tip",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Tip",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Tip",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Tip",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Tip",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Tip",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Tip",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Tip",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Tip",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Tip",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Tip",
                keyColumn: "Id",
                keyValue: 12);
        }
    }
}
