using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Excepciones;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Logica;
using CivitaBack.Logica.Interfaces;
using CivitaBack.Utils;
using Moq;

namespace CivitaBack.Tests;

public class RecursoLogicaTest
{
    private readonly Mock<IRecursoRepositorio> _mockRecursoRepositorio;
    private readonly Mock<IUnidadDeTrabajo> _mockUow;
    private readonly Mock<IAccesoUsuarios> _mockAccesoUsuarios;
    private readonly IRecursoLogica _recursoLogica;

    public RecursoLogicaTest()
    {
        _mockRecursoRepositorio = new Mock<IRecursoRepositorio>();
        _mockUow = new Mock<IUnidadDeTrabajo>();
        _mockAccesoUsuarios = new Mock<IAccesoUsuarios>();

        _recursoLogica = new RecursoLogica(
            _mockRecursoRepositorio.Object,
            _mockUow.Object,
            _mockAccesoUsuarios.Object
        );

        _mockUow.Setup(u => u.CommitAsync()).ReturnsAsync(1);
    }

    [Fact]
    public async Task ConfigurarInicial_PartidaValida_ConfiguraRecursos()
    {
        // Arrange
        var partida = new Partida { Id = 1, UsuarioId = 1 };
        var recurso = new Recurso
        {
            Id = 1,
            PartidaId = 1,
            EcoCoins = 0,
            Felicidad = 0,
            Energia = 0,
            Contaminacion = 0
        };

        _mockRecursoRepositorio.Setup(r => r.ObtenerPorId(1))
            .ReturnsAsync(recurso);
        _mockRecursoRepositorio.Setup(r => r.Actualizar(It.IsAny<Recurso>()))
            .Returns(Task.CompletedTask);

        // Act
        await _recursoLogica.ConfigurarInicial(partida);

        // Assert
        _mockRecursoRepositorio.Verify(r => r.ObtenerPorId(1), Times.Once);
        _mockRecursoRepositorio.Verify(r => r.Actualizar(It.Is<Recurso>(rec =>
            rec.EcoCoins == 450 &&
            rec.Felicidad == 40 &&
            rec.Energia == 30 &&
            rec.Contaminacion == 60
        )), Times.Once);
        _mockUow.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task ConfigurarInicial_PartidaNull_LanzaExcepcion()
    {
        // Act & Assert
        await Assert.ThrowsAsync<PartidaExcepcion>(() => _recursoLogica.ConfigurarInicial(null!));
        _mockRecursoRepositorio.Verify(r => r.ObtenerPorId(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task ConfigurarInicial_RecursoNoExiste_LanzaExcepcion()
    {
        // Arrange
        var partida = new Partida { Id = 1 };

        _mockRecursoRepositorio.Setup(r => r.ObtenerPorId(1))
            .ReturnsAsync((Recurso?)null);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _recursoLogica.ConfigurarInicial(partida));
    }

    [Fact]
    public async Task ObtenerRecursos_PartidaValida_RetornaRecursos()
    {
        // Arrange
        const int idPartida = 1;
        const int idUsuario = 1;
        var partida = new Partida { Id = idPartida, UsuarioId = idUsuario };
        var recurso = new Recurso
        {
            Id = 1,
            PartidaId = idPartida,
            Partida = partida,
            EcoCoins = 100,
            Felicidad = 50,
            Energia = 75,
            Contaminacion = 30
        };

        _mockRecursoRepositorio.Setup(r => r.ObtenerRecursosPartida(idPartida))
            .ReturnsAsync(recurso);
        _mockAccesoUsuarios.Setup(a => a.ValidarAcceso(idUsuario))
            .Verifiable();

        // Act
        var resultado = await _recursoLogica.ObtenerRecursos(idPartida);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(idPartida, resultado.PartidaId);
        _mockRecursoRepositorio.Verify(r => r.ObtenerRecursosPartida(idPartida), Times.Once);
        // _mockAccesoUsuarios.Verify(a => a.ValidarAcceso(idUsuario), Times.Once);
    }

    [Fact]
    public async Task ObtenerRecursos_RecursoNoExiste_LanzaExcepcion()
    {
        // Arrange
        const int idPartida = 999;

        _mockRecursoRepositorio.Setup(r => r.ObtenerRecursosPartida(idPartida))
            .ReturnsAsync((Recurso?)null);

        // Act & Assert
        await Assert.ThrowsAsync<PartidaExcepcion>(() => _recursoLogica.ObtenerRecursos(idPartida));
    }

    // [Fact]
    // public async Task ObtenerRecursos_AccesoDenegado_LanzaExcepcion()
    // {
    //     // Arrange
    //     const int idPartida = 1;
    //     const int idUsuario = 1;
    //     var partida = new Partida { Id = idPartida, UsuarioId = idUsuario };
    //     var recurso = new Recurso
    //     {
    //         Id = 1,
    //         PartidaId = idPartida,
    //         Partida = partida
    //     };
    //
    //     _mockRecursoRepositorio.Setup(r => r.ObtenerRecursosPartida(idPartida))
    //         .ReturnsAsync(recurso);
    //     _mockAccesoUsuarios.Setup(a => a.ValidarAcceso(idUsuario))
    //         .Throws(new AccesoDenegadoExcepcion("Acceso denegado"));
    //
    //     // Act & Assert
    //     await Assert.ThrowsAsync<AccesoDenegadoExcepcion>(() => _recursoLogica.ObtenerRecursos(idPartida));
    // }

    [Fact]
    public async Task ModificarEnergia_CantidadPositiva_AumentaEnergia()
    {
        // Arrange
        const int idPartida = 1;
        const int cantidad = 20;
        var recurso = new Recurso
        {
            Id = 1,
            PartidaId = idPartida,
            Energia = 50
        };

        _mockRecursoRepositorio.Setup(r => r.ObtenerRecursosPartida(idPartida))
            .ReturnsAsync(recurso);
        _mockRecursoRepositorio.Setup(r => r.Actualizar(It.IsAny<Recurso>()))
            .Returns(Task.CompletedTask);

        // Act
        await _recursoLogica.ModificarEnergia(idPartida, cantidad);

        // Assert
        _mockRecursoRepositorio.Verify(r => r.ObtenerRecursosPartida(idPartida), Times.Once);
        _mockRecursoRepositorio.Verify(r => r.Actualizar(It.Is<Recurso>(rec => rec.Energia == 70)), Times.Once);
        _mockUow.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task ModificarEnergia_CantidadNegativa_DisminuyeEnergia()
    {
        // Arrange
        const int idPartida = 1;
        const int cantidad = -30;
        var recurso = new Recurso
        {
            Id = 1,
            PartidaId = idPartida,
            Energia = 50
        };

        _mockRecursoRepositorio.Setup(r => r.ObtenerRecursosPartida(idPartida))
            .ReturnsAsync(recurso);
        _mockRecursoRepositorio.Setup(r => r.Actualizar(It.IsAny<Recurso>()))
            .Returns(Task.CompletedTask);

        // Act
        await _recursoLogica.ModificarEnergia(idPartida, cantidad);

        // Assert
        _mockRecursoRepositorio.Verify(r => r.Actualizar(It.Is<Recurso>(rec => rec.Energia == 20)), Times.Once);
    }

    [Fact]
    public async Task ModificarEnergia_EnergiaSupera100_LimitaA100()
    {
        // Arrange
        const int idPartida = 1;
        const int cantidad = 60;
        var recurso = new Recurso
        {
            Id = 1,
            PartidaId = idPartida,
            Energia = 50
        };

        _mockRecursoRepositorio.Setup(r => r.ObtenerRecursosPartida(idPartida))
            .ReturnsAsync(recurso);
        _mockRecursoRepositorio.Setup(r => r.Actualizar(It.IsAny<Recurso>()))
            .Returns(Task.CompletedTask);

        // Act
        await _recursoLogica.ModificarEnergia(idPartida, cantidad);

        // Assert
        _mockRecursoRepositorio.Verify(r => r.Actualizar(It.Is<Recurso>(rec => rec.Energia == 100)), Times.Once);
    }

    [Fact]
    public async Task ModificarEnergia_EnergiaBajaDe0_LimitaA0()
    {
        // Arrange
        const int idPartida = 1;
        const int cantidad = -60;
        var recurso = new Recurso
        {
            Id = 1,
            PartidaId = idPartida,
            Energia = 50
        };

        _mockRecursoRepositorio.Setup(r => r.ObtenerRecursosPartida(idPartida))
            .ReturnsAsync(recurso);
        _mockRecursoRepositorio.Setup(r => r.Actualizar(It.IsAny<Recurso>()))
            .Returns(Task.CompletedTask);

        // Act
        await _recursoLogica.ModificarEnergia(idPartida, cantidad);

        // Assert
        _mockRecursoRepositorio.Verify(r => r.Actualizar(It.Is<Recurso>(rec => rec.Energia == 0)), Times.Once);
    }

    [Fact]
    public async Task ModificarEnergia_RecursoNoExiste_LanzaExcepcion()
    {
        // Arrange
        const int idPartida = 999;

        _mockRecursoRepositorio.Setup(r => r.ObtenerRecursosPartida(idPartida))
            .ReturnsAsync((Recurso?)null);

        // Act & Assert
        await Assert.ThrowsAsync<PartidaExcepcion>(() => _recursoLogica.ModificarEnergia(idPartida, 10));
    }
}



