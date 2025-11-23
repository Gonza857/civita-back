using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Excepciones;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Logica;
using CivitaBack.Logica.Interfaces;
using CivitaBack.Utils;
using Moq;

namespace CivitaBack.Tests;

public class MapaLogicaTest
{
    private readonly Mock<IPartidaRepositorio> _mockPartidaRepositorio;
    private readonly Mock<IEstructuraMapaRepositorio> _mockEstructuraMapaRepositorio;
    private readonly Mock<IAccesoUsuarios> _mockAccesoUsuarios;
    private readonly Mock<IUnidadDeTrabajo> _mockUow;
    private readonly IMapaLogica _mapaLogica;

    public MapaLogicaTest()
    {
        _mockPartidaRepositorio = new Mock<IPartidaRepositorio>();
        _mockEstructuraMapaRepositorio = new Mock<IEstructuraMapaRepositorio>();
        _mockAccesoUsuarios = new Mock<IAccesoUsuarios>();
        _mockUow = new Mock<IUnidadDeTrabajo>();

        _mapaLogica = new MapaLogica(
            _mockPartidaRepositorio.Object,
            _mockEstructuraMapaRepositorio.Object,
            _mockAccesoUsuarios.Object,
            _mockUow.Object
        );

        _mockUow.Setup(u => u.CommitAsync()).ReturnsAsync(1);
    }

