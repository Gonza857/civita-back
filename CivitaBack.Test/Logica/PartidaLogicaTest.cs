using CivitaBack.Data.DTO;
using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Logica;
using CivitaBack.Domain.Excepciones;
using CivitaBack.Utils;
using Moq;

namespace CivitaBack.Tests;

public class PartidaLogicaTest
{
    private readonly Mock<IPartidaRepositorio> _mockPartidaRepositorio;
    private readonly Mock<IRecursoRepositorio> _mockRecursoRepositorio;
    private readonly Mock<IEstructuraMapaRepositorio> _mockEstructuraMapaRepositorio;
    private readonly Mock<ILogroRepositorio> _mockLogroRepositorio;

    private readonly IPartidaLogica _partidaLogica;
    private readonly IRecursoLogica _recursoLogica;

    private readonly Mock<IUnidadDeTrabajo> _mockUow;

    private readonly Mock<IEstructuraRepositorio> _mockEstructuraRepositorio;

    public PartidaLogicaTest()
    {
        // Creamos los mocks de las dependencias
        _mockPartidaRepositorio = new Mock<IPartidaRepositorio>();
        _mockRecursoRepositorio = new Mock<IRecursoRepositorio>();
        _mockEstructuraMapaRepositorio = new Mock<IEstructuraMapaRepositorio>();
        _mockLogroRepositorio = new Mock<ILogroRepositorio>();
        _mockEstructuraRepositorio = new Mock<IEstructuraRepositorio>(); 
        _mockUow = new Mock<IUnidadDeTrabajo>();

        // Inyectamos los mocks en el constructor de PartidaLogica
        _partidaLogica = new PartidaLogica(
            _mockPartidaRepositorio.Object,
            _mockRecursoRepositorio.Object,
            _mockEstructuraMapaRepositorio.Object,
            _mockLogroRepositorio.Object,
            _mockEstructuraRepositorio.Object, 
            _mockUow.Object
        );
    }

    [Fact]
    public async void CrearPartida_ThrowError_Existente()
    {
        // Arrange
        _mockPartidaRepositorio.Setup(r => r.ObtenerPorUsuarioId(7))
         .ReturnsAsync(new Partida
         {
             Id = 0,
             UsuarioId = 7,
         });

        // Act & Assert
        var ex = await Assert.ThrowsAsync<PartidaExcepcion>(() => _partidaLogica.CrearPartida(7));
    }

    [Fact]
    public async void CrearPartida_RetornaPartida_OK()
    {
        // Arrange
        _mockPartidaRepositorio.Setup(r => r.ObtenerPorUsuarioId(7))
         .ReturnsAsync((Partida)null);
        _mockPartidaRepositorio.Setup(r => r.CrearPartida(It.IsAny<int>()))
         .ReturnsAsync(new Partida
         {
             Id = 1,
             UsuarioId = 7
         });

        // Act
        Partida partida = await _partidaLogica.CrearPartida(7);

        // Assert
        Assert.NotNull(partida);
    }

