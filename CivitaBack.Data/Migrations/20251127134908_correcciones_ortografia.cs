using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CivitaBack.Data.Migrations
{
    /// <inheritdoc />
    public partial class correcciones_ortografia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "EventoMaestro",
                keyColumn: "Id",
                keyValue: 1,
                column: "Titulo",
                value: "¿Cómo viajo hoy?");

            migrationBuilder.UpdateData(
                table: "EventoMaestro",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ContenidoPrincipal", "OpcionA_Texto" },
                values: new object[] { "Tu nivel de Contaminación es críticamente alto. Intentá construir más estructuras que limpien el aire y sumar espacios verdes.", "Entendido" });

            migrationBuilder.UpdateData(
                table: "EventoMaestro",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ContenidoPrincipal", "OpcionA_Texto" },
                values: new object[] { "Tus reservas de Energía están peligrosamente bajas. Considerá construir más fuentes de energía renovable.", "Entendido" });

            migrationBuilder.UpdateData(
                table: "EventoMaestro",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ContenidoPrincipal", "OpcionA_Texto" },
                values: new object[] { "La Felicidad de tus ciudadanos está peligrosamente baja. Revisá si hay suficiente refugio disponible y tratá de reducir los niveles de contaminación para mejorar el ánimo de la población.", "Entendido" });

            migrationBuilder.UpdateData(
                table: "Mision",
                keyColumn: "Id",
                keyValue: 2,
                column: "Titulo",
                value: "¡Construí 1 fábrica!");

            migrationBuilder.UpdateData(
                table: "Mision",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Descripcion", "Titulo" },
                values: new object[] { "Nuestros ciudadanos necesitan lugar para vivir.", "¡Construí 1 casa!" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "EventoMaestro",
                keyColumn: "Id",
                keyValue: 1,
                column: "Titulo",
                value: "¿Cómo me muevo hoy?");

            migrationBuilder.UpdateData(
                table: "EventoMaestro",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ContenidoPrincipal", "OpcionA_Texto" },
                values: new object[] { "Tu nivel de contaminación es críticamente alto. Si excede el 80%, la Felicidad de tus ciudadanos caerá rápidamente. Intentá construir más estructuras que limpien el aire y sumar espacios verdes.", "Entendido." });

            migrationBuilder.UpdateData(
                table: "EventoMaestro",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ContenidoPrincipal", "OpcionA_Texto" },
                values: new object[] { "Tus reservas de energía están peligrosamente bajas. Si caen por debajo del 10%, varias estructuras dejarán de funcionar, afectando la Felicidad y el crecimiento de tu ciudad. Considerá construir más fuentes de energía renovable.", "Entendido." });

            migrationBuilder.UpdateData(
                table: "EventoMaestro",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ContenidoPrincipal", "OpcionA_Texto" },
                values: new object[] { "La Felicidad de tus ciudadanos está peligrosamente baja. Cuando esto ocurre, la población puede comenzar a disminuir y tu ciudad se vuelve menos estable. Revisá si hay suficiente refugio disponible y tratá de reducir los niveles de contaminación para mejorar el ánimo general de la población.", "Entendido." });

            migrationBuilder.UpdateData(
                table: "Mision",
                keyColumn: "Id",
                keyValue: 2,
                column: "Titulo",
                value: "¡Construye 1 fabrica!");

            migrationBuilder.UpdateData(
                table: "Mision",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Descripcion", "Titulo" },
                values: new object[] { "Nuestros ciudadanos necesitan lugar para vivir. Construí 1 casa.", "¡Construye 1 casa!" });
        }
    }
}
