using CivitaBack.Data.BO;
using Xunit;

namespace CivitaBack.Tests
{
    public class PartidaTest
    {
        [Fact]
        public void Partida_Constructor_InitializesProperties()
        {
            // Act
            var partida = new Partida();

            // Assert
            Assert.Equal(0, partida.Id);
            Assert.Null(partida.UltimaVez);
            Assert.Null(partida.JsonMapa);
            Assert.Equal(0, partida.UsuarioId);
            Assert.Null(partida.Usuario);
            Assert.Null(partida.Recurso);
            Assert.Null(partida.Evento);
            Assert.Null(partida.Tienda);
            Assert.Null(partida.EstructuraEnMapa);
            Assert.Null(partida.TipEnPartida);
            // La propiedad LogroPartidas no se inicializa automáticamente
        }

        [Fact]
        public void Partida_SetProperties_ValuesAreSet()
        {
            // Arrange
            var partida = new Partida();
            var fecha = DateTime.Now;

            // Act
            partida.Id = 1;
            partida.UltimaVez = fecha;
            partida.JsonMapa = "{\"test\": \"data\"}";
            partida.UsuarioId = 5;

            // Assert
            Assert.Equal(1, partida.Id);
            Assert.Equal(fecha, partida.UltimaVez);
            Assert.Equal("{\"test\": \"data\"}", partida.JsonMapa);
            Assert.Equal(5, partida.UsuarioId);
        }

        [Fact]
        public void Partida_WithNullValues_PropertiesCanBeNull()
        {
            // Arrange
            var partida = new Partida
            {
                Id = 1,
                UltimaVez = null,
                JsonMapa = null,
                UsuarioId = 0,
                Usuario = null
            };

            // Assert
            Assert.Equal(1, partida.Id);
            Assert.Null(partida.UltimaVez);
            Assert.Null(partida.JsonMapa);
            Assert.Equal(0, partida.UsuarioId);
            Assert.Null(partida.Usuario);
        }

        [Fact]
        public void Partida_WithDateTime_HandlesDateTimeCorrectly()
        {
            // Arrange
            var partida = new Partida();
            var specificDate = new DateTime(2023, 12, 25, 10, 30, 0);

            // Act
            partida.UltimaVez = specificDate;

            // Assert
            Assert.Equal(specificDate, partida.UltimaVez);
            Assert.Equal(2023, partida.UltimaVez.Value.Year);
            Assert.Equal(12, partida.UltimaVez.Value.Month);
            Assert.Equal(25, partida.UltimaVez.Value.Day);
        }
    }
}
