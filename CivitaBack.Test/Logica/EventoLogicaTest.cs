using AutoMapper;
using CivitaBack.Data.DTO;
using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Logica;
using CivitaBack.Logica.Interfaces;
using CivitaBack.Utils;
using Moq;

namespace CivitaBack.Tests;

public class EventoLogicaTest
{
    private readonly Mock<IEventoRepositorio> _mockEventoRepositorio;
    private readonly Mock<IPartidaRepositorio> _mockPartidaRepositorio;
    private readonly Mock<IUnidadDeTrabajo> _mockUow;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IActualizarRecursosLogica> _mockActualizarRecursosLogica;
    private readonly IEventoLogica _eventoLogica;

    public EventoLogicaTest()
    {
        _mockEventoRepositorio = new Mock<IEventoRepositorio>();
        _mockPartidaRepositorio = new Mock<IPartidaRepositorio>();
        _mockUow = new Mock<IUnidadDeTrabajo>();
        _mockMapper = new Mock<IMapper>();
        _mockActualizarRecursosLogica = new Mock<IActualizarRecursosLogica>();

        _eventoLogica = new EventoLogica(
            _mockEventoRepositorio.Object,
            _mockPartidaRepositorio.Object,
            _mockUow.Object,
            _mockMapper.Object,
            _mockActualizarRecursosLogica.Object
        );

        _mockUow.Setup(u => u.CommitAsync()).ReturnsAsync(1);
    }

    [Fact]
    public async Task DispararEventoAsync_EventoMaestroExiste_RetornaEventoDisparado()
    {
        // Arrange
        const int idPartida = 1;
        var eventoMaestro = new EventoMaestro
        {
            Id = 1,
            Nombre = "Evento Test",
            TextoDescripcion = "Descripción Test"
        };
        var evento = new Evento
        {
            Id = 1,
            EventoMaestroId = 1,
            PartidaId = idPartida,
            SeDisparo = true,
            Resuelto = false
        };
        var eventoDisparadoDTO = new EventoDisparadoDTO
        {
            Id = 1
        };

        _mockEventoRepositorio.Setup(r => r.ObtenerEventoMaestroAsync())
            .ReturnsAsync(eventoMaestro);
        _mockMapper.Setup(m => m.Map<Evento>(eventoMaestro))
            .Returns(evento);
        _mockEventoRepositorio.Setup(r => r.CrearEventoAsync(It.IsAny<Evento>()))
            .ReturnsAsync(evento);
        _mockMapper.Setup(m => m.Map<EventoDisparadoDTO>(It.IsAny<Evento>()))
            .Returns(eventoDisparadoDTO);

        // Act
        var resultado = await _eventoLogica.DispararEventoAsync(idPartida);

        // Assert
        Assert.NotNull(resultado);
        _mockEventoRepositorio.Verify(r => r.ObtenerEventoMaestroAsync(), Times.Once);
        _mockEventoRepositorio.Verify(r => r.CrearEventoAsync(It.Is<Evento>(e =>
            e.EventoMaestroId == 1 &&
            e.PartidaId == idPartida &&
            e.SeDisparo == true &&
            e.Resuelto == false
        )), Times.Once);
    }

    [Fact]
    public async Task DispararEventoAsync_EventoMaestroNoExiste_RetornaNull()
    {
        // Arrange
        const int idPartida = 1;

        _mockEventoRepositorio.Setup(r => r.ObtenerEventoMaestroAsync())
            .ReturnsAsync((EventoMaestro?)null);

        // Act
        var resultado = await _eventoLogica.DispararEventoAsync(idPartida);

        // Assert
        Assert.Null(resultado);
        _mockEventoRepositorio.Verify(r => r.CrearEventoAsync(It.IsAny<Evento>()), Times.Never);
    }

