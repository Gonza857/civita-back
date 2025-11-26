using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Excepciones;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Logica;
using CivitaBack.Logica.Interfaces;
using CivitaBack.Utils;
using Moq;

namespace CivitaBack.Tests;

public class CompraEstructurasLogicaTest
{
    private readonly Mock<IPartidaRepositorio> _mockPartidaRepositorio;
    private readonly Mock<IEstructuraRepositorio> _mockEstructuraRepositorio;
    private readonly Mock<IAccesoUsuarios> _mockAccesoUsuarios;
    private readonly Mock<IActualizarRecursosLogica> _mockActualizarRecursosLogica;
    private readonly Mock<IUnidadDeTrabajo> _mockUow;
    private readonly ICompraEstructurasLogica _compraEstructurasLogica;

    public CompraEstructurasLogicaTest()
    {
        _mockPartidaRepositorio = new Mock<IPartidaRepositorio>();
        _mockEstructuraRepositorio = new Mock<IEstructuraRepositorio>();
        _mockAccesoUsuarios = new Mock<IAccesoUsuarios>();
        _mockActualizarRecursosLogica = new Mock<IActualizarRecursosLogica>();
        _mockUow = new Mock<IUnidadDeTrabajo>();

        _compraEstructurasLogica = new CompraEstructurasLogica(
            _mockPartidaRepositorio.Object,
            _mockEstructuraRepositorio.Object,
            _mockAccesoUsuarios.Object,
            _mockActualizarRecursosLogica.Object,
            _mockUow.Object
        );

        _mockUow.Setup(u => u.CommitAsync()).ReturnsAsync(1);
    }

    [Fact]
    public async Task ComprarEstructuraAsync_DineroSuficiente_CompraEstructura()
    {
        // // Arrange
        // const int partidaId = 1;
        // const int estructuraId = 1;
        // var partida = new Partida
        // {
        //     Id = partidaId,
        //     UsuarioId = 1,
        //     Recursos = new Recurso
        //     {
        //         EcoCoins = 500
        //     }
        // };
        // var estructura = new Estructura
        // {
        //     Id = estructuraId,
        //     CostoDinero = 100
        // };
        //
        // _mockPartidaRepositorio.Setup(r => r.ObtenerPorId(partidaId))
        //     .ReturnsAsync(partida);
        // _mockEstructuraRepositorio.Setup(r => r.ObtenerPorId(estructuraId))
        //     .ReturnsAsync(estructura);
        // _mockPartidaRepositorio.Setup(r => r.Actualizar(It.IsAny<Partida>()))
        //     .Returns(Task.CompletedTask);
        // _mockAccesoUsuarios.Setup(a => a.ValidarAcceso(1))
        //     .Verifiable();
        //
        // // Act
        // var resultado = await _compraEstructurasLogica.ComprarEstructuraAsync(partidaId, estructuraId);
        //
        // // Assert
        // Assert.True(resultado >= 0);
        // _mockActualizarRecursosLogica.Verify(a => a.ActualizarRecursosAsync(
        //     partida,
        //     0,
        //     0,
        //     -100, // Cambio negativo de EcoCoins
        //     0
        // ), Times.Once);
        // _mockPartidaRepositorio.Verify(r => r.Actualizar(partida), Times.Once);
        // _mockUow.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task ComprarEstructuraAsync_DineroInsuficiente_LanzaExcepcion()
    {
        // Arrange
        const int partidaId = 1;
        const int estructuraId = 1;
        var partida = new Partida
        {
            Id = partidaId,
            UsuarioId = 1,
            Recursos = new Recurso
            {
                EcoCoins = 50
            }
        };
        var estructura = new Estructura
        {
            Id = estructuraId,
            CostoDinero = 100
        };

        _mockPartidaRepositorio.Setup(r => r.ObtenerPorId(partidaId))
            .ReturnsAsync(partida);
        _mockEstructuraRepositorio.Setup(r => r.ObtenerPorId(estructuraId))
            .ReturnsAsync(estructura);
        _mockAccesoUsuarios.Setup(a => a.ValidarAcceso(1))
            .Verifiable();

        // Act & Assert
        await Assert.ThrowsAsync<PartidaExcepcion>(() => 
            _compraEstructurasLogica.ComprarEstructuraAsync(partidaId, estructuraId));
        _mockPartidaRepositorio.Verify(r => r.Actualizar(It.IsAny<Partida>()), Times.Never);
    }

    [Fact]
    public async Task ComprarEstructuraAsync_PartidaNoExiste_LanzaExcepcion()
    {
        // Arrange
        const int partidaId = 999;
        const int estructuraId = 1;

        _mockPartidaRepositorio.Setup(r => r.ObtenerPorId(partidaId))
            .ReturnsAsync((Partida?)null);

        // Act & Assert
        await Assert.ThrowsAsync<PartidaExcepcion>(() => 
            _compraEstructurasLogica.ComprarEstructuraAsync(partidaId, estructuraId));
    }

    [Fact]
    public async Task ComprarEstructuraAsync_PartidaSinRecursos_LanzaExcepcion()
    {
        // Arrange
        const int partidaId = 1;
        const int estructuraId = 1;
        var partida = new Partida
        {
            Id = partidaId,
            UsuarioId = 1,
            Recursos = null
        };

        _mockPartidaRepositorio.Setup(r => r.ObtenerPorId(partidaId))
            .ReturnsAsync(partida);

        // Act & Assert
        await Assert.ThrowsAsync<PartidaExcepcion>(() => 
            _compraEstructurasLogica.ComprarEstructuraAsync(partidaId, estructuraId));
    }

    [Fact]
    public async Task ComprarEstructuraAsync_EstructuraNoExiste_LanzaExcepcion()
    {
        // Arrange
        const int partidaId = 1;
        const int estructuraId = 999;
        var partida = new Partida
        {
            Id = partidaId,
            UsuarioId = 1,
            Recursos = new Recurso { EcoCoins = 500 }
        };

        _mockPartidaRepositorio.Setup(r => r.ObtenerPorId(partidaId))
            .ReturnsAsync(partida);
        _mockEstructuraRepositorio.Setup(r => r.ObtenerPorId(estructuraId))
            .ReturnsAsync((Estructura?)null);

        // Act & Assert
        await Assert.ThrowsAsync<PartidaExcepcion>(() => 
            _compraEstructurasLogica.ComprarEstructuraAsync(partidaId, estructuraId));
    }
    
}


