using CivitaBack.Data.BO;
using Xunit;

namespace CivitaBack.Tests
{
    public class TiendaTest
    {
        [Fact]
        public void Tienda_Constructor_InitializesProperties()
        {
            // Act
            var tienda = new Tienda();

            // Assert
            Assert.Equal(0, tienda.Id);
            Assert.Null(tienda.NombreArticulo);
            Assert.Null(tienda.CodigoArticulo);
            Assert.Equal(0, tienda.PartidaId);
            Assert.Null(tienda.Partida);
        }

        [Fact]
        public void Tienda_SetProperties_ValuesAreSet()
        {
            // Arrange
            var tienda = new Tienda();
            var partida = new Partida { Id = 1 };

            // Act
            tienda.Id = 1;
            tienda.NombreArticulo = "Test Item";
            tienda.CodigoArticulo = "TI001";
            tienda.PartidaId = 1;
            tienda.Partida = partida;

            // Assert
            Assert.Equal(1, tienda.Id);
            Assert.Equal("Test Item", tienda.NombreArticulo);
            Assert.Equal("TI001", tienda.CodigoArticulo);
            Assert.Equal(1, tienda.PartidaId);
            Assert.Equal(partida, tienda.Partida);
        }

        [Fact]
        public void Tienda_WithNullValues_PropertiesCanBeNull()
        {
            // Arrange
            var tienda = new Tienda
            {
                Id = 1,
                NombreArticulo = null,
                CodigoArticulo = null,
                PartidaId = 0,
                Partida = null
            };

            // Assert
            Assert.Equal(1, tienda.Id);
            Assert.Null(tienda.NombreArticulo);
            Assert.Null(tienda.CodigoArticulo);
            Assert.Equal(0, tienda.PartidaId);
            Assert.Null(tienda.Partida);
        }
    }
}
