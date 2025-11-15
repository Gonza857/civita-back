using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Enum;
using CivitaBack.Domain.Excepciones;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Logica;
using CivitaBack.Utils;
using Moq;

namespace CivitaBack.Tests;

public class CondicionLogicaTest
{
    private readonly Mock<ICondicionRepositorio> _mockCondicionRepositorio;
    private readonly Mock<IEstructuraRepositorio> _mockEstructuraRepositorio;
    private readonly Mock<IUnidadDeTrabajo> _mockUow;
    private readonly ICondicionLogica _condicionLogica;

    public CondicionLogicaTest()
    {
        _mockCondicionRepositorio = new Mock<ICondicionRepositorio>();
        _mockEstructuraRepositorio = new Mock<IEstructuraRepositorio>();
        _mockUow = new Mock<IUnidadDeTrabajo>();

        _condicionLogica = new CondicionLogica(
            _mockCondicionRepositorio.Object,
            _mockEstructuraRepositorio.Object,
            _mockUow.Object
        );

        _mockUow.Setup(u => u.CommitAsync()).ReturnsAsync(1);
    }

    [Fact]
    public async Task ObtenerPorId_CondicionExiste_RetornaCondicion()
    {
        // Arrange
        const int id = 1;
        var condicionMock = new Condicion
        {
            Id = id,
            Cantidad = 100,
            NombreColumna = "EcoCoins"
        };

        _mockCondicionRepositorio.Setup(r => r.ObtenerPorId(id))
            .ReturnsAsync(condicionMock);

        // Act
        var resultado = await _condicionLogica.ObtenerPorId(id);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(id, resultado.Id);
        _mockCondicionRepositorio.Verify(r => r.ObtenerPorId(id), Times.Once);
    }

    [Fact]
    public async Task ObtenerPorId_CondicionNoExiste_LanzaExcepcion()
    {
        // Arrange
        const int id = 999;

        _mockCondicionRepositorio.Setup(r => r.ObtenerPorId(id))
            .ReturnsAsync((Condicion?)null);

        // Act & Assert
        await Assert.ThrowsAsync<CondicionExcepcion>(() => _condicionLogica.ObtenerPorId(id));
    }

    [Fact]
    public async Task ObtenerListado_HayCondiciones_RetornaLista()
    {
        // Arrange
        var condicionesMock = new List<Condicion>
        {
            new Condicion { Id = 1, NombreColumna = "EcoCoins", Cantidad = 100 },
            new Condicion { Id = 2, NombreColumna = "Felicidad", Cantidad = 50 }
        };

        _mockCondicionRepositorio.Setup(r => r.ObtenerTodos())
            .ReturnsAsync(condicionesMock);

        // Act
        var resultado = await _condicionLogica.ObtenerListado();

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(2, resultado.Count);
        _mockCondicionRepositorio.Verify(r => r.ObtenerTodos(), Times.Once);
    }

