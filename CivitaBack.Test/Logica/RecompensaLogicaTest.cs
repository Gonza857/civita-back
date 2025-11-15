using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Logica;
using CivitaBack.Utils;
using Moq;

namespace CivitaBack.Tests;

public class RecompensaLogicaTest
{
    private readonly Mock<IPartidaRepositorio> _mockPartidaRepositorio;
    private readonly Mock<IUnidadDeTrabajo> _mockUnidadDeTrabajo;
    private readonly IRecompensaLogica _recompensaLogica;

    public RecompensaLogicaTest()
    {
        _mockPartidaRepositorio = new Mock<IPartidaRepositorio>();
        _mockUnidadDeTrabajo = new Mock<IUnidadDeTrabajo>();

        _recompensaLogica = new RecompensaLogica(
            _mockPartidaRepositorio.Object,
            _mockUnidadDeTrabajo.Object
        );

        _mockUnidadDeTrabajo.Setup(u => u.CommitAsync()).ReturnsAsync(1);
    }

    [Fact]
    public async Task ReclamarRecompensas_RecompensasValidas_AplicaRecompensas()
    {
        // Arrange
        var partida = new Partida
        {
            Id = 1,
            Recursos = new Recurso
            {
                EcoCoins = 100,
                Felicidad = 50,
                Energia = 75,
                Contaminacion = 30
            }
        };
        var recompensas = new List<Condicion>
        {
            new Condicion { NombreColumna = "EcoCoins", Cantidad = 50, EsRecompensa = true },
            new Condicion { NombreColumna = "Felicidad", Cantidad = 10, EsRecompensa = true }
        };

        _mockPartidaRepositorio.Setup(r => r.Actualizar(It.IsAny<Partida>()))
            .Returns(Task.CompletedTask);

        // Act
        await _recompensaLogica.ReclamarRecompensas(recompensas, partida);

        // Assert
        Assert.Equal(150, partida.Recursos.EcoCoins);
        Assert.Equal(60, partida.Recursos.Felicidad);
        _mockPartidaRepositorio.Verify(r => r.Actualizar(partida), Times.Once);
        _mockUnidadDeTrabajo.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task ReclamarRecompensas_PartidaNull_LanzaExcepcion()
    {
        // Arrange
        var recompensas = new List<Condicion>
        {
            new Condicion { EsRecompensa = true }
        };

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _recompensaLogica.ReclamarRecompensas(recompensas, null!));
    }

    [Fact]
    public async Task ReclamarRecompensas_RecursosNull_LanzaExcepcion()
    {
        // Arrange
        var partida = new Partida
        {
            Id = 1,
            Recursos = null
        };
        var recompensas = new List<Condicion>
        {
            new Condicion { EsRecompensa = true }
        };

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _recompensaLogica.ReclamarRecompensas(recompensas, partida));
    }

    [Fact]
    public async Task ReclamarRecompensas_RecompensaInvalida_LanzaExcepcion()
    {
        // Arrange
        var partida = new Partida
        {
            Id = 1,
            Recursos = new Recurso()
        };
        var recompensas = new List<Condicion>
        {
            new Condicion { EsRecompensa = false }
        };

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _recompensaLogica.ReclamarRecompensas(recompensas, partida));
    }

    [Fact]
    public async Task ReclamarRecompensa_RecompensaValida_AplicaRecompensa()
    {
        // Arrange
        var partida = new Partida
        {
            Id = 1,
            Recursos = new Recurso
            {
                Energia = 50
            }
        };
        var recompensa = new Condicion
        {
            NombreColumna = "Energia",
            Cantidad = 25,
            EsRecompensa = true
        };

        _mockPartidaRepositorio.Setup(r => r.Actualizar(It.IsAny<Partida>()))
            .Returns(Task.CompletedTask);

        // Act
        await _recompensaLogica.ReclamarRecompensa(recompensa, partida);

        // Assert
        Assert.Equal(75, partida.Recursos.Energia);
        _mockPartidaRepositorio.Verify(r => r.Actualizar(partida), Times.Once);
    }

    [Fact]
    public async Task ReclamarRecompensa_PartidaNull_LanzaExcepcion()
    {
        // Arrange
        var recompensa = new Condicion { EsRecompensa = true };

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _recompensaLogica.ReclamarRecompensa(recompensa, null!));
    }

    [Fact]
    public async Task ReclamarRecompensa_RecompensaInvalida_LanzaExcepcion()
    {
        // Arrange
        var partida = new Partida
        {
            Id = 1,
            Recursos = new Recurso()
        };
        var recompensa = new Condicion { EsRecompensa = false };

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _recompensaLogica.ReclamarRecompensa(recompensa, partida));
    }

    [Fact]
    public async Task ReclamarRecompensas_RecompensaConColumnaInvalida_IgnoraRecompensa()
    {
        // Arrange
        var partida = new Partida
        {
            Id = 1,
            Recursos = new Recurso
            {
                EcoCoins = 100
            }
        };
        var recompensas = new List<Condicion>
        {
            new Condicion { NombreColumna = "EcoCoins", Cantidad = 50, EsRecompensa = true },
            new Condicion { NombreColumna = "ColumnaInexistente", Cantidad = 10, EsRecompensa = true }
        };

        _mockPartidaRepositorio.Setup(r => r.Actualizar(It.IsAny<Partida>()))
            .Returns(Task.CompletedTask);

        // Act
        await _recompensaLogica.ReclamarRecompensas(recompensas, partida);

        // Assert
        Assert.Equal(150, partida.Recursos.EcoCoins);
        _mockPartidaRepositorio.Verify(r => r.Actualizar(partida), Times.Once);
    }
}
