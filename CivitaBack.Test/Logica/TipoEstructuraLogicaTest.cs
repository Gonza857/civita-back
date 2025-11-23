using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Excepciones;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Logica;
using CivitaBack.Logica.Interfaces;
using CivitaBack.Utils;
using Moq;

namespace CivitaBack.Tests;

public class TipoEstructuraLogicaTest
{
    private readonly Mock<ITipoEstructuraRepositorio> _mockTipoEstructuraRepositorio;
    private readonly Mock<IUnidadDeTrabajo> _mockUow;
    private readonly Mock<IAccesoUsuarios> _mockAccesoUsuarios;
    private readonly ITipoEstructuraLogica _tipoEstructuraLogica;

    public TipoEstructuraLogicaTest()
    {
        _mockTipoEstructuraRepositorio = new Mock<ITipoEstructuraRepositorio>();
        _mockUow = new Mock<IUnidadDeTrabajo>();
        _mockAccesoUsuarios = new Mock<IAccesoUsuarios>();

        _tipoEstructuraLogica = new TipoEstructuraLogica(
            _mockTipoEstructuraRepositorio.Object,
            _mockUow.Object,
            _mockAccesoUsuarios.Object
        );

        _mockUow.Setup(u => u.CommitAsync()).ReturnsAsync(1);
    }

    [Fact]
    public async Task ObtenerPorId_TipoEstructuraExiste_RetornaTipoEstructura()
    {
        // Arrange
        const int id = 1;
        var tipoEstructuraMock = new TipoEstructura
        {
            Id = id,
            Nombre = "Tipo Test"
        };

        _mockTipoEstructuraRepositorio.Setup(r => r.ObtenerPorId(id))
            .ReturnsAsync(tipoEstructuraMock);

        // Act
        var resultado = await _tipoEstructuraLogica.ObtenerPorId(id);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(id, resultado.Id);
        _mockTipoEstructuraRepositorio.Verify(r => r.ObtenerPorId(id), Times.Once);
    }

