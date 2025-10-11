using CivitaBack.Data.BO;
using Xunit;

namespace CivitaBack.Tests
{
    public class RecursoTest
    {
        [Fact]
        public void Recurso_Constructor_InitializesProperties()
        {
            // Act
            var recurso = new Recurso();

            // Assert
            Assert.Equal(0, recurso.Id);
            Assert.Null(recurso.Nombre);
            Assert.Equal(0, recurso.Cantidad);
            Assert.Equal(0, recurso.Felicidad);
            Assert.Equal(0, recurso.Contaminacion);
            Assert.Equal(0, recurso.PartidaId);
            Assert.Null(recurso.Partida);
        }

        [Fact]
        public void Recurso_SetProperties_ValuesAreSet()
        {
            // Arrange
            var recurso = new Recurso();
            var partida = new Partida { Id = 1 };

            // Act
            recurso.Id = 1;
            recurso.Nombre = "Test Resource";
            recurso.Cantidad = 100;
            recurso.Felicidad = 10;
            recurso.Contaminacion = 5;
            recurso.PartidaId = 1;
            recurso.Partida = partida;

            // Assert
            Assert.Equal(1, recurso.Id);
            Assert.Equal("Test Resource", recurso.Nombre);
            Assert.Equal(100, recurso.Cantidad);
            Assert.Equal(10, recurso.Felicidad);
            Assert.Equal(5, recurso.Contaminacion);
            Assert.Equal(1, recurso.PartidaId);
            Assert.Equal(partida, recurso.Partida);
        }

        [Fact]
        public void Recurso_WithNullValues_PropertiesCanBeNull()
        {
            // Arrange
            var recurso = new Recurso
            {
                Id = 1,
                Nombre = null,
                Cantidad = 0,
                Felicidad = 0,
                Contaminacion = 0,
                PartidaId = 0,
                Partida = null
            };

            // Assert
            Assert.Equal(1, recurso.Id);
            Assert.Null(recurso.Nombre);
            Assert.Equal(0, recurso.Cantidad);
            Assert.Equal(0, recurso.Felicidad);
            Assert.Equal(0, recurso.Contaminacion);
            Assert.Equal(0, recurso.PartidaId);
            Assert.Null(recurso.Partida);
        }

        [Fact]
        public void Recurso_WithNegativeValues_CanHandleNegativeNumbers()
        {
            // Arrange
            var recurso = new Recurso();

            // Act
            recurso.Cantidad = -10;
            recurso.Felicidad = -5;
            recurso.Contaminacion = -2;

            // Assert
            Assert.Equal(-10, recurso.Cantidad);
            Assert.Equal(-5, recurso.Felicidad);
            Assert.Equal(-2, recurso.Contaminacion);
        }
    }
}
