using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Logica;
using CivitaBack.Logica.Backgrounds;
using CivitaBack.Logica.Hubs;
using CivitaBack.Utils;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Moq;

namespace CivitaBack.Tests;

public class CicloLogicaTest
{
    private readonly Mock<IPartidaRepositorio> _mockPartidaRepositorio;
    private readonly Mock<IUnidadDeTrabajo> _mockUow;
    private readonly Mock<IActualizarRecursosLogica> _mockActualizarRecursosLogica;
    private readonly ICicloLogica _cicloLogica;

    public CicloLogicaTest()
    {
        _mockPartidaRepositorio = new Mock<IPartidaRepositorio>();
        _mockUow = new Mock<IUnidadDeTrabajo>();
        _mockActualizarRecursosLogica = new Mock<IActualizarRecursosLogica>();

        _cicloLogica = new CicloLogica(
            _mockPartidaRepositorio.Object,
            _mockUow.Object,
            _mockActualizarRecursosLogica.Object
        );

        _mockUow.Setup(u => u.CommitAsync()).ReturnsAsync(1);
    }

    [Fact]
    public async Task EjecutarCicloAsync_HayPartidas_ProcesaPartidas()
    {
        // Arrange
        var tipoEstructura = new TipoEstructura
        {
            Id = 1,
            EnergiaPorCiclo = 10,
            DineroPorCiclo = 5,
            Capacidad = 100
        };
        var estructura = new Estructura
        {
            Id = 1,
            FelicidadCiclo = 2,
            ContaminacionCiclo = 1,
            TipoEstructura = tipoEstructura
        };
        var estructuraMapa = new EstructuraMapa
        {
            EstructuraId = 1,
            Estructura = estructura
        };
        var partida = new Partida
        {
            Id = 1,
            Recursos = new Recurso
            {
                Contaminacion = 50
            },
            EstructuraMapa = new List<EstructuraMapa> { estructuraMapa }
        };

        var partidas = new List<Partida> { partida };

        _mockPartidaRepositorio.Setup(r => r.ObtenerTodasConEstructurasYRecursosAsync())
            .ReturnsAsync(partidas);
        _mockPartidaRepositorio.Setup(r => r.Actualizar(It.IsAny<Partida>()))
            .Returns(Task.CompletedTask);

        // Act
        var resultado = await _cicloLogica.EjecutarCicloAsync();

        // Assert
        Assert.NotNull(resultado);
        Assert.Single(resultado);
        _mockPartidaRepositorio.Verify(r => r.ObtenerTodasConEstructurasYRecursosAsync(), Times.Once);
        _mockPartidaRepositorio.Verify(r => r.Actualizar(It.IsAny<Partida>()), Times.Once);
        _mockActualizarRecursosLogica.Verify(a => a.ActualizarRecursosAsync(
            It.IsAny<Partida>(),
            It.IsAny<int>(), // Felicidad
            It.IsAny<int>(), // Contaminacion
            It.IsAny<int>(), // EcoCoins
            It.IsAny<int>()  // Energia
        ), Times.Once);
        _mockUow.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task EjecutarCicloAsync_ListaVacia_RetornaListaVacia()
    {
        // Arrange
        _mockPartidaRepositorio.Setup(r => r.ObtenerTodasConEstructurasYRecursosAsync())
            .ReturnsAsync(new List<Partida>());

        // Act
        var resultado = await _cicloLogica.EjecutarCicloAsync();

        // Assert
        Assert.NotNull(resultado);
        Assert.Empty(resultado);
        _mockPartidaRepositorio.Verify(r => r.ObtenerTodasConEstructurasYRecursosAsync(), Times.Once);
        _mockPartidaRepositorio.Verify(r => r.Actualizar(It.IsAny<Partida>()), Times.Never);
    }

    [Fact]
    public async Task EjecutarCicloAsync_ListaNull_RetornaListaVacia()
    {
        // Arrange
        _mockPartidaRepositorio.Setup(r => r.ObtenerTodasConEstructurasYRecursosAsync())
            .ReturnsAsync((List<Partida>?)null);

        // Act
        var resultado = await _cicloLogica.EjecutarCicloAsync();

        // Assert
        Assert.NotNull(resultado);
        Assert.Empty(resultado);
    }

    [Fact]
    public async Task EjecutarCicloAsync_ContaminacionAlta_ReduceFelicidad()
    {
        // Arrange
        var partida = new Partida
        {
            Id = 1,
            Recursos = new Recurso
            {
                Contaminacion = 90 // Mayor a 80
            },
            EstructuraMapa = new List<EstructuraMapa>()
        };

        var partidas = new List<Partida> { partida };

        _mockPartidaRepositorio.Setup(r => r.ObtenerTodasConEstructurasYRecursosAsync())
            .ReturnsAsync(partidas);
        _mockPartidaRepositorio.Setup(r => r.Actualizar(It.IsAny<Partida>()))
            .Returns(Task.CompletedTask);

        // Act
        await _cicloLogica.EjecutarCicloAsync();

        // Assert
        _mockActualizarRecursosLogica.Verify(a => a.ActualizarRecursosAsync(
            It.IsAny<Partida>(),
            -3,// Felicidad reducida por contaminación alta
            It.IsAny<int>(), 
            It.IsAny<int>(),
            It.IsAny<int>()
        ), Times.Once);
    }

    [Fact]
    public async Task EjecutarCicloAsync_ContaminacionBaja_AumentaFelicidad()
    {
        // Arrange
        var partida = new Partida
        {
            Id = 1,
            Recursos = new Recurso
            {
                Contaminacion = 5 // Menor a 10
            },
            EstructuraMapa = new List<EstructuraMapa>()
        };

        var partidas = new List<Partida> { partida };

        _mockPartidaRepositorio.Setup(r => r.ObtenerTodasConEstructurasYRecursosAsync())
            .ReturnsAsync(partidas);
        _mockPartidaRepositorio.Setup(r => r.Actualizar(It.IsAny<Partida>()))
            .Returns(Task.CompletedTask);

        // Act
        await _cicloLogica.EjecutarCicloAsync();

        // Assert
        _mockActualizarRecursosLogica.Verify(a => a.ActualizarRecursosAsync(
            It.IsAny<Partida>(),
            3, // Felicidad aumentada por contaminación baja
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<int>()
        ), Times.Once);
    }

    [Fact]
    public async Task EjecutarCicloAsync_PartidaNull_LanzaExcepcion()
    {
        // Arrange
        var partidas = new List<Partida> { null! };

        _mockPartidaRepositorio.Setup(r => r.ObtenerTodasConEstructurasYRecursosAsync())
            .ReturnsAsync(partidas);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _cicloLogica.EjecutarCicloAsync());
    }

    [Fact]
    public async Task EjecutarCicloAsync_RecursosNull_LanzaExcepcion()
    {
        // Arrange
        var partida = new Partida
        {
            Id = 1,
            Recursos = null
        };
        var partidas = new List<Partida> { partida };

        _mockPartidaRepositorio.Setup(r => r.ObtenerTodasConEstructurasYRecursosAsync())
            .ReturnsAsync(partidas);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _cicloLogica.EjecutarCicloAsync());
    }
}