    [Fact]
    public async Task ObtenerPorId_TipoEstructuraNoExiste_LanzaExcepcion()
    {
        // Arrange
        const int id = 999;

        _mockTipoEstructuraRepositorio.Setup(r => r.ObtenerPorId(id))
            .ReturnsAsync((TipoEstructura?)null);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _tipoEstructuraLogica.ObtenerPorId(id));
    }

    [Fact]
    public async Task Listado_HayTiposEstructura_RetornaLista()
    {
        // Arrange
        var tiposMock = new List<TipoEstructura>
        {
            new TipoEstructura { Id = 1, Nombre = "Tipo 1" },
            new TipoEstructura { Id = 2, Nombre = "Tipo 2" }
        };

        _mockTipoEstructuraRepositorio.Setup(r => r.ObtenerTodos())
            .ReturnsAsync(tiposMock);

        // Act
        var resultado = await _tipoEstructuraLogica.Listado();

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(2, resultado.Count);
        _mockTipoEstructuraRepositorio.Verify(r => r.ObtenerTodos(), Times.Once);
    }

    [Fact]
    public async Task Crear_TipoEstructuraValido_CreaTipoEstructura()
    {
        // Arrange
        var tipoEstructuraNuevo = new TipoEstructura
        {
            Nombre = "Nuevo Tipo",
            Capacidad = 100,
            Ocupacion = 50,
            DineroPorCiclo = 10,
            EnergiaPorCiclo = 5
        };

        _mockAccesoUsuarios.Setup(a => a.EsDios()).Returns(true);
        _mockTipoEstructuraRepositorio.Setup(r => r.Agregar(It.IsAny<TipoEstructura>()))
            .Returns(Task.CompletedTask);

        // Act
        var resultado = await _tipoEstructuraLogica.Crear(tipoEstructuraNuevo);

        // Assert
        Assert.NotNull(resultado);
        _mockTipoEstructuraRepositorio.Verify(r => r.Agregar(It.IsAny<TipoEstructura>()), Times.Once);
        _mockUow.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task Crear_NoEsAdmin_LanzaExcepcion()
    {
        // Arrange
        var tipoEstructuraNuevo = new TipoEstructura { Nombre = "Test" };

        _mockAccesoUsuarios.Setup(a => a.EsDios()).Returns(false);

        // Act & Assert
        await Assert.ThrowsAsync<AccesoDenegadoExcepcion>(() => _tipoEstructuraLogica.Crear(tipoEstructuraNuevo));
        _mockTipoEstructuraRepositorio.Verify(r => r.Agregar(It.IsAny<TipoEstructura>()), Times.Never);
    }

    [Fact]
    public async Task Crear_TipoEstructuraNull_LanzaExcepcion()
    {
        // Arrange
        _mockAccesoUsuarios.Setup(a => a.EsDios()).Returns(true);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _tipoEstructuraLogica.Crear(null!));
    }

    [Fact]
    public async Task Actualizar_TipoEstructuraValido_ActualizaTipoEstructura()
    {
        // Arrange
        const int id = 1;
        var tipoEstructuraExistente = new TipoEstructura
        {
            Id = id,
            Nombre = "Tipo Original"
        };
        var tipoEstructuraActualizado = new TipoEstructura
        {
            Nombre = "Tipo Actualizado",
            Capacidad = 200,
            Ocupacion = 100,
            DineroPorCiclo = 20,
            EnergiaPorCiclo = 10
        };

        _mockAccesoUsuarios.Setup(a => a.EsDios()).Returns(true);
        _mockTipoEstructuraRepositorio.Setup(r => r.ObtenerPorId(id))
            .ReturnsAsync(tipoEstructuraExistente);
        _mockTipoEstructuraRepositorio.Setup(r => r.Actualizar(It.IsAny<TipoEstructura>()))
            .Returns(Task.CompletedTask);

        // Act
        await _tipoEstructuraLogica.Actualizar(tipoEstructuraActualizado, id);

        // Assert
        _mockTipoEstructuraRepositorio.Verify(r => r.ObtenerPorId(id), Times.Once);
        _mockTipoEstructuraRepositorio.Verify(r => r.Actualizar(It.IsAny<TipoEstructura>()), Times.Once);
        _mockUow.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task Actualizar_TipoEstructuraNoExiste_LanzaExcepcion()
    {
        // Arrange
        var tipoEstructuraActualizado = new TipoEstructura { Nombre = "Test" };

        _mockAccesoUsuarios.Setup(a => a.EsDios()).Returns(true);
        _mockTipoEstructuraRepositorio.Setup(r => r.ObtenerPorId(1))
            .ReturnsAsync((TipoEstructura?)null);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _tipoEstructuraLogica.Actualizar(tipoEstructuraActualizado, 1));
    }

    [Fact]
    public async Task Actualizar_NoEsAdmin_LanzaExcepcion()
    {
        // Arrange
        var tipoEstructuraActualizado = new TipoEstructura { Nombre = "Test" };
        var tipoEstructuraExistente = new TipoEstructura { Id = 1 };

        _mockAccesoUsuarios.Setup(a => a.EsDios()).Returns(false);
        _mockTipoEstructuraRepositorio.Setup(r => r.ObtenerPorId(1))
            .ReturnsAsync(tipoEstructuraExistente);

        // Act & Assert
        await Assert.ThrowsAsync<AccesoDenegadoExcepcion>(() => _tipoEstructuraLogica.Actualizar(tipoEstructuraActualizado, 1));
    }

    [Fact]
    public async Task Eliminar_IdValido_EliminaTipoEstructura()
    {
        // Arrange
        const int id = 1;

        _mockAccesoUsuarios.Setup(a => a.EsDios()).Returns(true);
        _mockTipoEstructuraRepositorio.Setup(r => r.Eliminar(id))
            .Returns(Task.CompletedTask);

        // Act
        await _tipoEstructuraLogica.Eliminar(id);

        // Assert
        _mockTipoEstructuraRepositorio.Verify(r => r.Eliminar(id), Times.Once);
        _mockUow.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task Eliminar_IdInvalido_LanzaExcepcion()
    {
        // Arrange
        const int idInvalido = 0;

        _mockAccesoUsuarios.Setup(a => a.EsDios()).Returns(true);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _tipoEstructuraLogica.Eliminar(idInvalido));
        _mockTipoEstructuraRepositorio.Verify(r => r.Eliminar(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task Eliminar_NoEsAdmin_LanzaExcepcion()
    {
        // Arrange
        const int id = 1;

        _mockAccesoUsuarios.Setup(a => a.EsDios()).Returns(false);

        // Act & Assert
        await Assert.ThrowsAsync<AccesoDenegadoExcepcion>(() => _tipoEstructuraLogica.Eliminar(id));
        _mockTipoEstructuraRepositorio.Verify(r => r.Eliminar(It.IsAny<int>()), Times.Never);
    }
}



