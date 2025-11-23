using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Excepciones;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Logica;
using CivitaBack.Utils;
using Moq;

namespace CivitaBack.Tests;

public class MisionPartidaLogicaTest
{
    private readonly Mock<IMisionPartidaRepositorio> _mockMisionPartidaRepositorio;
    private readonly Mock<IUnidadDeTrabajo> _mockUnidadDeTrabajo;

    private readonly IMisionPartidaLogica _misionPartidaLogica;

    public MisionPartidaLogicaTest()
    {
        _mockMisionPartidaRepositorio = new Mock<IMisionPartidaRepositorio>();
        _mockUnidadDeTrabajo = new Mock<IUnidadDeTrabajo>();

        _misionPartidaLogica = new MisionPartidaLogica(
            _mockMisionPartidaRepositorio.Object,
            _mockUnidadDeTrabajo.Object
        );

        // Mockeo por defecto para el CommitAsync, para no repetirlo
        _mockUnidadDeTrabajo.Setup(u => u.CommitAsync()).ReturnsAsync(1);
    }

    // --- Tests para ObtenerMisionPartidaPorId ---

    [Fact]
    public async Task ObtenerMisionPartidaPorId_MisionExiste_DebeRetornarMision()
    {
        // Arrange
        var misionEsperada = new Mision { Id = 1, Titulo = "Test" };
        _mockMisionPartidaRepositorio.Setup(r => r.ObtenerMisionPartidaPorId(1, 1))
            .ReturnsAsync(misionEsperada);

        // Act
        var resultado = await _misionPartidaLogica.ObtenerMisionPartidaPorId(1, 1);

        // Assert
        Assert.NotNull(resultado);
        Assert.Same(misionEsperada, resultado);
    }

    [Fact]
    public async Task ObtenerMisionPartidaPorId_MisionNoExiste_DebeLanzarExcepcion()
    {
        // Arrange
        _mockMisionPartidaRepositorio.Setup(r => r.ObtenerMisionPartidaPorId(1, 99))
            .ReturnsAsync((Mision)null);

        // Act & Assert
        await Assert.ThrowsAsync<MisionPartidaExcepcion>(
            () => _misionPartidaLogica.ObtenerMisionPartidaPorId(1, 99)
        );
    }

    // --- Test para AsignarMisiones ---

    [Fact]
    public async Task AsignarMisiones_ConDatosValidos_DebeLlamarAlRepositorioYGuardar()
    {
        // Arrange
        var partida = new Partida { Id = 1 };
        var misiones = new List<Mision> { new Mision { Id = 1 } };

        _mockMisionPartidaRepositorio.Setup(r => r.AgregarMisionesPartida(misiones, partida))
            .Returns(Task.CompletedTask);

        // Act
        await _misionPartidaLogica.AsignarMisiones(misiones, partida);

        // Assert
        _mockMisionPartidaRepositorio.Verify(r => r.AgregarMisionesPartida(misiones, partida), Times.Once);
        _mockUnidadDeTrabajo.Verify(u => u.CommitAsync(), Times.Once);
    }

    // --- Tests para Métodos de Listado (Día, Semana, Mes) ---

    [Fact]
    public async Task ObtenerMisionesDia_DebeLlamarAlRepositorioCorrecto()
    {
        // Arrange
        var misionesEsperadas = new List<Mision> { new Mision { Id = 1, Titulo = "Diaria" } };
        _mockMisionPartidaRepositorio.Setup(r => r.ObtenerMisionesDia(1))
            .ReturnsAsync(misionesEsperadas);

        // Act
        var resultado = await _misionPartidaLogica.ObtenerMisionesDia(1);

        // Assert
        Assert.Same(misionesEsperadas, resultado);
        _mockMisionPartidaRepositorio.Verify(r => r.ObtenerMisionesDia(1), Times.Once);
    }

    [Fact]
    public async Task ObtenerMisionesSemana_DebeLlamarAlRepositorioCorrecto()
    {
        // Arrange
        var misionesEsperadas = new List<Mision> { new Mision { Id = 2, Titulo = "Semanal" } };
        _mockMisionPartidaRepositorio.Setup(r => r.ObtenerMisionesSemana(1))
            .ReturnsAsync(misionesEsperadas);

        // Act
        var resultado = await _misionPartidaLogica.ObtenerMisionesSemana(1);

        // Assert
        Assert.Same(misionesEsperadas, resultado);
        _mockMisionPartidaRepositorio.Verify(r => r.ObtenerMisionesSemana(1), Times.Once);
    }

    [Fact]
    public async Task ObtenerMisionesMes_DebeLlamarAlRepositorioCorrecto()
    {
        // Arrange
        var misionesEsperadas = new List<Mision> { new Mision { Id = 3, Titulo = "Mensual" } };
        _mockMisionPartidaRepositorio.Setup(r => r.ObtenerMisionesMes(1))
            .ReturnsAsync(misionesEsperadas);

        // Act
        var resultado = await _misionPartidaLogica.ObtenerMisionesMes(1);

        // Assert
        Assert.Same(misionesEsperadas, resultado);
        _mockMisionPartidaRepositorio.Verify(r => r.ObtenerMisionesMes(1), Times.Once);
    }

    [Fact]
    public async Task MarcarMisionCompletada_DebeMarcarMisionComoCompletada()
    {
        // Arrange
        var partida = new Partida { Id = 1 };
        var mision = new Mision { Id = 1, Titulo = "Test" };
        var misionPartida = new MisionPartida
        {
            PartidaId = 1,
            MisionId = 1,
            FechaCompletado = null,
            Reclamado = false
        };

        _mockMisionPartidaRepositorio.Setup(r => r.ObtenerUnaMisionDePartida(1, 1))
            .ReturnsAsync(misionPartida);
        _mockMisionPartidaRepositorio.Setup(r => r.Actualizar(It.IsAny<MisionPartida>()))
            .Returns(Task.CompletedTask);

        // Act
        await _misionPartidaLogica.MarcarMisionCompletada(mision, partida);

        // Assert
        _mockMisionPartidaRepositorio.Verify(r => r.ObtenerUnaMisionDePartida(1, 1), Times.Once);
        _mockMisionPartidaRepositorio.Verify(r => r.MarcarCompletada(It.Is<MisionPartida>(mp => 
            mp.FechaCompletado != null && mp.Reclamado == true
        )), Times.Once);
        _mockUnidadDeTrabajo.Verify(u => u.CommitAsync(), Times.Once);
    }
}