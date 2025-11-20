using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Excepciones;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Logica;
using CivitaBack.Logica.Interfaces;
using CivitaBack.Utils;
using Moq;

namespace CivitaBack.Tests;

public class TipoTipLogicaTest
{
    private readonly Mock<ITipoTipRepositorio> _mockTipoTipRepositorio;
    private readonly Mock<IUnidadDeTrabajo> _mockUow;
    private readonly Mock<IAccesoUsuarios> _mockAccesoUsuarios;
    private readonly ITipoTipLogica _tipoTipLogica;

    public TipoTipLogicaTest()
    {
        _mockTipoTipRepositorio = new Mock<ITipoTipRepositorio>();
        _mockUow = new Mock<IUnidadDeTrabajo>();
        _mockAccesoUsuarios = new Mock<IAccesoUsuarios>();

        _tipoTipLogica = new TipoTipLogica(
            _mockTipoTipRepositorio.Object,
            _mockUow.Object,
            _mockAccesoUsuarios.Object
        );

        _mockUow.Setup(u => u.CommitAsync()).ReturnsAsync(1);
    }

    [Fact]
    public async Task Listado_HayTiposTip_RetornaLista()
    {
        // Arrange
        var tiposMock = new List<TipoTip>
        {
            new TipoTip { Id = 1, Descripcion = "Tipo 1" },
            new TipoTip { Id = 2, Descripcion = "Tipo 2" }
        };

        _mockTipoTipRepositorio.Setup(r => r.ObtenerTodos())
            .ReturnsAsync(tiposMock);

        // Act
        var resultado = await _tipoTipLogica.Listado();

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(2, resultado.Count);
        _mockTipoTipRepositorio.Verify(r => r.ObtenerTodos(), Times.Once);
    }

    [Fact]
    public async Task ObtenerPorId_TipoTipExiste_RetornaTipoTip()
    {
        // Arrange
        const int id = 1;
        var tipoTipMock = new TipoTip
        {
            Id = id,
            Descripcion = "Tipo Test"
        };

        _mockTipoTipRepositorio.Setup(r => r.ObtenerPorId(id))
            .ReturnsAsync(tipoTipMock);

        // Act
        var resultado = await _tipoTipLogica.ObtenerPorId(id);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(id, resultado.Id);
        _mockTipoTipRepositorio.Verify(r => r.ObtenerPorId(id), Times.Once);
    }

    [Fact]
    public async Task ObtenerPorId_TipoTipNoExiste_RetornaNull()
    {
        // Arrange
        const int id = 999;

        _mockTipoTipRepositorio.Setup(r => r.ObtenerPorId(id))
            .ReturnsAsync((TipoTip?)null);

        // Act
        var resultado = await _tipoTipLogica.ObtenerPorId(id);

        // Assert
        Assert.Null(resultado);
    }