    [Fact]
    public async void Actualizar_SaleOK()
    {
        // Arrange
        Usuario usuarioMock = new Usuario
        {
            Id = 7,
        };


        Partida partidaDominioActualizada = new Partida
        {
            UsuarioId = 7,
            Recursos = new Recurso
            {
                Felicidad = 100,
                Contaminacion = 100,
                Energia = 100,
                EcoCoins = 100,
            },
        };

        Partida partidaDBExistente = new Partida
        {
            Id = 1,
            UsuarioId = 7,
            Recursos = new Recurso { EcoCoins = 0, Contaminacion = 0, Energia = 0, Felicidad = 0 }
        };

        _mockPartidaRepositorio.Setup(r => r.ObtenerPorUsuarioId(7))
         .ReturnsAsync(partidaDBExistente);

        // Act
        await _partidaLogica.Actualizar(partidaDominioActualizada, usuarioMock);

        // Assert
        _mockPartidaRepositorio.Verify(
        r => r.Actualizar(
            // Utilizamos It.Is<T> para verificar el estado FINAL de la entidad que se pasó
            // al repositorio para ser marcada como 'Updated'.
            It.Is<Partida>(p =>
                p.Recursos!.Energia == 100 &&
                p.Recursos.Felicidad == 100 &&
                p.Recursos.EcoCoins == 100
            )
            ),
        Times.Once
        );

        _mockUow.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async void Actualizar_CuandoPartidaYUsuarioSonNull_LanzaError()
    {
        // Arrange
        Partida partidaMock = new Partida
        {
            Id = 1,
            UsuarioId = 7
        };
        Recurso recursosMock = new Recurso
        {
            Partida = partidaMock,
            EcoCoins = 0,
            Contaminacion = 0,
            Energia = 0,
            Felicidad = 0,
            Poblacion = 0,
        };
        partidaMock.Recursos = recursosMock;
        Usuario usuarioMock = null;
        Partida partidaDominioMock = null;

        _mockPartidaRepositorio.Setup(r => r.ObtenerPorUsuarioId(7))
         .ReturnsAsync(partidaMock);

        // Act & Assert
        await Assert.ThrowsAsync<PartidaExcepcion>(() => _partidaLogica.Actualizar(partidaDominioMock, usuarioMock));
    }

    [Fact]
    public async void Actualizar_CuandoNoSeEncuentraPartida_LanzaError()
    {
        // Arrange

        Usuario usuarioMock = new Usuario
        {
            Id = 7,
        };


        Partida partidaDominioActualizada = new Partida
        {
            UsuarioId = 7,
            Recursos = new Recurso
            {
                Felicidad = 100,
                Contaminacion = 100,
                Energia = 100,
                EcoCoins = 100,
            },
        };

        _mockPartidaRepositorio.Setup(r => r.ObtenerPorUsuarioId(7))
         .ReturnsAsync((Partida)null);

        // Act & Assert
        await Assert.ThrowsAsync<PartidaExcepcion>(() => _partidaLogica.Actualizar(partidaDominioActualizada, usuarioMock));
        _mockPartidaRepositorio.Verify(r => r.ObtenerPorUsuarioId(7), Times.Once);
        _mockUow.Verify(u => u.CommitAsync(), Times.Never());
    }

    [Fact]
    public async void Actualizar_CuandoRecursosNegativos_LanzaError()
    {
        // Arrange
        Partida partidaMock = new Partida
        {
            Id = 1,
            UsuarioId = 7
        };
        Recurso recursosMock = new Recurso
        {
            Partida = partidaMock,
            EcoCoins = 0,
            Contaminacion = 0,
            Energia = 0,
            Felicidad = 0,
            Poblacion = 0,
        };
        partidaMock.Recursos = recursosMock;
        Usuario usuarioMock = new Usuario
        {
            Id = 7,
        };
        PartidaDTO partidaDTOMock = new PartidaDTO
        {
            Felicidad = -50,
            Contaminacion = -0,
            Energia = -2,
            EcoCoins = -1,
            UsuarioId = 7,
        };

        Partida partidaDominioActualizada = new Partida
        {
            UsuarioId = partidaDTOMock.UsuarioId,
            Recursos = new Recurso
            {
                Felicidad = partidaDTOMock.Felicidad,
                Contaminacion = partidaDTOMock.Contaminacion,
                Energia = partidaDTOMock.Energia,
                EcoCoins = partidaDTOMock.EcoCoins,
            },
        };

        Partida partidaDBExistente = new Partida
        {
            Id = 1,
            UsuarioId = 7,
            Recursos = new Recurso { EcoCoins = 0, Contaminacion = 0, Energia = 0, Felicidad = 0 },
        };

        _mockPartidaRepositorio.Setup(r => r.ObtenerPorUsuarioId(7))
         .ReturnsAsync(partidaDBExistente);

        // Act & Assert
        await Assert.ThrowsAsync<PartidaExcepcion>(() => _partidaLogica.Actualizar(partidaDominioActualizada, usuarioMock));
        _mockUow.Verify(u => u.CommitAsync(), Times.Never());
    }

    [Fact]
    public async Task GuardarMapa_SinEstructuras_OK()
    {
        // Arrange
        Partida partidaMock = new Partida { Id = 1, UsuarioId = 7 };
        List<EstructuraMapa> estructurasVacias = new List<EstructuraMapa>();
        string nuevoJson = "{}";

        _mockPartidaRepositorio
            .Setup(r => r.ObtenerPartidaConMapaAsync(partidaMock.Id))
            .ReturnsAsync(partidaMock);

        // Act
        await _partidaLogica.ActualizarMapaDePartidaAsync(partidaMock.Id, nuevoJson, estructurasVacias);

        // Assert
        _mockPartidaRepositorio.Verify(r => r.ObtenerPartidaConMapaAsync(partidaMock.Id), Times.Once);
        _mockEstructuraMapaRepositorio.Verify(r => r.EliminarPorPartidaIdAsync(partidaMock.Id), Times.Never());
        _mockEstructuraMapaRepositorio.Verify(r => r.AgregarNuevas(It.IsAny<List<EstructuraMapa>>()), Times.Never());
        _mockPartidaRepositorio.Verify(r => r.ActualizarMapaAsync(
            It.Is<Partida>(p => p.JsonMapa == nuevoJson)
        ), Times.Once);
        _mockUow.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task GuardarMapa_ConEstructuras_OK()
    {
        // Arrange
        Partida partidaMock = new Partida
        {
            Id = 1,
            UsuarioId = 7
        };
        EstructuraMapa e1 = new EstructuraMapa { EstructuraId = 1, PartidaId = partidaMock.Id };

        List<EstructuraMapa> estructurasNuevas = new List<EstructuraMapa> { e1 };

        _mockPartidaRepositorio
            .Setup(r => r.ObtenerPartidaConMapaAsync(partidaMock.Id))
            .ReturnsAsync(partidaMock);

        // Act
        await _partidaLogica.ActualizarMapaDePartidaAsync(partidaMock.Id, "{}", estructurasNuevas);

        // Assert
        _mockPartidaRepositorio.Verify(r => r.ObtenerPartidaConMapaAsync(partidaMock.Id), Times.Once);
        _mockEstructuraMapaRepositorio.Verify(r => r.EliminarPorPartidaIdAsync(partidaMock.Id), Times.Once);
        _mockEstructuraMapaRepositorio.Verify(r => r.AgregarNuevas(
                It.Is<List<EstructuraMapa>>(list => list.Count == 1)
            ), Times.Once);
        _mockPartidaRepositorio.Verify(r => r.ActualizarMapaAsync(
        It.Is<Partida>(p => p.JsonMapa == "{}")
    ), Times.Once);
        _mockUow.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task GuardarMapa_ConPartidaNull_Falla()
    {
        // Arrange

        // Act & Assert
        await Assert.ThrowsAsync<PartidaExcepcion>(() => _partidaLogica.ActualizarMapaDePartidaAsync(0, null, null));
    }

    [Fact]
    public async Task ObtenerMapa_OK()
    {
        // Arrange
        Partida partidaMock = new Partida
        {
            Id = 1,
            UsuarioId = 7
        };
        _mockPartidaRepositorio
            .Setup(r => r.ObtenerPartidaConMapaAsync(partidaMock.Id))
            .ReturnsAsync(partidaMock);

        // Act 
        Partida p = await _partidaLogica.ObtenerMapaAsync(partidaMock.Id);

        // Assert
        Assert.NotNull(p);
    }

    [Fact]
    public async Task ObtenerMapa_ConIdInexistente_RetornaNull()
    {
        // Arrange
        Partida partidaMock = new Partida
        {
            Id = 1,
            UsuarioId = 7
        };
        _mockPartidaRepositorio
            .Setup(r => r.ObtenerPartidaConMapaAsync(partidaMock.Id))
            .ReturnsAsync((Partida?)null);

        // Act 
        Partida? p = await _partidaLogica.ObtenerMapaAsync(partidaMock.Id);

        // Assert
        Assert.Null(p);
    }
}


