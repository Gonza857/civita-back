using CivitaBack.Data.BO;
using Xunit;

namespace CivitaBack.Tests
{
    public class EstructuraTest
    {
        [Fact]
        public void Estructura_Constructor_InitializesProperties()
        {
            // Act
            var estructura = new EstructuraEF();

            // Assert
            Assert.Equal(0, estructura.Id);
            Assert.Null(estructura.Nombre);
            Assert.False(estructura.EsMejorable);
            Assert.Null(estructura.RutaImagen);
            Assert.Equal(0, estructura.CostoEnergia);
            Assert.Equal(0, estructura.CostoDinero);
            Assert.Equal(0, estructura.FelicidadCiclo);
            Assert.Equal(0, estructura.ContaminacionCiclo);
            Assert.Equal(0, estructura.TipoEstructuraId);
            Assert.Null(estructura.TipoEstructura);
            Assert.Null(estructura.EstructurasEnMapa);
        }

        [Fact]
        public void Estructura_SetProperties_ValuesAreSet()
        {
            // Arrange
            var estructura = new EstructuraEF();
            var tipoEstructura = new TipoEstructura { Id = 1 };

            // Act
            estructura.Id = 1;
            estructura.Nombre = "Test Structure";
            estructura.EsMejorable = true;
            estructura.RutaImagen = "/images/test.png";
            estructura.CostoEnergia = 100;
            estructura.CostoDinero = 50;
            estructura.FelicidadCiclo = 10;
            estructura.ContaminacionCiclo = 5;
            estructura.TipoEstructuraId = 1;
            estructura.TipoEstructura = tipoEstructura;

            // Assert
            Assert.Equal(1, estructura.Id);
            Assert.Equal("Test Structure", estructura.Nombre);
            Assert.True(estructura.EsMejorable);
            Assert.Equal("/images/test.png", estructura.RutaImagen);
            Assert.Equal(100, estructura.CostoEnergia);
            Assert.Equal(50, estructura.CostoDinero);
            Assert.Equal(10, estructura.FelicidadCiclo);
            Assert.Equal(5, estructura.ContaminacionCiclo);
            Assert.Equal(1, estructura.TipoEstructuraId);
            Assert.Equal(tipoEstructura, estructura.TipoEstructura);
        }

        [Fact]
        public void Estructura_WithNullValues_PropertiesCanBeNull()
        {
            // Arrange
            var estructura = new EstructuraEF
            {
                Id = 1,
                Nombre = null,
                EsMejorable = false,
                RutaImagen = null,
                TipoEstructura = null
            };

            // Assert
            Assert.Equal(1, estructura.Id);
            Assert.Null(estructura.Nombre);
            Assert.False(estructura.EsMejorable);
            Assert.Null(estructura.RutaImagen);
            Assert.Null(estructura.TipoEstructura);
        }

        [Fact]
        public void Estructura_BooleanProperties_CanBeSetToTrue()
        {
            // Arrange
            var estructura = new EstructuraEF();

            // Act
            estructura.EsMejorable = true;

            // Assert
            Assert.True(estructura.EsMejorable);
        }
    }
}
