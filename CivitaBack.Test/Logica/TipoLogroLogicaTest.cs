using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Excepciones;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Logica;
using CivitaBack.Logica.Interfaces;
using CivitaBack.Utils;
using Moq;

namespace CivitaBack.Tests;

public class TipoLogroLogicaTest
{
    private readonly Mock<ITipoLogroRepositorio> _mockTipoLogroRepositorio;
    private readonly Mock<IUnidadDeTrabajo> _mockUow;
    private readonly Mock<IAccesoUsuarios> _mockAccesoUsuarios;
    private readonly ITipoLogroLogica _tipoLogroLogica;

    public TipoLogroLogicaTest()
    {
        _mockTipoLogroRepositorio = new Mock<ITipoLogroRepositorio>();
        _mockUow = new Mock<IUnidadDeTrabajo>();
        _mockAccesoUsuarios = new Mock<IAccesoUsuarios>();

        _tipoLogroLogica = new TipoLogroLogica(
            _mockTipoLogroRepositorio.Object,
            _mockUow.Object,
            _mockAccesoUsuarios.Object
        );

        _mockUow.Setup(u => u.CommitAsync()).ReturnsAsync(1);
    }

    [Fact]
    public async Task ObtenerPorId_TipoLogroExiste_RetornaTipoLogro()
    {
        // Arrange
        const int id = 1;
        var tipoLogroMock = new TipoLogro
        {
            Id = id,
            Nombre = "Tipo Test"
        };

        _mockTipoLogroRepositorio.Setup(r => r.ObtenerPorId(id))
            .ReturnsAsync(tipoLogroMock);

        // Act
        var resultado = await _tipoLogroLogica.ObtenerPorId(id);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(id, resultado.Id);
        _mockTipoLogroRepositorio.Verify(r => r.ObtenerPorId(id), Times.Once);
    }

    [Fact]
    public async Task ObtenerPorId_TipoLogroNoExiste_LanzaExcepcion()
    {
        // Arrange
        const int id = 999;

        _mockTipoLogroRepositorio.Setup(r => r.ObtenerPorId(id))
            .ReturnsAsync((TipoLogro?)null);

        // Act & Assert
        await Assert.ThrowsAsync<TipoLogroException>(() => _tipoLogroLogica.ObtenerPorId(id));
    }

    [Fact]
    public async Task ObtenerTiposLogro_HayTiposLogro_RetornaLista()
    {
        // Arrange
        var tiposMock = new List<TipoLogro>
        {
            new TipoLogro { Id = 1, Nombre = "Tipo 1" },
            new TipoLogro { Id = 2, Nombre = "Tipo 2" }
        };

        _mockTipoLogroRepositorio.Setup(r => r.ObtenerTodos())
            .ReturnsAsync(tiposMock);

        // Act
        var resultado = await _tipoLogroLogica.ObtenerTiposLogro();

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(2, resultado.Count);
        _mockTipoLogroRepositorio.Verify(r => r.ObtenerTodos(), Times.Once);
    }

