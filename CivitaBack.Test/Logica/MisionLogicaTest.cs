using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Enum;
using CivitaBack.Domain.Excepciones;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Logica;
using CivitaBack.Logica.Interfaces;
using CivitaBack.Utils;
using Moq;

namespace CivitaBack.Tests;

public class MisionLogicaTest
{
    private readonly Mock<IMisionRepositorio> _mockMisionRepositorio;
    private readonly Mock<IMisionPartidaRepositorio> _mockMisionPartidaRepositorio;
    private readonly Mock<ICondicionRepositorio> _mockCondicionRepositorio;
    private readonly Mock<IUnidadDeTrabajo> _mockUnidadDeTrabajo;
    private readonly Mock<IAccesoUsuarios> _mockAccesoUsuarios;

    private readonly IMisionLogica _misionLogica;

    public MisionLogicaTest()
    {
        
        _mockMisionRepositorio = new Mock<IMisionRepositorio>();
        _mockMisionPartidaRepositorio = new Mock<IMisionPartidaRepositorio>();
        _mockCondicionRepositorio = new Mock<ICondicionRepositorio>();
        _mockUnidadDeTrabajo = new Mock<IUnidadDeTrabajo>();
        _mockAccesoUsuarios = new Mock<IAccesoUsuarios>();

        _misionLogica = new MisionLogica(
            _mockMisionRepositorio.Object,
            _mockCondicionRepositorio.Object,
            _mockUnidadDeTrabajo.Object,
            _mockMisionPartidaRepositorio.Object,
            _mockAccesoUsuarios.Object
        );
        
        // Mockeo por defecto para el CommitAsync
        _mockUnidadDeTrabajo.Setup(u => u.CommitAsync()).ReturnsAsync(1);
    }

