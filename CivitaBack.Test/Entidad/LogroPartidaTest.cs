using CivitaBack.Data.BO;
using Xunit;

namespace CivitaBack.Tests
{
    public class LogroPartidaTest
    {
        [Fact]
        public void LogroPartida_Constructor_InitializesProperties()
        {
            // Act
            var logroPartida = new LogroPartidaEF();

            // Assert
            Assert.Equal(0, logroPartida.LogroId);
            Assert.Null(logroPartida.Logro);
            Assert.Equal(0, logroPartida.PartidaId);
            Assert.Null(logroPartida.Partida);
        }

        [Fact]
        public void LogroPartida_SetProperties_ValuesAreSet()
        {
            // Arrange
            var logroPartida = new LogroPartidaEF();
            var logro = new LogroEF { Id = 1 };
            var partida = new PartidaEF { Id = 1 };

            // Act
            logroPartida.LogroId = 1;
            logroPartida.Logro = logro;
            logroPartida.PartidaId = 1;
            logroPartida.Partida = partida;

            // Assert
            Assert.Equal(1, logroPartida.LogroId);
            Assert.Equal(logro, logroPartida.Logro);
            Assert.Equal(1, logroPartida.PartidaId);
            Assert.Equal(partida, logroPartida.Partida);
        }

        [Fact]
        public void LogroPartida_WithNullValues_PropertiesCanBeNull()
        {
            // Arrange
            var logroPartida = new LogroPartidaEF
            {
                LogroId = 0,
                Logro = null,
                PartidaId = 0,
                Partida = null
            };

            // Assert
            Assert.Equal(0, logroPartida.LogroId);
            Assert.Null(logroPartida.Logro);
            Assert.Equal(0, logroPartida.PartidaId);
            Assert.Null(logroPartida.Partida);
        }
    }
}