    [Fact]
    public async Task Guardar_TipoLogroValido_GuardaTipoLogro()
    {
        // Arrange
        var tipoLogroNuevo = new TipoLogro
        {
            Nombre = "Nuevo Tipo"
        };

        _mockAccesoUsuarios.Setup(a => a.EsDios()).Returns(true);
        _mockTipoLogroRepositorio.Setup(r => r.Agregar(It.IsAny<TipoLogro>()))
            .Returns(Task.CompletedTask);

        // Act
        var resultado = await _tipoLogroLogica.Guardar(tipoLogroNuevo);

        // Assert
        Assert.NotNull(resultado);
        _mockTipoLogroRepositorio.Verify(r => r.Agregar(It.IsAny<TipoLogro>()), Times.Once);
        _mockUow.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task Guardar_NoEsAdmin_LanzaExcepcion()
    {
        // Arrange
        var tipoLogroNuevo = new TipoLogro { Nombre = "Test" };

        _mockAccesoUsuarios.Setup(a => a.EsDios()).Returns(false);

        // Act & Assert
        await Assert.ThrowsAsync<AccesoDenegadoExcepcion>(() => _tipoLogroLogica.Guardar(tipoLogroNuevo));
        _mockTipoLogroRepositorio.Verify(r => r.Agregar(It.IsAny<TipoLogro>()), Times.Never);
    }

    [Fact]
    public async Task Guardar_TipoLogroNull_LanzaExcepcion()
    {
        // Arrange
        _mockAccesoUsuarios.Setup(a => a.EsDios()).Returns(true);

        // Act & Assert
        await Assert.ThrowsAsync<TipoLogroException>(() => _tipoLogroLogica.Guardar(null!));
    }

    [Fact]
    public async Task Actualizar_TipoLogroValido_ActualizaTipoLogro()
    {
        // Arrange
        const int id = 1;
        var tipoLogroExistente = new TipoLogro
        {
            Id = id,
            Nombre = "Tipo Original"
        };
        var tipoLogroActualizado = new TipoLogro
        {
            Nombre = "Tipo Actualizado"
        };

        _mockAccesoUsuarios.Setup(a => a.EsDios()).Returns(true);
        _mockTipoLogroRepositorio.Setup(r => r.ObtenerPorId(id))
            .ReturnsAsync(tipoLogroExistente);
        _mockTipoLogroRepositorio.Setup(r => r.Actualizar(It.IsAny<TipoLogro>()))
            .Returns(Task.CompletedTask);

        // Act
        await _tipoLogroLogica.Actualizar(tipoLogroActualizado, id);

        // Assert
        _mockTipoLogroRepositorio.Verify(r => r.ObtenerPorId(id), Times.Once);
        _mockTipoLogroRepositorio.Verify(r => r.Actualizar(It.IsAny<TipoLogro>()), Times.Once);
        _mockUow.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task Actualizar_TipoLogroNoExiste_LanzaExcepcion()
    {
        // Arrange
        var tipoLogroActualizado = new TipoLogro { Nombre = "Test" };

        _mockAccesoUsuarios.Setup(a => a.EsDios()).Returns(true);
        _mockTipoLogroRepositorio.Setup(r => r.ObtenerPorId(1))
            .ReturnsAsync((TipoLogro?)null);

        // Act & Assert
        await Assert.ThrowsAsync<TipoLogroException>(() => _tipoLogroLogica.Actualizar(tipoLogroActualizado, 1));
    }

    [Fact]
    public async Task Actualizar_NoEsAdmin_LanzaExcepcion()
    {
        // Arrange
        var tipoLogroActualizado = new TipoLogro { Nombre = "Test" };
        var tipoLogroExistente = new TipoLogro { Id = 1 };

        _mockAccesoUsuarios.Setup(a => a.EsDios()).Returns(false);
        _mockTipoLogroRepositorio.Setup(r => r.ObtenerPorId(1))
            .ReturnsAsync(tipoLogroExistente);

        // Act & Assert
        await Assert.ThrowsAsync<AccesoDenegadoExcepcion>(() => _tipoLogroLogica.Actualizar(tipoLogroActualizado, 1));
    }

    [Fact]
    public async Task Eliminar_IdValido_EliminaTipoLogro()
    {
        // Arrange
        const int id = 1;

        _mockAccesoUsuarios.Setup(a => a.EsDios()).Returns(true);
        _mockTipoLogroRepositorio.Setup(r => r.Eliminar(id))
            .Returns(Task.CompletedTask);

        // Act
        await _tipoLogroLogica.Eliminar(id);

        // Assert
        _mockTipoLogroRepositorio.Verify(r => r.Eliminar(id), Times.Once);
        _mockUow.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task Eliminar_IdInvalido_LanzaExcepcion()
    {
        // Arrange
        const int idInvalido = 0;

        _mockAccesoUsuarios.Setup(a => a.EsDios()).Returns(true);

        // Act & Assert
        await Assert.ThrowsAsync<TipoLogroException>(() => _tipoLogroLogica.Eliminar(idInvalido));
        _mockTipoLogroRepositorio.Verify(r => r.Eliminar(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task Eliminar_NoEsAdmin_LanzaExcepcion()
    {
        // Arrange
        const int id = 1;

        _mockAccesoUsuarios.Setup(a => a.EsDios()).Returns(false);

        // Act & Assert
        await Assert.ThrowsAsync<AccesoDenegadoExcepcion>(() => _tipoLogroLogica.Eliminar(id));
        _mockTipoLogroRepositorio.Verify(r => r.Eliminar(It.IsAny<int>()), Times.Never);
    }
}



