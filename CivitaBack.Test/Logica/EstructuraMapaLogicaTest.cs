using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Excepciones;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Logica;
using CivitaBack.Utils;
using Moq;

namespace CivitaBack.Tests;

public class EstructuraMapaLogicaTest
{
    private readonly Mock<IEstructuraMapaRepositorio> _mockEstructuraMapaRepositorio;
    private readonly Mock<IUnidadDeTrabajo> _mockUow;
    private readonly IEstructuraMapaLogica _estructuraMapaLogica;

    public EstructuraMapaLogicaTest()
    {
        _mockEstructuraMapaRepositorio = new Mock<IEstructuraMapaRepositorio>();
        _mockUow = new Mock<IUnidadDeTrabajo>();

        _estructuraMapaLogica = new EstructuraMapaLogica(
            _mockEstructuraMapaRepositorio.Object,
            _mockUow.Object
        );

        _mockUow.Setup(u => u.CommitAsync()).ReturnsAsync(1);
    }

    [Fact]
    public async Task ReiniciarEstructurasDePartida_IdValido_EliminaEstructuras()
    {
        // Arrange
        const int idPartida = 1;

        _mockEstructuraMapaRepositorio.Setup(r => r.EliminarPorPartidaIdAsync(idPartida))
            .Returns(Task.CompletedTask);

        // Act
        await _estructuraMapaLogica.ReiniciarEstructurasDePartida(idPartida);

        // Assert
        _mockEstructuraMapaRepositorio.Verify(r => r.EliminarPorPartidaIdAsync(idPartida), Times.Once);
        _mockUow.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task EliminarEstructuraAsync_EstructuraExiste_EliminaEstructura()
    {
        // Arrange
        var estructuraMapa = new EstructuraMapa
        {
            PartidaId = 1,
            EstructuraId = 1,
            X = 10,
            Y = 20,
            Width = 50,
            Height = 50
        };
        var estructuraEncontrada = new EstructuraMapa
        {
            Id = 1,
            PartidaId = 1,
            EstructuraId = 1
        };

        _mockEstructuraMapaRepositorio.Setup(r => r.ObtenerCoincidenteAsync(
            estructuraMapa.PartidaId,
            estructuraMapa.EstructuraId,
            estructuraMapa.X,
            estructuraMapa.Y
        )).ReturnsAsync(estructuraEncontrada);
        _mockEstructuraMapaRepositorio.Setup(r => r.EliminarAsync(estructuraEncontrada))
            .Returns(Task.CompletedTask);

        // Act
        await _estructuraMapaLogica.EliminarEstructuraAsync(estructuraMapa);

        // Assert
        _mockEstructuraMapaRepositorio.Verify(r => r.ObtenerCoincidenteAsync(
            estructuraMapa.PartidaId,
            estructuraMapa.EstructuraId,
            estructuraMapa.X,
            estructuraMapa.Y
        ), Times.Once);
        _mockEstructuraMapaRepositorio.Verify(r => r.EliminarAsync(estructuraEncontrada), Times.Once);
        _mockUow.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task EliminarEstructuraAsync_EstructuraNoExiste_LanzaExcepcion()
    {
        // Arrange
        var estructuraMapa = new EstructuraMapa
        {
            PartidaId = 1,
            EstructuraId = 1,
            X = 10,
            Y = 20,
            Width = 50,
            Height = 50
        };

        _mockEstructuraMapaRepositorio.Setup(r => r.ObtenerCoincidenteAsync(
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<int>()
        )).ReturnsAsync((EstructuraMapa?)null);

        // Act & Assert
        await Assert.ThrowsAsync<EstructuraMapaException>(() => _estructuraMapaLogica.EliminarEstructuraAsync(estructuraMapa));
        _mockEstructuraMapaRepositorio.Verify(r => r.EliminarAsync(It.IsAny<EstructuraMapa>()), Times.Never);
    }
}



