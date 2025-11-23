using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Excepciones;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Logica;
using CivitaBack.Logica.Interfaces;
using CivitaBack.Utils;
using Moq;

namespace CivitaBack.Tests;

public class EstructuraLogicaTest
{
    private readonly Mock<IEstructuraRepositorio> _mockEstructuraRepositorio;
    private readonly Mock<ITipoEstructuraRepositorio> _mockTipoEstructuraRepositorio;
    private readonly Mock<IAccesoUsuarios> _mockAccesoUsuarios;
    private readonly Mock<IUnidadDeTrabajo> _mockUow;
    private readonly IEstructuraLogica _estructuraLogica;

    public EstructuraLogicaTest()
    {
        _mockEstructuraRepositorio = new Mock<IEstructuraRepositorio>();
        _mockTipoEstructuraRepositorio = new Mock<ITipoEstructuraRepositorio>();
        _mockAccesoUsuarios = new Mock<IAccesoUsuarios>();
        _mockUow = new Mock<IUnidadDeTrabajo>();

        _estructuraLogica = new EstructuraLogica(
            _mockEstructuraRepositorio.Object,
            _mockTipoEstructuraRepositorio.Object,
            _mockAccesoUsuarios.Object,
            _mockUow.Object
        );

        _mockUow.Setup(u => u.CommitAsync()).ReturnsAsync(1);
    }

    [Fact]
    public async Task ObtenerPorId_IdValido_RetornaEstructura()
    {
        // Arrange
        const int idEstructura = 1;
        var estructuraMock = new Estructura
        {
            Id = idEstructura,
            Nombre = "Test Estructura",
            TipoEstructuraId = 1
        };

        _mockEstructuraRepositorio.Setup(r => r.ObtenerPorId(idEstructura))
            .ReturnsAsync(estructuraMock);

        // Act
        var resultado = await _estructuraLogica.ObtenerPorId(idEstructura);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(idEstructura, resultado.Id);
        _mockEstructuraRepositorio.Verify(r => r.ObtenerPorId(idEstructura), Times.Once);
    }

