using AutoMapper;
using CivitaBack.Data.DTO;
using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Logica;
using CivitaBack.Logica.Hubs;
using CivitaBack.Utils;
using Microsoft.AspNetCore.SignalR;
using Moq;
using Xunit;

namespace CivitaBack.Tests
{
    public class EventoTest
    {
        private readonly Mock<IEventoRepositorio> _mockEventoRepositorio;
        private readonly Mock<IUnidadDeTrabajo> _mockUow;
        private readonly Mock<IMapper> _mockMapper;
        private readonly IEventoLogica _eventoLogica;

        public EventoTest()
        {
            // 1. Inicializar los Mocks
            _mockEventoRepositorio = new Mock<IEventoRepositorio>();
            _mockUow = new Mock<IUnidadDeTrabajo>();
            _mockMapper = new Mock<IMapper>();
            // Si tu EventoLogica usa IHubContext, necesitas mockearlo también.

            // 2. Inicializar el objeto bajo prueba (EventoLogica) con las dependencias.
            // **Asegúrate de que el orden de los argumentos coincida con el constructor real de EventoLogica.**
            _eventoLogica = new EventoLogica(
                _mockEventoRepositorio.Object,
                _mockUow.Object,
                _mockMapper.Object
            );
        }

        private EventoMaestro CrearMaestro() => new EventoMaestro
        {
            Id = 10,
            Nombre = "Riesgo",
            TextoDescripcion = "Desc",
            TextoAceptar = "Aceptado",
            TextoRechazar = "Rechazado",
            EcoCoinsAceptar = 10,
            FelicidadAceptar = 5,
            ContaminacionAceptar = 5,
            FelicidadRechazar = -10,
            ContaminacionRechazar = 10
        };
        private Evento CrearEventoBase() => new Evento
        {
            Id = 1,
            TextoAceptar = "Aceptado",
            TextoRechazar = "Rechazado",
            EcoCoinsAceptar = 10,
            FelicidadAceptar = 5,
            ContaminacionAceptar = 5,
            FelicidadRechazar = -10,
            ContaminacionRechazar = 10
        };
        private Partida CrearPartida(int id) => new Partida { Id = id, Recursos = new Recurso { EcoCoins = 100, Contaminacion = 50, Felicidad = 50 } };

        [Fact]
        public async Task DispararEventoAsync_CreacionYCommit_Exito()
        {
            // Arrange
            const int partidaId = 5;
            var maestroMock = CrearMaestro();
            var eventoDominioCreado = CrearEventoBase();

            eventoDominioCreado.EventoMaestro = maestroMock;
            eventoDominioCreado.EventoMaestroId = maestroMock.Id;
            eventoDominioCreado.Partida = CrearPartida(partidaId);

            // Simula que el repositorio devuelve el EventoMaestro
            _mockEventoRepositorio
                .Setup(r => r.ObtenerEventoMaestroAsync())
                .ReturnsAsync(maestroMock);

            // Simula el mapeo de EventoMaestro -> Evento
            _mockMapper
                .Setup(m => m.Map<Evento>(maestroMock))
                .Returns(eventoDominioCreado);

            // Simula el mapeo de Evento -> EventoDisparadoDTO
            _mockMapper
                .Setup(m => m.Map<EventoDisparadoDTO>(It.IsAny<Evento>()))
                .Returns(new EventoDisparadoDTO { Id = eventoDominioCreado.Id });

            // Act
            var resultado = await _eventoLogica.DispararEventoAsync(partidaId);

            // Assert
            Assert.NotNull(resultado);

            Assert.Equal(partidaId, eventoDominioCreado.PartidaId);
            Assert.True(eventoDominioCreado.SeDisparo);

            _mockEventoRepositorio.Verify(
            r => r.CrearEventoAsync(
            It.Is<Evento>(e =>
                e.PartidaId == partidaId &&
                e.EventoMaestroId == maestroMock.Id && // Asegurar que la FK fue copiada/usada
                e.EcoCoinsAceptar == 10
            )
        ),
        Times.Once
    );

            // Verificar que se persistió la transacción
            _mockUow.Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task DispararEventoAsync_SinMaestro_RetornaNull()
        {
            // Arrange
            // Simula que el repositorio devuelve null
            _mockEventoRepositorio
                .Setup(r => r.ObtenerEventoMaestroAsync())
                .ReturnsAsync((EventoMaestro)null);

            // Act
            var resultado = await _eventoLogica.DispararEventoAsync(12);

            // Assert
            Assert.Null(resultado);
            // Verificar que NO se intentó guardar nada
            _mockUow.Verify(u => u.CommitAsync(), Times.Never());
        }

        [Fact]
        public async Task ResolverEventoAsync_Aceptado_AplicaEfectosYCommit()
        {
            // Arrange
            const int eventoId = 1;
            var partida = CrearPartida(100); 
            var evento = CrearEventoBase();
            var recursos = partida.Recursos;

            // Configurar los efectos en el evento para el cálculo
            evento.EcoCoinsAceptar = 50;
            evento.FelicidadAceptar = 10;
            evento.ContaminacionAceptar = -5;
            evento.Partida = partida;

            // Simular la obtención del evento con su partida asociada
            _mockEventoRepositorio
                .Setup(r => r.ObtenerEventoConPartidaAsync(eventoId))
                .ReturnsAsync(evento);

            // Simular el mapeo de la respuesta
            _mockMapper
                .Setup(m => m.Map<EventoResueltoDTO>(It.IsAny<object>()))
                .Returns(new EventoResueltoDTO { Id = eventoId });

            // Act
            await _eventoLogica.ResolverEventoAsync(eventoId, true); // Aceptar

            // Assert
            Assert.Equal(150, partida.Recursos.EcoCoins);
            Assert.Equal(60, partida.Recursos.Felicidad);
            Assert.Equal(45, partida.Recursos.Contaminacion);
            Assert.True(evento.Resuelto);

            _mockUow.Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task ResolverEventoAsync_YaResuelto_LanzaExcepcion()
        {
            // Arrange
            const int eventoId = 2;
            var partida = CrearPartida(100);
            var eventoYaResuelto = CrearEventoBase();
            eventoYaResuelto.Resuelto = true; // Marcarlo como resuelto
            eventoYaResuelto.Partida = partida;

            _mockEventoRepositorio
                .Setup(r => r.ObtenerEventoConPartidaAsync(eventoId))
                .ReturnsAsync(eventoYaResuelto);

            // Act & Assert
            // Se espera que lance una excepción (tu servicio lanza Exception genérica)
            await Assert.ThrowsAsync<Exception>(() => _eventoLogica.ResolverEventoAsync(eventoId, true));

            // Verificar que NO se intentó guardar
            _mockUow.Verify(u => u.CommitAsync(), Times.Never());
        }
    }

}
