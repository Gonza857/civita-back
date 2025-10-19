using CivitaBack.Data.BO;
using Xunit;

namespace CivitaBack.Tests
{
    public class TipoEstructuraTest
    {
        [Fact]
        public void TipoEstructura_Constructor_InitializesProperties()
        {
            // Act
            var tipoEstructura = new TipoEstructura();

            // Assert
            Assert.Equal(0, tipoEstructura.Id);
            Assert.Null(tipoEstructura.Nombre);
            Assert.Equal(0, tipoEstructura.Ocupacion);
            Assert.Equal(0, tipoEstructura.Capacidad);
            Assert.Equal(0, tipoEstructura.EnergiaPorCiclo);
            Assert.Equal(0, tipoEstructura.DineroPorCiclo);
            Assert.Null(tipoEstructura.Estructura);
        }

        // [Fact]
        // public void TipoEstructura_SetProperties_ValuesAreSet()
        // {
        //     // Arrange
        //     var tipoEstructura = new TipoEstructura();
        //
        //     // Act
        //     tipoEstructura.Id = 1;
        //     tipoEstructura.Nombre = "Generadora";
        //     tipoEstructura.Ocupacion = "Industrial";
        //     tipoEstructura.Capacidad = 100;
        //     tipoEstructura.EnergiaPorCiclo = 50;
        //     tipoEstructura.DineroPorCiclo = 25;
        //
        //     // Assert
        //     Assert.Equal(1, tipoEstructura.Id);
        //     Assert.Equal("Generadora", tipoEstructura.Nombre);
        //     Assert.Equal("Industrial", tipoEstructura.Ocupacion);
        //     Assert.Equal(100, tipoEstructura.Capacidad);
        //     Assert.Equal(50, tipoEstructura.EnergiaPorCiclo);
        //     Assert.Equal(25, tipoEstructura.DineroPorCiclo);
        // }

        // [Fact]
        // public void TipoEstructura_WithNullValues_PropertiesCanBeNull()
        // {
        //     // Arrange
        //     var tipoEstructura = new TipoEstructura
        //     {
        //         Id = 1,
        //         Nombre = null,
        //         Ocupacion = null,
        //         Capacidad = 0,
        //         EnergiaPorCiclo = 0,
        //         DineroPorCiclo = 0,
        //         Estructura = null
        //     };
        //
        //     // Assert
        //     Assert.Equal(1, tipoEstructura.Id);
        //     Assert.Null(tipoEstructura.Nombre);
        //     Assert.Null(tipoEstructura.Ocupacion);
        //     Assert.Equal(0, tipoEstructura.Capacidad);
        //     Assert.Equal(0, tipoEstructura.EnergiaPorCiclo);
        //     Assert.Equal(0, tipoEstructura.DineroPorCiclo);
        //     Assert.Null(tipoEstructura.Estructura);
        // }

        [Fact]
        public void TipoEstructura_WithNegativeValues_CanHandleNegativeNumbers()
        {
            // Arrange
            var tipoEstructura = new TipoEstructura();

            // Act
            tipoEstructura.Capacidad = -10;
            tipoEstructura.EnergiaPorCiclo = -5;
            tipoEstructura.DineroPorCiclo = -2;

            // Assert
            Assert.Equal(-10, tipoEstructura.Capacidad);
            Assert.Equal(-5, tipoEstructura.EnergiaPorCiclo);
            Assert.Equal(-2, tipoEstructura.DineroPorCiclo);
        }
    }
}
