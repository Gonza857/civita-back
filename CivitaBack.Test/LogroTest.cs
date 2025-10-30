using CivitaBack.Data.BO;
using Xunit;

namespace CivitaBack.Tests
{
    public class LogroTest
    {
        [Fact]
        public void Logro_Constructor_InitializesProperties()
        {
            // Act
            var logro = new LogroEF();

            // Assert
            Assert.Equal(0, logro.Id);
            Assert.Null(logro.Titulo);
            Assert.Equal(string.Empty, logro.Descripcion);
            Assert.Null(logro.TipoLogro);
            Assert.Null(logro.Condicion);
            // La propiedad LogroPartidas no se inicializa automáticamente
        }

        [Fact]
        public void Logro_SetProperties_ValuesAreSet()
        {
            // Arrange
            var logro = new LogroEF();
            var tipoLogro = new TipoLogro { Id = 1, Nombre = "Test Type" };
            var condicion = new CondicionEF { Id = 1, Cantidad = 10 };

            // Act
            logro.Id = 1;
            logro.Titulo = "Test Achievement";
            logro.Descripcion = "Test Description";
            logro.TipoLogro = tipoLogro;
            logro.Condicion = condicion;

            // Assert
            Assert.Equal(1, logro.Id);
            Assert.Equal("Test Achievement", logro.Titulo);
            Assert.Equal("Test Description", logro.Descripcion);
            Assert.Equal(tipoLogro, logro.TipoLogro);
            Assert.Equal(condicion, logro.Condicion);
        }

        [Fact]
        public void Logro_WithNullValues_PropertiesCanBeNull()
        {
            // Arrange
            var logro = new LogroEF
            {
                Id = 1,
                Titulo = null,
                Descripcion = string.Empty,
                TipoLogro = null,
                Condicion = null
            };

            // Assert
            Assert.Equal(1, logro.Id);
            Assert.Null(logro.Titulo);
            Assert.Equal(string.Empty, logro.Descripcion);
            Assert.Null(logro.TipoLogro);
            Assert.Null(logro.Condicion);
        }

        [Fact]
        public void Logro_DefaultDescripcion_IsEmptyString()
        {
            // Act
            var logro = new LogroEF();

            // Assert
            Assert.Equal(string.Empty, logro.Descripcion);
        }
        
        
    }
}