    // --- TESTS PARA Crear ---
    [Fact]
    public async Task Crear_ConDatosValidos_DebeAgregarYGuardar()
    {
        // Arrange
        var mision = new Mision { Titulo = "Test", Descripcion = "Test Desc", CondicionId = 1 };
        var condicion = new Condicion { Id = 1, Cantidad = 100 };

        // 1. Simular que el usuario SÍ es Admin (BYPASS)
        _mockAccesoUsuarios.Setup(a => a.EsDios()).Returns(true);

        _mockCondicionRepositorio.Setup(r => r.ObtenerPorId(1)).ReturnsAsync(condicion);
        _mockMisionRepositorio.Setup(r => r.Agregar(It.IsAny<Mision>())).Returns(Task.CompletedTask);

        // Act
        await _misionLogica.Crear(mision);

        // Assert
        // La lógica procede porque el bypass fue activado.
        _mockMisionRepositorio.Verify(r => r.Agregar(It.Is<Mision>(m => m.Titulo == mision.Titulo)), Times.Once);
        _mockUnidadDeTrabajo.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task Crear_ConTituloVacio_DebeLanzarExcepcion()
    {
        // Arrange
        var mision = new Mision { Titulo = "", Descripcion = "Test Desc", CondicionId = 1 };

        // Act & Assert
        // El método ValidarMision debe lanzar la excepción
        await Assert.ThrowsAsync<MisionExcepcion>(() => _misionLogica.Crear(mision));
    }
    
    [Fact]
    public async Task Crear_ConCondicionInvalida_DebeLanzarExcepcion()
    {
        // Arrange
        var mision = new Mision { Titulo = "Test", Descripcion = "Test Desc", CondicionId = 99 };
        
        // Simulamos que el repositorio no encuentra la condición
        _mockCondicionRepositorio.Setup(r => r.ObtenerPorId(99)).ReturnsAsync((Condicion)null);

        // Act & Assert
        await Assert.ThrowsAsync<MisionExcepcion>(() => _misionLogica.Crear(mision));
    }
    
    // --- TESTS PARA Actualizar ---
   
    [Fact]
    public async Task Actualizar_ConDatosValidos_DebeActualizarYGuardar()
    {
        // Arrange
        var misionNuevosDatos = new Mision { Titulo = "Nuevo Titulo", Descripcion = "Nueva Desc", CondicionId = 1, Tipo = TipoMision.Semanal };
        var condicion = new Condicion { Id = 1, Cantidad = 100 };
        var misionDb = new Mision { Id = 5, Titulo = "Viejo Titulo", Descripcion = "Vieja Desc", CondicionId = 2, Tipo = TipoMision.Diaria };

        _mockCondicionRepositorio.Setup(r => r.ObtenerPorId(1)).ReturnsAsync(condicion);
        _mockMisionRepositorio.Setup(r => r.ObtenerPorId(5)).ReturnsAsync(misionDb);

        _mockAccesoUsuarios.Setup(a => a.EsDios()).Returns(true);

        // Act
        await _misionLogica.Actualizar(misionNuevosDatos, 5);

        // Assert
        // Verifica que la entidad de la BD fue actualizada
        Assert.Equal("Nuevo Titulo", misionDb.Titulo);
        Assert.Equal(TipoMision.Semanal, misionDb.Tipo);
        
        // Verifica que se llamó a Actualizar y Guardar
        _mockMisionRepositorio.Verify(r => r.Actualizar(misionDb), Times.Once);
        _mockUnidadDeTrabajo.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task Actualizar_MisionNoEncontrada_DebeLanzarExcepcion()
    {
        // Arrange
        var misionNuevosDatos = new Mision { Titulo = "Test", Descripcion = "Test Desc", CondicionId = 1 };
        var condicion = new Condicion { Id = 1, Cantidad = 100 };
        
        _mockCondicionRepositorio.Setup(r => r.ObtenerPorId(1)).ReturnsAsync(condicion);
        // Simulamos que la misión a actualizar no existe
        _mockMisionRepositorio.Setup(r => r.ObtenerPorId(99)).ReturnsAsync((Mision)null);

        // Act & Assert
        await Assert.ThrowsAsync<MisionExcepcion>(() => _misionLogica.Actualizar(misionNuevosDatos, 99));
    }
    
    // --- TESTS PARA ObtenerPorId ---

    [Fact]
    public async Task ObtenerPorId_MisionNoExiste_DebeLanzarExcepcion()
    {
        // Arrange
        _mockMisionRepositorio.Setup(r => r.ObtenerPorId(99)).ReturnsAsync((Mision)null);

        // Act & Assert
        await Assert.ThrowsAsync<MisionExcepcion>(() => _misionLogica.ObtenerPorId(99));
    }
    
    // --- TESTS PARA AsignarMisiones ---
    
    [Fact]
    public async Task AsignarMisiones_ConPartidaValida_DebeAgregarMisionesYGuardar()
    {
        // Arrange
        var partida = new Partida { Id = 1 };
        var misionesActivas = new List<Mision>
        {
            new Mision { Id = 1, Titulo = "Mision 1" },
            new Mision { Id = 2, Titulo = "Mision 2" }
        };
        
        _mockMisionRepositorio.Setup(r => r.ListadoActivo()).ReturnsAsync(misionesActivas);

        // Act
        await _misionLogica.AsignarMisiones(partida);

        // Assert
        // Verifica que se llamó al método del repo de unión con los datos correctos
        _mockMisionPartidaRepositorio.Verify(
            r => r.AgregarMisionesPartida(misionesActivas, partida), 
            Times.Once
        );
        _mockUnidadDeTrabajo.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task AsignarMisiones_ConPartidaNull_DebeLanzarExcepcion()
    {
        // Arrange
        Partida? partida = null;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<MisionExcepcion>(() => _misionLogica.AsignarMisiones(partida));
        Assert.Equal("No se encontró la partida", exception.Message);
    }
    
    // --- TESTS PARA ResetMisiones ---
    
    [Fact]
    public async Task ResetMisiones_SinMisiones_NoDebeGuardar()
    {
        // Arrange
        _mockMisionPartidaRepositorio.Setup(r => r.Listado())
            .ReturnsAsync(new List<MisionPartida>());

        // Act
        await _misionLogica.ResetMisiones(TipoMision.Diaria);

        // Assert
        _mockMisionPartidaRepositorio.Verify(r => r.ActualizarVarios(It.IsAny<ICollection<MisionPartida>>()), Times.Never);
        _mockUnidadDeTrabajo.Verify(u => u.CommitAsync(), Times.Never);
    }

    [Fact]
    public async Task ResetMisiones_ConMisiones_ReseteaMisiones()
    {
        // Arrange
        var misionesPartida = new List<MisionPartida>
        {
            new MisionPartida
            {
                Id = 1,
                Reclamado = true,
                FechaCompletado = DateTime.UtcNow,
                FechaEntrega = DateTime.UtcNow.AddDays(-1)
            },
            new MisionPartida
            {
                Id = 2,
                Reclamado = true,
                FechaCompletado = DateTime.UtcNow,
                FechaEntrega = DateTime.UtcNow.AddDays(-1)
            }
        };

        _mockMisionPartidaRepositorio.Setup(r => r.Listado())
            .ReturnsAsync(misionesPartida);
        _mockMisionPartidaRepositorio.Setup(r => r.ActualizarVarios(It.IsAny<ICollection<MisionPartida>>()))
            .Returns(Task.CompletedTask);

        // Act
        await _misionLogica.ResetMisiones(TipoMision.Diaria);

        // Assert
        Assert.All(misionesPartida, mp =>
        {
            Assert.False(mp.Reclamado);
            Assert.Null(mp.FechaCompletado);
            Assert.NotNull(mp.FechaEntrega);
        });
        _mockMisionPartidaRepositorio.Verify(r => r.ActualizarVarios(misionesPartida), Times.Once);
        _mockUnidadDeTrabajo.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task Listado_RetornaListaMisiones()
    {
        // Arrange
        var misionesMock = new List<Mision>
        {
            new Mision { Id = 1, Titulo = "Mision 1" },
            new Mision { Id = 2, Titulo = "Mision 2" }
        };

        _mockMisionRepositorio.Setup(r => r.Listado())
            .ReturnsAsync(misionesMock);

        // Act
        var resultado = await _misionLogica.Listado();

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(2, resultado.Count);
        _mockMisionRepositorio.Verify(r => r.Listado(), Times.Once);
    }

    [Fact]
    public async Task ObtenerPorId_MisionExiste_RetornaMision()
    {
        // Arrange
        const int idMision = 1;
        var misionMock = new Mision
        {
            Id = idMision,
            Titulo = "Test Mision"
        };

        _mockMisionRepositorio.Setup(r => r.ObtenerPorId(idMision))
            .ReturnsAsync(misionMock);

        // Act
        var resultado = await _misionLogica.ObtenerPorId(idMision);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(idMision, resultado.Id);
        _mockMisionRepositorio.Verify(r => r.ObtenerPorId(idMision), Times.Once);
    }
    

    [Fact]
    public async Task ObtenerMisionesDisponibles_RetornaMisionesActivas()
    {
        // Arrange
        var misionesMock = new List<Mision>
        {
            new Mision { Id = 1, Titulo = "Mision 1", Disponible = true },
            new Mision { Id = 2, Titulo = "Mision 2", Disponible = true }
        };

        _mockMisionRepositorio.Setup(r => r.ListadoActivo())
            .ReturnsAsync(misionesMock);

        // Act
        var resultado = await _misionLogica.ObtenerMisionesDisponibles();

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(2, resultado.Count);
        _mockMisionRepositorio.Verify(r => r.ListadoActivo(), Times.Once);
    }

    [Fact]
    public async Task Crear_NoEsAdmin_LanzaExcepcion()
    {
        // Arrange
        var mision = new Mision { Titulo = "Test", Descripcion = "Test Desc", CondicionId = 1 };
        var condicion = new Condicion { Id = 1, Cantidad = 100 };

        _mockCondicionRepositorio.Setup(r => r.ObtenerPorId(1))
            .ReturnsAsync(condicion);
        _mockAccesoUsuarios.Setup(a => a.EsDios()).Returns(false);

        // Act & Assert
        await Assert.ThrowsAsync<AccesoDenegadoExcepcion>(() => _misionLogica.Crear(mision));
        _mockMisionRepositorio.Verify(r => r.Agregar(It.IsAny<Mision>()), Times.Never);
    }

    [Fact]
    public async Task Actualizar_NoEsAdmin_LanzaExcepcion()
    {
        // Arrange
        var misionNuevosDatos = new Mision { Titulo = "Test", Descripcion = "Test Desc", CondicionId = 1 };
        var condicion = new Condicion { Id = 1, Cantidad = 100 };
        var misionDb = new Mision { Id = 5, Titulo = "Viejo", Descripcion = "Vieja", CondicionId = 2 };

        _mockCondicionRepositorio.Setup(r => r.ObtenerPorId(1))
            .ReturnsAsync(condicion);
        _mockMisionRepositorio.Setup(r => r.ObtenerPorId(5))
            .ReturnsAsync(misionDb);
        _mockAccesoUsuarios.Setup(a => a.EsDios()).Returns(false);

        // Act & Assert
        await Assert.ThrowsAsync<AccesoDenegadoExcepcion>(() => _misionLogica.Actualizar(misionNuevosDatos, 5));
        _mockMisionRepositorio.Verify(r => r.Actualizar(It.IsAny<Mision>()), Times.Never);
    }
}