    [Fact]
    public async Task ResolverEventoAsync_AceptaEvento_ActualizaRecursos()
    {
        // Arrange
        const int eventoId = 1;
        const bool acepto = true;
        var partida = new Partida
        {
            Id = 1,
            Recursos = new Recurso
            {
                Felicidad = 50,
                Contaminacion = 30,
                EcoCoins = 100
            }
        };
        var evento = new Evento
        {
            Id = eventoId,
            PartidaId = 1,
            Partida = partida,
            Resuelto = false,
            FelicidadAceptar = 10,
            ContaminacionAceptar = 5,
            EcoCoinsAceptar = 50,
            FelicidadRechazar = -5,
            ContaminacionRechazar = -2
        };
        var eventoResueltoDTO = new EventoResueltoDTO
        {
            Id = eventoId
        };

        _mockEventoRepositorio.Setup(r => r.ObtenerEventoConPartidaAsync(eventoId))
            .ReturnsAsync(evento);
        _mockEventoRepositorio.Setup(r => r.Actualizar(It.IsAny<Evento>()))
            .Returns(Task.CompletedTask);
        _mockPartidaRepositorio.Setup(r => r.Actualizar(It.IsAny<Partida>()))
            .Returns(Task.CompletedTask);
        _mockMapper.Setup(m => m.Map<EventoResueltoDTO>(
            It.IsAny<Evento>(), 
            It.IsAny<Action<IMappingOperationOptions<object, EventoResueltoDTO>>>()))
            .Returns(eventoResueltoDTO);

        // Act
        var resultado = await _eventoLogica.ResolverEventoAsync(eventoId, acepto);

        // Assert
        Assert.NotNull(resultado);
        _mockActualizarRecursosLogica.Verify(a => a.ActualizarRecursosAsync(
            partida,
            10, // FelicidadAceptar
            5,  // ContaminacionAceptar
            50, // EcoCoinsAceptar
            0   // cambioEnergia
        ), Times.Once);
        _mockEventoRepositorio.Verify(r => r.Actualizar(It.Is<Evento>(e => e.Resuelto == true)), Times.Once);
        _mockPartidaRepositorio.Verify(r => r.Actualizar(partida), Times.Once);
        _mockUow.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task ResolverEventoAsync_RechazaEvento_ActualizaRecursos()
    {
        // Arrange
        const int eventoId = 1;
        const bool acepto = false;
        var partida = new Partida
        {
            Id = 1,
            Recursos = new Recurso
            {
                Felicidad = 50,
                Contaminacion = 30
            }
        };
        var evento = new Evento
        {
            Id = eventoId,
            PartidaId = 1,
            Partida = partida,
            Resuelto = false,
            FelicidadAceptar = 10,
            ContaminacionAceptar = 5,
            FelicidadRechazar = -5,
            ContaminacionRechazar = -2
        };
        var eventoResueltoDTO = new EventoResueltoDTO { Id = eventoId };

        _mockEventoRepositorio.Setup(r => r.ObtenerEventoConPartidaAsync(eventoId))
            .ReturnsAsync(evento);
        _mockEventoRepositorio.Setup(r => r.Actualizar(It.IsAny<Evento>()))
            .Returns(Task.CompletedTask);
        _mockPartidaRepositorio.Setup(r => r.Actualizar(It.IsAny<Partida>()))
            .Returns(Task.CompletedTask);
        _mockMapper.Setup(m => m.Map<EventoResueltoDTO>(
            It.IsAny<Evento>(), 
            It.IsAny<Action<IMappingOperationOptions<object, EventoResueltoDTO>>>()))
            .Returns(eventoResueltoDTO);

        // Act
        var resultado = await _eventoLogica.ResolverEventoAsync(eventoId, acepto);

        // Assert
        Assert.NotNull(resultado);
        _mockActualizarRecursosLogica.Verify(a => a.ActualizarRecursosAsync(
            partida,
            -5, // FelicidadRechazar
            -2, // ContaminacionRechazar
            0,  // EcoCoins (no se da al rechazar)
            0   // cambioEnergia
        ), Times.Once);
    }

    [Fact]
    public async Task ResolverEventoAsync_EventoNoExiste_LanzaExcepcion()
    {
        // Arrange
        const int eventoId = 999;

        _mockEventoRepositorio.Setup(r => r.ObtenerEventoConPartidaAsync(eventoId))
            .ReturnsAsync((Evento?)null);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _eventoLogica.ResolverEventoAsync(eventoId, true));
    }

    [Fact]
    public async Task ResolverEventoAsync_EventoYaResuelto_LanzaExcepcion()
    {
        // Arrange
        const int eventoId = 1;
        var evento = new Evento
        {
            Id = eventoId,
            Resuelto = true
        };

        _mockEventoRepositorio.Setup(r => r.ObtenerEventoConPartidaAsync(eventoId))
            .ReturnsAsync(evento);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _eventoLogica.ResolverEventoAsync(eventoId, true));
    }

    [Fact]
    public async Task ResolverEventoAsync_PartidaNull_LanzaExcepcion()
    {
        // Arrange
        const int eventoId = 1;
        var evento = new Evento
        {
            Id = eventoId,
            PartidaId = 1,
            Partida = null,
            Resuelto = false
        };

        _mockEventoRepositorio.Setup(r => r.ObtenerEventoConPartidaAsync(eventoId))
            .ReturnsAsync(evento);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _eventoLogica.ResolverEventoAsync(eventoId, true));
    }
}

