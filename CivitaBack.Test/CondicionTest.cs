using CivitaBack.Data.BO;
using Xunit;

namespace CivitaBack.Tests
{
    public class CondicionTest
    {
        [Fact]
        public void Condicion_Constructor_InitializesProperties()
        {
            // Act
            var condicion = new Condicion();

            // Assert
            Assert.Equal(0, condicion.Id);
            Assert.Equal(0, condicion.Cantidad);
            Assert.Equal(0, condicion.EstructuraId);
            Assert.Null(condicion.Estructura);
            Assert.Equal(0, condicion.RecursoId);
            Assert.Null(condicion.Recurso);
        }

        [Fact]
        public void Condicion_SetProperties_ValuesAreSet()
        {
            // Arrange
            var condicion = new Condicion();
            var estructura = new Estructura { Id = 1 };
            var recurso = new Recurso { Id = 1 };

            // Act
            condicion.Id = 1;
            condicion.Cantidad = 10;
            condicion.EstructuraId = 5;
            condicion.Estructura = estructura;
            condicion.RecursoId = 3;
            condicion.Recurso = recurso;

            // Assert
            Assert.Equal(1, condicion.Id);
            Assert.Equal(10, condicion.Cantidad);
            Assert.Equal(5, condicion.EstructuraId);
            Assert.Equal(estructura, condicion.Estructura);
            Assert.Equal(3, condicion.RecursoId);
            Assert.Equal(recurso, condicion.Recurso);
        }

        [Fact]
        public void Condicion_WithNullReferences_PropertiesCanBeNull()
        {
            // Arrange
            var condicion = new Condicion
            {
                Id = 1,
                Cantidad = 5,
                EstructuraId = 2,
                Estructura = null,
                RecursoId = 3,
                Recurso = null
            };

            // Assert
            Assert.Equal(1, condicion.Id);
            Assert.Equal(5, condicion.Cantidad);
            Assert.Equal(2, condicion.EstructuraId);
            Assert.Null(condicion.Estructura);
            Assert.Equal(3, condicion.RecursoId);
            Assert.Null(condicion.Recurso);
        }

        [Fact]
        public void Condicion_WithZeroValues_PropertiesCanBeZero()
        {
            // Arrange
            var condicion = new Condicion
            {
                Id = 0,
                Cantidad = 0,
                EstructuraId = 0,
                RecursoId = 0
            };

            // Assert
            Assert.Equal(0, condicion.Id);
            Assert.Equal(0, condicion.Cantidad);
            Assert.Equal(0, condicion.EstructuraId);
            Assert.Equal(0, condicion.RecursoId);
        }
    }
}
