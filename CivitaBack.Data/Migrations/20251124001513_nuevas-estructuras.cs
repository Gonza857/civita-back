using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CivitaBack.Data.Migrations
{
    /// <inheritdoc />
    public partial class nuevasestructuras : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 1,
                column: "ContaminacionCiclo",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ContaminacionCiclo", "CostoDinero", "FelicidadCiclo" },
                values: new object[] { -3, 300, 0 });

            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 3,
                column: "ContaminacionCiclo",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CostoDinero", "FelicidadCiclo", "Nombre", "RutaImagen" },
                values: new object[] { 20, 2, "Parque", "Estructura-Parque-Pequeño.png" });

            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ContaminacionCiclo", "CostoDinero", "FelicidadCiclo", "Nombre", "RutaImagen" },
                values: new object[] { 3, 150, 5, "Hotel", "Estructura-Hotel.png" });

            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ContaminacionCiclo", "CostoDinero", "FelicidadCiclo", "Nombre", "RutaImagen" },
                values: new object[] { -5, 400, 3, "Planta Neocorp", "Estructura-Fabrica-Nivel-2.png" });

            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "ContaminacionCiclo", "CostoDinero", "Nombre", "RutaImagen" },
                values: new object[] { -1, 80, "Panel solar", "Estructura-Panel-Solar.png" });

            migrationBuilder.UpdateData(
                table: "TipoEstructura",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Capacidad", "DineroPorCiclo", "EnergiaPorCiclo" },
                values: new object[] { 5, 1, -1 });

            migrationBuilder.UpdateData(
                table: "TipoEstructura",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DineroPorCiclo", "EnergiaPorCiclo" },
                values: new object[] { -12, 5 });

            migrationBuilder.UpdateData(
                table: "TipoEstructura",
                keyColumn: "Id",
                keyValue: 3,
                column: "DineroPorCiclo",
                value: 30);

            migrationBuilder.UpdateData(
                table: "TipoEstructura",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "DineroPorCiclo", "EnergiaPorCiclo", "Nombre" },
                values: new object[] { -3, 1, "Verde" });

            migrationBuilder.UpdateData(
                table: "TipoEstructura",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Capacidad", "DineroPorCiclo", "EnergiaPorCiclo", "Nombre" },
                values: new object[] { 50, 10, -2, "Alojamiento" });

            migrationBuilder.UpdateData(
                table: "TipoEstructura",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "DineroPorCiclo", "Nombre" },
                values: new object[] { -10, "Reciclaje" });

            migrationBuilder.UpdateData(
                table: "TipoEstructura",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "DineroPorCiclo", "EnergiaPorCiclo", "Nombre" },
                values: new object[] { -5, 2, "Energia solar" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 1,
                column: "ContaminacionCiclo",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ContaminacionCiclo", "CostoDinero", "FelicidadCiclo" },
                values: new object[] { -4, 50, 2 });

            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 3,
                column: "ContaminacionCiclo",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CostoDinero", "FelicidadCiclo", "Nombre", "RutaImagen" },
                values: new object[] { 0, 0, "Iniciales", null });

            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ContaminacionCiclo", "CostoDinero", "FelicidadCiclo", "Nombre", "RutaImagen" },
                values: new object[] { 0, 0, 0, "InicialesCasa2", null });

            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ContaminacionCiclo", "CostoDinero", "FelicidadCiclo", "Nombre", "RutaImagen" },
                values: new object[] { 0, 0, 0, "InicialesEdificio", null });

            migrationBuilder.UpdateData(
                table: "Estructura",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "ContaminacionCiclo", "CostoDinero", "Nombre", "RutaImagen" },
                values: new object[] { 0, 0, "InicialesBase", null });

            migrationBuilder.UpdateData(
                table: "TipoEstructura",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Capacidad", "DineroPorCiclo", "EnergiaPorCiclo" },
                values: new object[] { 0, -3, -2 });

            migrationBuilder.UpdateData(
                table: "TipoEstructura",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DineroPorCiclo", "EnergiaPorCiclo" },
                values: new object[] { -7, 10 });

            migrationBuilder.UpdateData(
                table: "TipoEstructura",
                keyColumn: "Id",
                keyValue: 3,
                column: "DineroPorCiclo",
                value: 50);

            migrationBuilder.UpdateData(
                table: "TipoEstructura",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "DineroPorCiclo", "EnergiaPorCiclo", "Nombre" },
                values: new object[] { 0, 0, "Inicial" });

            migrationBuilder.UpdateData(
                table: "TipoEstructura",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Capacidad", "DineroPorCiclo", "EnergiaPorCiclo", "Nombre" },
                values: new object[] { 0, 0, 0, "InicialCasa2" });

            migrationBuilder.UpdateData(
                table: "TipoEstructura",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "DineroPorCiclo", "Nombre" },
                values: new object[] { 0, "InicialEdificio" });

            migrationBuilder.UpdateData(
                table: "TipoEstructura",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "DineroPorCiclo", "EnergiaPorCiclo", "Nombre" },
                values: new object[] { 0, 0, "InicialBase" });
        }
    }
}
