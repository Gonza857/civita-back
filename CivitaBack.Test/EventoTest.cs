using CivitaBack.Data.BO;
using Xunit;

namespace CivitaBack.Tests
{
    public class EventoTest
    {
        /*[Fact]
        public void Evento_Constructor_InitializesProperties()
        {
            // Act
            var evento = new Evento();

            // Assert
            Assert.Equal(0, evento.Id);
            Assert.Null(evento.DescripcionEvento);
            Assert.Equal(0, evento.TiempoParaHacerlo);
            Assert.Equal(0, evento.EventoMaestroId);
            Assert.Null(evento.EventoMaestro);
            Assert.Equal(0, evento.PartidaId);
            Assert.Null(evento.Partida);
        }

        [Fact]
        public void Evento_SetProperties_ValuesAreSet()
        {
            // Arrange
            var evento = new Evento();
            var eventoMaestro = new EventoMaestro { Id = 1 };
            var partida = new Partida { Id = 1 };

            // Act
            evento.Id = 1;
            evento.DescripcionEvento = "Test Event Description";
            evento.TiempoParaHacerlo = 30;
            evento.EventoMaestroId = 1;
            evento.EventoMaestro = eventoMaestro;
            evento.PartidaId = 1;
            evento.Partida = partida;

            // Assert
            Assert.Equal(1, evento.Id);
            Assert.Equal("Test Event Description", evento.DescripcionEvento);
            Assert.Equal(30, evento.TiempoParaHacerlo);
            Assert.Equal(1, evento.EventoMaestroId);
            Assert.Equal(eventoMaestro, evento.EventoMaestro);
            Assert.Equal(1, evento.PartidaId);
            Assert.Equal(partida, evento.Partida);
        }

        [Fact]
        public void Evento_WithNullValues_PropertiesCanBeNull()
        {
            // Arrange
            var evento = new Evento
            {
                Id = 1,
                DescripcionEvento = null,
                TiempoParaHacerlo = 0,
                EventoMaestroId = 0,
                EventoMaestro = null,
                PartidaId = 0,
                Partida = null
            };

            // Assert
            Assert.Equal(1, evento.Id);
            Assert.Null(evento.DescripcionEvento);
            Assert.Equal(0, evento.TiempoParaHacerlo);
            Assert.Equal(0, evento.EventoMaestroId);
            Assert.Null(evento.EventoMaestro);
            Assert.Equal(0, evento.PartidaId);
            Assert.Null(evento.Partida);
        }

        [Fact]
        public void Evento_WithNegativeTime_CanHandleNegativeNumbers()
        {
            // Arrange
            var evento = new Evento();

            // Act
            evento.TiempoParaHacerlo = -10;

            // Assert
            Assert.Equal(-10, evento.TiempoParaHacerlo);
        }*/
    }
}