    [Fact]
    public async Task Guardar_TipoTipValido_GuardaTipoTip()
    {
        // Arrange
        var tipoTipNuevo = new TipoTip
        {
            Descripcion = "Nuevo Tipo"
        };

        _mockAccesoUsuarios.Setup(a => a.EsDios()).Returns(true);
        _mockTipoTipRepositorio.Setup(r => r.Agregar(It.IsAny<TipoTip>()))
            .Returns(Task.CompletedTask);

        // Act
        await _tipoTipLogica.Guardar(tipoTipNuevo);

        // Assert
        _mockTipoTipRepositorio.Verify(r => r.Agregar(It.IsAny<TipoTip>()), Times.Once);
        _mockUow.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task Guardar_NoEsAdmin_LanzaExcepcion()
    {
        // Arrange
        var tipoTipNuevo = new TipoTip { Descripcion = "Test" };

        _mockAccesoUsuarios.Setup(a => a.EsDios()).Returns(false);

        // Act & Assert
        await Assert.ThrowsAsync<AccesoDenegadoExcepcion>(() => _tipoTipLogica.Guardar(tipoTipNuevo));
        _mockTipoTipRepositorio.Verify(r => r.Agregar(It.IsAny<TipoTip>()), Times.Never);
    }

    [Fact]
    public async Task Actualizar_TipoTipValido_ActualizaTipoTip()
    {
        // Arrange
        const int id = 1;
        var tipoTipExistente = new TipoTip
        {
            Id = id,
            Descripcion = "Tipo Original"
        };
        var tipoTipActualizado = new TipoTip
        {
            Descripcion = "Tipo Actualizado"
        };

        _mockAccesoUsuarios.Setup(a => a.EsDios()).Returns(true);
        _mockTipoTipRepositorio.Setup(r => r.ObtenerPorId(id))
            .ReturnsAsync(tipoTipExistente);
        _mockTipoTipRepositorio.Setup(r => r.Actualizar(It.IsAny<TipoTip>()))
            .Returns(Task.CompletedTask);

        // Act
        await _tipoTipLogica.Actualizar(tipoTipActualizado, id);

        // Assert
        _mockTipoTipRepositorio.Verify(r => r.ObtenerPorId(id), Times.Once);
        _mockTipoTipRepositorio.Verify(r => r.Actualizar(It.IsAny<TipoTip>()), Times.Once);
        _mockUow.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task Actualizar_TipoTipNoExiste_LanzaExcepcion()
    {
        // Arrange
        var tipoTipActualizado = new TipoTip { Descripcion = "Test" };

        _mockAccesoUsuarios.Setup(a => a.EsDios()).Returns(true);
        _mockTipoTipRepositorio.Setup(r => r.ObtenerPorId(1))
            .ReturnsAsync((TipoTip?)null);

        // Act & Assert
        await Assert.ThrowsAsync<TipoTipException>(() => _tipoTipLogica.Actualizar(tipoTipActualizado, 1));
    }

    [Fact]
    public async Task Actualizar_NoEsAdmin_LanzaExcepcion()
    {
        // Arrange
        var tipoTipActualizado = new TipoTip { Descripcion = "Test" };
        var tipoTipExistente = new TipoTip { Id = 1 };

        _mockAccesoUsuarios.Setup(a => a.EsDios()).Returns(false);
        _mockTipoTipRepositorio.Setup(r => r.ObtenerPorId(1))
            .ReturnsAsync(tipoTipExistente);

        // Act & Assert
        await Assert.ThrowsAsync<AccesoDenegadoExcepcion>(() => _tipoTipLogica.Actualizar(tipoTipActualizado, 1));
    }

    [Fact]
    public async Task Eliminar_TipoTipExiste_EliminaTipoTip()
    {
        // Arrange
        const int id = 1;
        var tipoTipExistente = new TipoTip { Id = id };

        _mockAccesoUsuarios.Setup(a => a.EsDios()).Returns(true);
        _mockTipoTipRepositorio.Setup(r => r.ObtenerPorId(id))
            .ReturnsAsync(tipoTipExistente);
        _mockTipoTipRepositorio.Setup(r => r.Eliminar(id))
            .Returns(Task.CompletedTask);

        // Act
        await _tipoTipLogica.Eliminar(id);

        // Assert
        _mockTipoTipRepositorio.Verify(r => r.ObtenerPorId(id), Times.Once);
        _mockTipoTipRepositorio.Verify(r => r.Eliminar(id), Times.Once);
        _mockUow.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task Eliminar_TipoTipNoExiste_LanzaExcepcion()
    {
        // Arrange
        const int id = 999;

        _mockAccesoUsuarios.Setup(a => a.EsDios()).Returns(true);
        _mockTipoTipRepositorio.Setup(r => r.ObtenerPorId(id))
            .ReturnsAsync((TipoTip?)null);

        // Act & Assert
        await Assert.ThrowsAsync<TipoTipException>(() => _tipoTipLogica.Eliminar(id));
        _mockTipoTipRepositorio.Verify(r => r.Eliminar(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task Eliminar_NoEsAdmin_LanzaExcepcion()
    {
        // Arrange
        const int id = 1;
        var tipoTipExistente = new TipoTip { Id = id };

        _mockAccesoUsuarios.Setup(a => a.EsDios()).Returns(false);
        _mockTipoTipRepositorio.Setup(r => r.ObtenerPorId(id))
            .ReturnsAsync(tipoTipExistente);

        // Act & Assert
        await Assert.ThrowsAsync<AccesoDenegadoExcepcion>(() => _tipoTipLogica.Eliminar(id));
        _mockTipoTipRepositorio.Verify(r => r.Eliminar(It.IsAny<int>()), Times.Never);
    }
}



