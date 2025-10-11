using CivitaBack.Data.BO;
using Xunit;

namespace CivitaBack.Tests
{
    public class TipoLogroTest
    {
        [Fact]
        public void TipoLogro_Constructor_InitializesProperties()
        {
            // Act
            var tipoLogro = new TipoLogro();

            // Assert
            Assert.Equal(0, tipoLogro.Id);
            Assert.Equal(string.Empty, tipoLogro.Nombre);
        }

        [Fact]
        public void TipoLogro_SetProperties_ValuesAreSet()
        {
            // Arrange
            var tipoLogro = new TipoLogro();

            // Act
            tipoLogro.Id = 1;
            tipoLogro.Nombre = "Test Type";

            // Assert
            Assert.Equal(1, tipoLogro.Id);
            Assert.Equal("Test Type", tipoLogro.Nombre);
        }

        [Fact]
        public void TipoLogro_DefaultNombre_IsEmptyString()
        {
            // Act
            var tipoLogro = new TipoLogro();

            // Assert
            Assert.Equal(string.Empty, tipoLogro.Nombre);
        }

        [Fact]
        public void TipoLogro_WithEmptyNombre_CanBeSetToEmpty()
        {
            // Arrange
            var tipoLogro = new TipoLogro
            {
                Id = 1,
                Nombre = ""
            };

            // Assert
            Assert.Equal(1, tipoLogro.Id);
            Assert.Equal("", tipoLogro.Nombre);
        }
    }
}