    [Fact]
    public async Task Crear_CondicionPorRecurso_CreaCondicion()
    {
        // Arrange
        var condicionNueva = new Condicion
        {
            NombreColumna = "EcoCoins",
            Cantidad = 100
        };

        _mockCondicionRepositorio.Setup(r => r.Agregar(It.IsAny<Condicion>()))
            .Returns(Task.CompletedTask);

        // Act
        await _condicionLogica.Crear(condicionNueva);

        // Assert
        _mockCondicionRepositorio.Verify(r => r.Agregar(It.Is<Condicion>(c => 
            c.NombreColumna == "EcoCoins" && 
            c.Cantidad == 100 &&
            c.EsRecompensa == false
        )), Times.Once);
        _mockUow.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task Crear_CondicionPorEstructura_CreaCondicion()
    {
        // Arrange
        var estructura = new Estructura { Id = 1 };
        var condicionNueva = new Condicion
        {
            EstructuraId = 1,
            Cantidad = 5
        };

        _mockEstructuraRepositorio.Setup(r => r.ObtenerPorId(1))
            .ReturnsAsync(estructura);
        _mockCondicionRepositorio.Setup(r => r.Agregar(It.IsAny<Condicion>()))
            .Returns(Task.CompletedTask);

        // Act
        await _condicionLogica.Crear(condicionNueva);

        // Assert
        _mockEstructuraRepositorio.Verify(r => r.ObtenerPorId(1), Times.Once);
        _mockCondicionRepositorio.Verify(r => r.Agregar(It.IsAny<Condicion>()), Times.Once);
        _mockUow.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task Crear_EstructuraNoExiste_LanzaExcepcion()
    {
        // Arrange
        var condicionNueva = new Condicion
        {
            EstructuraId = 999,
            Cantidad = 5
        };

        _mockEstructuraRepositorio.Setup(r => r.ObtenerPorId(999))
            .ReturnsAsync((Estructura?)null);

        // Act & Assert
        await Assert.ThrowsAsync<CondicionExcepcion>(() => _condicionLogica.Crear(condicionNueva));
    }

    [Fact]
    public async Task Eliminar_IdValido_EliminaCondicion()
    {
        // Arrange
        const int id = 1;

        _mockCondicionRepositorio.Setup(r => r.Eliminar(id))
            .Returns(Task.CompletedTask);

        // Act
        await _condicionLogica.Eliminar(id);

        // Assert
        _mockCondicionRepositorio.Verify(r => r.Eliminar(id), Times.Once);
        _mockUow.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task Eliminar_IdInvalido_LanzaExcepcion()
    {
        // Arrange
        const int idInvalido = 0;

        // Act & Assert
        await Assert.ThrowsAsync<CondicionExcepcion>(() => _condicionLogica.Eliminar(idInvalido));
        _mockCondicionRepositorio.Verify(r => r.Eliminar(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task Actualizar_CondicionValida_ActualizaCondicion()
    {
        // Arrange
        const int id = 1;
        var condicionExistente = new Condicion
        {
            Id = id,
            Cantidad = 50,
            NombreColumna = "EcoCoins"
        };
        var condicionActualizada = new Condicion
        {
            Cantidad = 100,
            NombreColumna = "EcoCoins"
        };

        _mockCondicionRepositorio.Setup(r => r.ObtenerPorId(id))
            .ReturnsAsync(condicionExistente);
        _mockCondicionRepositorio.Setup(r => r.Actualizar(It.IsAny<Condicion>()))
            .Returns(Task.CompletedTask);

        // Act
        await _condicionLogica.Actualizar(condicionActualizada, id);

        // Assert
        _mockCondicionRepositorio.Verify(r => r.ObtenerPorId(id), Times.Once);
        _mockCondicionRepositorio.Verify(r => r.Actualizar(It.IsAny<Condicion>()), Times.Once);
        _mockUow.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task Actualizar_CondicionNoExiste_LanzaExcepcion()
    {
        // Arrange
        var condicionActualizada = new Condicion { NombreColumna = "EcoCoins", Cantidad = 100 };

        _mockCondicionRepositorio.Setup(r => r.ObtenerPorId(1))
            .ReturnsAsync((Condicion?)null);

        // Act & Assert
        await Assert.ThrowsAsync<CondicionExcepcion>(() => _condicionLogica.Actualizar(condicionActualizada, 1));
    }

    [Fact]
    public async Task ObtenerListadoRecompensas_HayRecompensas_RetornaLista()
    {
        // Arrange
        var recompensasMock = new List<Condicion>
        {
            new Condicion { Id = 1, EsRecompensa = true, NombreColumna = "EcoCoins", Cantidad = 50 },
            new Condicion { Id = 2, EsRecompensa = true, NombreColumna = "Felicidad", Cantidad = 10 }
        };

        _mockCondicionRepositorio.Setup(r => r.ObtenerTodasRecompensas())
            .ReturnsAsync(recompensasMock);

        // Act
        var resultado = await _condicionLogica.ObtenerListadoRecompensas();

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(2, resultado.Count);
        _mockCondicionRepositorio.Verify(r => r.ObtenerTodasRecompensas(), Times.Once);
    }

    [Fact]
    public async Task CrearRecompensa_RecompensaValida_CreaRecompensa()
    {
        // Arrange
        var recompensaNueva = new Condicion
        {
            NombreColumna = "EcoCoins",
            Cantidad = 50
        };

        _mockCondicionRepositorio.Setup(r => r.Agregar(It.IsAny<Condicion>()))
            .Returns(Task.CompletedTask);

        // Act
        await _condicionLogica.CrearRecompensa(recompensaNueva);

        // Assert
        _mockCondicionRepositorio.Verify(r => r.Agregar(It.Is<Condicion>(c => 
            c.EsRecompensa == true
        )), Times.Once);
        _mockUow.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public void FiltrarCondicionSiCumple_CondicionCumple_RetornaListaConCondicion()
    {
        // Arrange
        var partida = new Partida
        {
            Recursos = new Recurso
            {
                EcoCoins = 150
            }
        };
        var condicion = new Condicion
        {
            NombreColumna = "EcoCoins",
            Cantidad = 100
        };

        // Act
        var resultado = _condicionLogica.FiltrarCondicionSiCumple(condicion, partida);

        // Assert
        Assert.NotNull(resultado);
        Assert.Single(resultado);
        Assert.Equal(condicion.Id, resultado[0].Id);
    }

    [Fact]
    public void FiltrarCondicionSiCumple_CondicionNoCumple_RetornaListaVacia()
    {
        // Arrange
        var partida = new Partida
        {
            Recursos = new Recurso
            {
                EcoCoins = 50
            }
        };
        var condicion = new Condicion
        {
            NombreColumna = "EcoCoins",
            Cantidad = 100
        };

        // Act
        var resultado = _condicionLogica.FiltrarCondicionSiCumple(condicion, partida);

        // Assert
        Assert.NotNull(resultado);
        Assert.Empty(resultado);
    }

    [Fact]
    public void FiltrarCondicionesSiCumplen_MultiplesCondiciones_RetornaSoloLasQueCumplen()
    {
        // Arrange
        var partida = new Partida
        {
            Recursos = new Recurso
            {
                EcoCoins = 150,
                Felicidad = 30
            }
        };
        var condiciones = new List<Condicion>
        {
            new Condicion { NombreColumna = "EcoCoins", Cantidad = 100 }, // Cumple
            new Condicion { NombreColumna = "Felicidad", Cantidad = 50 }, // No cumple
            new Condicion { NombreColumna = "EcoCoins", Cantidad = 200 }  // No cumple
        };

        // Act
        var resultado = _condicionLogica.FiltrarCondicionesSiCumplen(condiciones, partida);

        // Assert
        Assert.NotNull(resultado);
        Assert.Single(resultado);
        Assert.Equal("EcoCoins", resultado[0].NombreColumna);
    }

    [Fact]
    public void FiltrarCondicionesSiCumplen_CondicionPorEstructura_Cumple()
    {
        // Arrange
        var estructura = new Estructura { Id = 1 };
        var estructuraMapa = new List<EstructuraMapa>
        {
            new EstructuraMapa { EstructuraId = 1 },
            new EstructuraMapa { EstructuraId = 1 },
            new EstructuraMapa { EstructuraId = 1 }
        };
        var partida = new Partida
        {
            EstructuraMapa = estructuraMapa
        };
        var condicion = new Condicion
        {
            EstructuraId = 1,
            Cantidad = 2
        };

        // Act
        var resultado = _condicionLogica.FiltrarCondicionSiCumple(condicion, partida);

        // Assert
        Assert.NotNull(resultado);
        Assert.Single(resultado);
    }

    [Fact]
    public void FiltrarCondicionesSiCumplen_PartidaSinRecursos_LanzaExcepcion()
    {
        // Arrange
        var partida = new Partida
        {
            Recursos = null
        };
        var condicion = new Condicion
        {
            NombreColumna = "EcoCoins",
            Cantidad = 100
        };

        // Act & Assert
        Assert.Throws<CondicionExcepcion>(() => _condicionLogica.FiltrarCondicionSiCumple(condicion, partida));
    }

    [Fact]
    public void FiltrarCondicionesSiCumplen_PartidaSinEstructuraMapa_LanzaExcepcion()
    {
        // Arrange
        var partida = new Partida
        {
            EstructuraMapa = null
        };
        var condicion = new Condicion
        {
            EstructuraId = 1,
            Cantidad = 2
        };

        // Act & Assert
        Assert.Throws<CondicionExcepcion>(() => _condicionLogica.FiltrarCondicionSiCumple(condicion, partida));
    }

    [Fact]
    public async Task Crear_CondicionSinEstructuraNiColumna_LanzaExcepcion()
    {
        // Arrange
        var condicionNueva = new Condicion
        {
            NombreColumna = null,
            EstructuraId = null,
            Cantidad = 100
        };

        // Act & Assert
        await Assert.ThrowsAsync<CondicionExcepcion>(() => _condicionLogica.Crear(condicionNueva));
    }

    [Fact]
    public async Task Crear_CondicionConEstructuraYColumna_LanzaExcepcion()
    {
        // Arrange
        var condicionNueva = new Condicion
        {
            NombreColumna = "EcoCoins",
            EstructuraId = 1,
            Cantidad = 100
        };

        // Act & Assert
        await Assert.ThrowsAsync<CondicionExcepcion>(() => _condicionLogica.Crear(condicionNueva));
    }

    [Fact]
    public async Task Crear_CondicionConColumnaInvalida_LanzaExcepcion()
    {
        // Arrange
        var condicionNueva = new Condicion
        {
            NombreColumna = "ColumnaInvalida",
            Cantidad = 100
        };

        // Act & Assert
        await Assert.ThrowsAsync<CondicionExcepcion>(() => _condicionLogica.Crear(condicionNueva));
    }

    [Fact]
    public async Task Crear_CondicionConCantidadNegativa_LanzaExcepcion()
    {
        // Arrange
        var condicionNueva = new Condicion
        {
            NombreColumna = "EcoCoins",
            Cantidad = -10
        };

        // Act & Assert
        await Assert.ThrowsAsync<CondicionExcepcion>(() => _condicionLogica.Crear(condicionNueva));
    }

    [Fact]
    public async Task CrearRecompensa_ConEstructura_CreaRecompensa()
    {
        // Arrange
        var estructura = new Estructura { Id = 1 };
        var recompensaNueva = new Condicion
        {
            EstructuraId = 1,
            Cantidad = 5
        };

        _mockEstructuraRepositorio.Setup(r => r.ObtenerPorId(1))
            .ReturnsAsync(estructura);
        _mockCondicionRepositorio.Setup(r => r.Agregar(It.IsAny<Condicion>()))
            .Returns(Task.CompletedTask);

        // Act
        await _condicionLogica.CrearRecompensa(recompensaNueva);

        // Assert
        _mockCondicionRepositorio.Verify(r => r.Agregar(It.Is<Condicion>(c => 
            c.EsRecompensa == true
        )), Times.Once);
        _mockUow.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task Actualizar_ConEstructura_ActualizaCorrectamente()
    {
        // Arrange
        const int id = 1;
        var estructura = new Estructura { Id = 1 };
        var condicionExistente = new Condicion
        {
            Id = id,
            Cantidad = 50,
            NombreColumna = "EcoCoins"
        };
        var condicionActualizada = new Condicion
        {
            EstructuraId = 1,
            Cantidad = 5
        };

        _mockCondicionRepositorio.Setup(r => r.ObtenerPorId(id))
            .ReturnsAsync(condicionExistente);
        _mockEstructuraRepositorio.Setup(r => r.ObtenerPorId(1))
            .ReturnsAsync(estructura);
        _mockCondicionRepositorio.Setup(r => r.Actualizar(It.IsAny<Condicion>()))
            .Returns(Task.CompletedTask);

        // Act
        await _condicionLogica.Actualizar(condicionActualizada, id);

        // Assert
        _mockEstructuraRepositorio.Verify(r => r.ObtenerPorId(1), Times.Once);
        _mockCondicionRepositorio.Verify(r => r.Actualizar(It.IsAny<Condicion>()), Times.Once);
        _mockUow.Verify(u => u.CommitAsync(), Times.Once);
    }
}