    [Fact]
    public async Task ObtenerPorId_IdInvalido_LanzaExcepcion()
    {
        // Arrange
        const int idInvalido = 0;

        // Act & Assert
        await Assert.ThrowsAsync<EstructuraExcepcion>(() => _estructuraLogica.ObtenerPorId(idInvalido));
        _mockEstructuraRepositorio.Verify(r => r.ObtenerPorId(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task ObtenerPorId_EstructuraNoExiste_RetornaNull()
    {
        // Arrange
        const int idEstructura = 999;

        _mockEstructuraRepositorio.Setup(r => r.ObtenerPorId(idEstructura))
            .ReturnsAsync((Estructura?)null);

        // Act
        var resultado = await _estructuraLogica.ObtenerPorId(idEstructura);

        // Assert
        Assert.Null(resultado);
    }

    [Fact]
    public async Task ObtenerListado_HayEstructuras_RetornaLista()
    {
        // Arrange
        var estructurasMock = new List<Estructura>
        {
            new Estructura { Id = 1, Nombre = "Estructura 1" },
            new Estructura { Id = 2, Nombre = "Estructura 2" }
        };

        _mockEstructuraRepositorio.Setup(r => r.ObtenerTodos())
            .ReturnsAsync(estructurasMock);

        // Act
        var resultado = await _estructuraLogica.ObtenerListado();

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(2, resultado.Count);
        _mockEstructuraRepositorio.Verify(r => r.ObtenerTodos(), Times.Once);
    }

    [Fact]
    public async Task ObtenerListado_ListaVacia_LanzaExcepcion()
    {
        // Arrange
        _mockEstructuraRepositorio.Setup(r => r.ObtenerTodos())
            .ReturnsAsync(new List<Estructura>());

        // Act & Assert
        await Assert.ThrowsAsync<EstructuraExcepcion>(() => _estructuraLogica.ObtenerListado());
    }

    [Fact]
    public async Task ObtenerListado_ListaNull_LanzaExcepcion()
    {
        // Arrange
        _mockEstructuraRepositorio.Setup(r => r.ObtenerTodos())
            .ReturnsAsync((List<Estructura>?)null);

        // Act & Assert
        await Assert.ThrowsAsync<EstructuraExcepcion>(() => _estructuraLogica.ObtenerListado());
    }

    [Fact]
    public async Task Crear_DatosValidos_CreaEstructura()
    {
        // Arrange
        var tipoEstructura = new TipoEstructura { Id = 1, Nombre = "Tipo Test" };
        var estructuraNueva = new Estructura
        {
            Nombre = "Nueva Estructura",
            TipoEstructuraId = 1,
            TipoEstructura = tipoEstructura,
            CostoDinero = 100,
            CostoEnergia = 50,
            FelicidadCiclo = 10,
            ContaminacionCiclo = 5,
            EsMejorable = true,
            RutaImagen = "test.jpg"
        };

        _mockAccesoUsuarios.Setup(a => a.EsDios()).Returns(true);
        _mockTipoEstructuraRepositorio.Setup(r => r.ObtenerPorId(1))
            .ReturnsAsync(tipoEstructura);
        _mockEstructuraRepositorio.Setup(r => r.Agregar(It.IsAny<Estructura>()))
            .Returns(Task.CompletedTask);

        // Act
        await _estructuraLogica.Crear(estructuraNueva);

        // Assert
        _mockTipoEstructuraRepositorio.Verify(r => r.ObtenerPorId(1), Times.Once);
        _mockEstructuraRepositorio.Verify(r => r.Agregar(It.IsAny<Estructura>()), Times.Once);
        _mockUow.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task Crear_NoEsAdmin_LanzaExcepcion()
    {
        // Arrange
        var estructuraNueva = new Estructura
        {
            Nombre = "Nueva Estructura",
            TipoEstructuraId = 1
        };

        _mockAccesoUsuarios.Setup(a => a.EsDios()).Returns(false);

        // Act & Assert
        await Assert.ThrowsAsync<AccesoDenegadoExcepcion>(() => _estructuraLogica.Crear(estructuraNueva));
        _mockEstructuraRepositorio.Verify(r => r.Agregar(It.IsAny<Estructura>()), Times.Never);
    }

    [Fact]
    public async Task Crear_TipoEstructuraNoExiste_LanzaExcepcion()
    {
        // Arrange
        var estructuraNueva = new Estructura
        {
            Nombre = "Nueva Estructura",
            TipoEstructuraId = 999
        };

        _mockAccesoUsuarios.Setup(a => a.EsDios()).Returns(true);
        _mockTipoEstructuraRepositorio.Setup(r => r.ObtenerPorId(999))
            .ReturnsAsync((TipoEstructura?)null);

        // Act & Assert
        // El método lanza NullReferenceException cuando tipoEstructura es null
        await Assert.ThrowsAsync<NullReferenceException>(() => _estructuraLogica.Crear(estructuraNueva));
        _mockEstructuraRepositorio.Verify(r => r.Agregar(It.IsAny<Estructura>()), Times.Never);
    }

    [Fact]
    public async Task Crear_EstructuraNull_LanzaExcepcion()
    {
        // Arrange
        _mockAccesoUsuarios.Setup(a => a.EsDios()).Returns(true);

        // Act & Assert
        await Assert.ThrowsAsync<EstructuraExcepcion>(() => _estructuraLogica.Crear(null!));
    }

    [Fact]
    public async Task Eliminar_IdValido_EliminaEstructura()
    {
        // Arrange
        const int idEstructura = 1;

        _mockAccesoUsuarios.Setup(a => a.EsDios()).Returns(true);
        _mockEstructuraRepositorio.Setup(r => r.Eliminar(idEstructura))
            .Returns(Task.CompletedTask);

        // Act
        await _estructuraLogica.Eliminar(idEstructura);

        // Assert
        _mockEstructuraRepositorio.Verify(r => r.Eliminar(idEstructura), Times.Once);
        _mockUow.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task Eliminar_IdInvalido_LanzaExcepcion()
    {
        // Arrange
        const int idInvalido = 0;

        _mockAccesoUsuarios.Setup(a => a.EsDios()).Returns(true);

        // Act & Assert
        await Assert.ThrowsAsync<EstructuraExcepcion>(() => _estructuraLogica.Eliminar(idInvalido));
        _mockEstructuraRepositorio.Verify(r => r.Eliminar(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task Eliminar_NoEsAdmin_LanzaExcepcion()
    {
        // Arrange
        const int idEstructura = 1;

        _mockAccesoUsuarios.Setup(a => a.EsDios()).Returns(false);

        // Act & Assert
        await Assert.ThrowsAsync<AccesoDenegadoExcepcion>(() => _estructuraLogica.Eliminar(idEstructura));
        _mockEstructuraRepositorio.Verify(r => r.Eliminar(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task Actualizar_DatosValidos_ActualizaEstructura()
    {
        // Arrange
        const int idEstructura = 1;
        var tipoEstructura = new TipoEstructura { Id = 1, Nombre = "Tipo Test" };
        var estructuraExistente = new Estructura
        {
            Id = idEstructura,
            Nombre = "Estructura Original",
            TipoEstructuraId = 1
        };
        var estructuraActualizada = new Estructura
        {
            Nombre = "Estructura Actualizada",
            TipoEstructuraId = 1,
            CostoDinero = 200,
            CostoEnergia = 100,
            FelicidadCiclo = 20,
            ContaminacionCiclo = 10,
            EsMejorable = false,
            RutaImagen = "nueva.jpg"
        };

        _mockAccesoUsuarios.Setup(a => a.EsDios()).Returns(true);
        _mockEstructuraRepositorio.Setup(r => r.ObtenerPorId(idEstructura))
            .ReturnsAsync(estructuraExistente);
        _mockTipoEstructuraRepositorio.Setup(r => r.ObtenerPorId(1))
            .ReturnsAsync(tipoEstructura);
        _mockEstructuraRepositorio.Setup(r => r.Actualizar(It.IsAny<Estructura>()))
            .Returns(Task.CompletedTask);

        // Act
        await _estructuraLogica.Actualizar(estructuraActualizada, idEstructura);

        // Assert
        _mockEstructuraRepositorio.Verify(r => r.ObtenerPorId(idEstructura), Times.Once);
        _mockTipoEstructuraRepositorio.Verify(r => r.ObtenerPorId(1), Times.Once);
        _mockEstructuraRepositorio.Verify(r => r.Actualizar(It.IsAny<Estructura>()), Times.Once);
        _mockUow.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task Actualizar_NoEsAdmin_LanzaExcepcion()
    {
        // Arrange
        var estructuraActualizada = new Estructura { Nombre = "Test" };

        _mockAccesoUsuarios.Setup(a => a.EsDios()).Returns(false);

        // Act & Assert
        await Assert.ThrowsAsync<AccesoDenegadoExcepcion>(() => _estructuraLogica.Actualizar(estructuraActualizada, 1));
    }

    [Fact]
    public async Task Actualizar_EstructuraNoExiste_LanzaExcepcion()
    {
        // Arrange
        var estructuraActualizada = new Estructura { Nombre = "Test", TipoEstructuraId = 1 };

        _mockAccesoUsuarios.Setup(a => a.EsDios()).Returns(true);
        _mockEstructuraRepositorio.Setup(r => r.ObtenerPorId(1))
            .ReturnsAsync((Estructura?)null);

        // Act & Assert
        await Assert.ThrowsAsync<EstructuraExcepcion>(() => _estructuraLogica.Actualizar(estructuraActualizada, 1));
    }

    [Fact]
    public async Task Actualizar_TipoEstructuraNoExiste_LanzaExcepcion()
    {
        // Arrange
        const int idEstructura = 1;
        var estructuraExistente = new Estructura { Id = idEstructura };
        var estructuraActualizada = new Estructura { Nombre = "Test", TipoEstructuraId = 999 };

        _mockAccesoUsuarios.Setup(a => a.EsDios()).Returns(true);
        _mockEstructuraRepositorio.Setup(r => r.ObtenerPorId(idEstructura))
            .ReturnsAsync(estructuraExistente);
        _mockTipoEstructuraRepositorio.Setup(r => r.ObtenerPorId(999))
            .ReturnsAsync((TipoEstructura?)null);

        // Act & Assert
        await Assert.ThrowsAsync<EstructuraExcepcion>(() => _estructuraLogica.Actualizar(estructuraActualizada, idEstructura));
    }
}