    [Fact]
    public async Task ObtenerMapaAsync_PartidaConJsonMapa_RetornaPartida()
    {
        // Arrange
        const int partidaId = 1;
        var partida = new Partida
        {
            Id = partidaId,
            UsuarioId = 1,
            JsonMapa = "{\"test\": \"mapa\"}"
        };

        _mockPartidaRepositorio.Setup(r => r.ObtenerPartidaConMapaAsync(partidaId))
            .ReturnsAsync(partida);
        _mockAccesoUsuarios.Setup(a => a.ValidarAcceso(1))
            .Verifiable();

        // Act
        var resultado = await _mapaLogica.ObtenerMapaAsync(partidaId);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(partidaId, resultado.Id);
        _mockPartidaRepositorio.Verify(r => r.ObtenerPartidaConMapaAsync(partidaId), Times.Once);
        _mockAccesoUsuarios.Verify(a => a.ValidarAcceso(1), Times.Once);
        _mockPartidaRepositorio.Verify(r => r.ObtenerMapaJsonPorPartidaIdAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task ObtenerMapaAsync_PartidaSinJsonMapa_ReconstruyeMapa()
    {
        // Arrange
        const int partidaId = 1;
        var partida = new Partida
        {
            Id = partidaId,
            UsuarioId = 1,
            JsonMapa = null
        };
        const string mapaReconstruido = "{\"reconstruido\": true}";

        _mockPartidaRepositorio.Setup(r => r.ObtenerPartidaConMapaAsync(partidaId))
            .ReturnsAsync(partida);
        _mockPartidaRepositorio.Setup(r => r.ObtenerMapaJsonPorPartidaIdAsync(partidaId))
            .ReturnsAsync(mapaReconstruido);
        _mockPartidaRepositorio.Setup(r => r.ActualizarMapaAsync(It.IsAny<Partida>()))
            .ReturnsAsync(true);
        _mockAccesoUsuarios.Setup(a => a.ValidarAcceso(1))
            .Verifiable();

        // Act
        var resultado = await _mapaLogica.ObtenerMapaAsync(partidaId);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(mapaReconstruido, resultado.JsonMapa);
        _mockPartidaRepositorio.Verify(r => r.ObtenerMapaJsonPorPartidaIdAsync(partidaId), Times.Once);
        _mockPartidaRepositorio.Verify(r => r.ActualizarMapaAsync(It.IsAny<Partida>()), Times.Once);
        _mockUow.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task ObtenerMapaAsync_PartidaNoExiste_RetornaNull()
    {
        // Arrange
        const int partidaId = 999;

        _mockPartidaRepositorio.Setup(r => r.ObtenerPartidaConMapaAsync(partidaId))
            .ReturnsAsync((Partida?)null);

        // Act
        var resultado = await _mapaLogica.ObtenerMapaAsync(partidaId);

        // Assert
        Assert.Null(resultado);
        _mockAccesoUsuarios.Verify(a => a.ValidarAcceso(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task ActualizarMapaDePartidaAsync_DatosValidosSinEstructuras_ActualizaMapa()
    {
        // Arrange
        const int partidaId = 1;
        const string jsonMapa = "{\"test\": \"mapa\"}";
        var estructuras = new List<EstructuraMapa>();
        var partida = new Partida
        {
            Id = partidaId,
            UsuarioId = 1
        };

        _mockPartidaRepositorio.Setup(r => r.ObtenerPartidaConMapaAsync(partidaId))
            .ReturnsAsync(partida);
        _mockPartidaRepositorio.Setup(r => r.ActualizarMapaAsync(It.IsAny<Partida>()))
            .ReturnsAsync(true);
        _mockAccesoUsuarios.Setup(a => a.ValidarAcceso(1))
            .Verifiable();

        // Act
        await _mapaLogica.ActualizarMapaDePartidaAsync(partidaId, jsonMapa, estructuras);

        // Assert
        _mockPartidaRepositorio.Verify(r => r.ObtenerPartidaConMapaAsync(partidaId), Times.Once);
        _mockPartidaRepositorio.Verify(r => r.ActualizarMapaAsync(It.Is<Partida>(p => p.JsonMapa == jsonMapa)), Times.Once);
        _mockEstructuraMapaRepositorio.Verify(r => r.EliminarPorPartidaIdAsync(It.IsAny<int>()), Times.Never);
        _mockUow.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task ActualizarMapaDePartidaAsync_DatosValidosConEstructuras_ActualizaMapaYEstructuras()
    {
        // Arrange
        const int partidaId = 1;
        const string jsonMapa = "{\"test\": \"mapa\"}";
        var estructuras = new List<EstructuraMapa>
        {
            new EstructuraMapa { EstructuraId = 1, X = 10, Y = 20, Width = 50, Height = 50 }
        };
        var partida = new Partida
        {
            Id = partidaId,
            UsuarioId = 1
        };

        _mockPartidaRepositorio.Setup(r => r.ObtenerPartidaConMapaAsync(partidaId))
            .ReturnsAsync(partida);
        _mockPartidaRepositorio.Setup(r => r.ActualizarMapaAsync(It.IsAny<Partida>()))
            .ReturnsAsync(true);
        _mockEstructuraMapaRepositorio.Setup(r => r.EliminarPorPartidaIdAsync(partidaId))
            .Returns(Task.CompletedTask);
        _mockEstructuraMapaRepositorio.Setup(r => r.AgregarNuevas(It.IsAny<List<EstructuraMapa>>()))
            .Returns(Task.CompletedTask);
        _mockAccesoUsuarios.Setup(a => a.ValidarAcceso(1))
            .Verifiable();

        // Act
        await _mapaLogica.ActualizarMapaDePartidaAsync(partidaId, jsonMapa, estructuras);

        // Assert
        _mockPartidaRepositorio.Verify(r => r.ObtenerPartidaConMapaAsync(partidaId), Times.Once);
        _mockEstructuraMapaRepositorio.Verify(r => r.EliminarPorPartidaIdAsync(partidaId), Times.Once);
        _mockEstructuraMapaRepositorio.Verify(r => r.AgregarNuevas(It.IsAny<List<EstructuraMapa>>()), Times.Once);
        _mockUow.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task ActualizarMapaDePartidaAsync_JsonMapaNull_LanzaExcepcion()
    {
        // Arrange
        const int partidaId = 1;
        var estructuras = new List<EstructuraMapa>();

        // Act & Assert
        await Assert.ThrowsAsync<PartidaExcepcion>(() => 
            _mapaLogica.ActualizarMapaDePartidaAsync(partidaId, null!, estructuras));
    }

    [Fact]
    public async Task ActualizarMapaDePartidaAsync_PartidaIdInvalido_LanzaExcepcion()
    {
        // Arrange
        const int partidaIdInvalido = 0;
        const string jsonMapa = "{\"test\": \"mapa\"}";
        var estructuras = new List<EstructuraMapa>();

        // Act & Assert
        await Assert.ThrowsAsync<PartidaExcepcion>(() => 
            _mapaLogica.ActualizarMapaDePartidaAsync(partidaIdInvalido, jsonMapa, estructuras));
    }

    [Fact]
    public async Task ActualizarMapaDePartidaAsync_EstructurasNull_LanzaExcepcion()
    {
        // Arrange
        const int partidaId = 1;
        const string jsonMapa = "{\"test\": \"mapa\"}";

        // Act & Assert
        await Assert.ThrowsAsync<PartidaExcepcion>(() => 
            _mapaLogica.ActualizarMapaDePartidaAsync(partidaId, jsonMapa, null!));
    }

    [Fact]
    public async Task ActualizarMapaDePartidaAsync_PartidaNoExiste_LanzaExcepcion()
    {
        // Arrange
        const int partidaId = 999;
        const string jsonMapa = "{\"test\": \"mapa\"}";
        var estructuras = new List<EstructuraMapa>();

        _mockPartidaRepositorio.Setup(r => r.ObtenerPartidaConMapaAsync(partidaId))
            .ReturnsAsync((Partida?)null);

        // Act & Assert
        await Assert.ThrowsAsync<PartidaExcepcion>(() => 
            _mapaLogica.ActualizarMapaDePartidaAsync(partidaId, jsonMapa, estructuras));
    }

    [Fact]
    public async Task ActualizarMapaDePartidaAsync_AccesoDenegado_LanzaExcepcion()
    {
        // Arrange
        const int partidaId = 1;
        const string jsonMapa = "{\"test\": \"mapa\"}";
        var estructuras = new List<EstructuraMapa>();
        var partida = new Partida
        {
            Id = partidaId,
            UsuarioId = 1
        };

        _mockPartidaRepositorio.Setup(r => r.ObtenerPartidaConMapaAsync(partidaId))
            .ReturnsAsync(partida);
        _mockAccesoUsuarios.Setup(a => a.ValidarAcceso(1))
            .Throws(new AccesoDenegadoExcepcion("Acceso denegado"));

        // Act & Assert
        await Assert.ThrowsAsync<AccesoDenegadoExcepcion>(() => 
            _mapaLogica.ActualizarMapaDePartidaAsync(partidaId, jsonMapa, estructuras));
    }
}

