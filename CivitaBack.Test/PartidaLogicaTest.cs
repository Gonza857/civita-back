using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CivitaBack.Data.BO;
using CivitaBack.Data.DTO;
using CivitaBack.Data.Enum;
using CivitaBack.Data.Repositorio;
using CivitaBack.Logica;
using CivitaBack.Logica.Excepciones;
using Moq;

namespace CivitaBack.Tests;

public class PartidaLogicaTest
{
    private readonly Mock<IPartidaRepositorio> _mockPartidaRepositorio;
    private readonly Mock<IRecursoLogica> _mockRecurso;
    private readonly Mock<IEstructuraMapaRepositorio> _mockEstructuraMapaRepositorio;

    private readonly IPartidaLogica _partidaLogica;
    private readonly IRecursoLogica _recursoLogica;

    public PartidaLogicaTest()
    {
        // Creamos los mocks de las dependencias
        _mockPartidaRepositorio = new Mock<IPartidaRepositorio>();
        _mockRecurso = new Mock<IRecursoLogica>();
        _mockEstructuraMapaRepositorio = new Mock<IEstructuraMapaRepositorio>();

        // Inyectamos los mocks en el constructor de PartidaLogica
        _partidaLogica = new PartidaLogica(
            _mockPartidaRepositorio.Object, 
            _mockRecurso.Object,
            _mockEstructuraMapaRepositorio.Object
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
            Felicidad = 100,
            Contaminacion = 100,
            Energia = 100,
            EcoCoins = 100,
            UsuarioId = 7,
        };

        _mockPartidaRepositorio.Setup(r => r.ObtenerPorUsuarioId(7))
         .ReturnsAsync(partidaMock);

        // Act
        await _partidaLogica.Actualizar(partidaDTOMock, usuarioMock);
        
        // Assert
        Assert.Equal(partidaMock.Recursos.Energia, partidaDTOMock.Energia);
        Assert.Equal(partidaMock.Recursos.Felicidad, partidaDTOMock.Felicidad);
        Assert.Equal(partidaMock.Recursos.EcoCoins, partidaDTOMock.EcoCoins);
        Assert.Equal(partidaMock.Recursos.Contaminacion, partidaDTOMock.Contaminacion);
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
        PartidaDTO partidaDTOMock = null;

        _mockPartidaRepositorio.Setup(r => r.ObtenerPorUsuarioId(7))
         .ReturnsAsync(partidaMock);

        // Act & Assert
        await Assert.ThrowsAsync<PartidaExcepcion>(() => _partidaLogica.Actualizar(partidaDTOMock, usuarioMock));
    }

    [Fact]
    public async void Actualizar_CuandoNoSeEncuentraPartida_LanzaError()
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
            Felicidad = 100,
            Contaminacion = 100,
            Energia = 100,
            EcoCoins = 100,
            UsuarioId = 7,
        };

        _mockPartidaRepositorio.Setup(r => r.ObtenerPorUsuarioId(7))
         .ReturnsAsync((Partida)null);

        // Act & Assert
        await Assert.ThrowsAsync<PartidaExcepcion>(() => _partidaLogica.Actualizar(partidaDTOMock, usuarioMock));
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

        _mockPartidaRepositorio.Setup(r => r.ObtenerPorUsuarioId(7))
         .ReturnsAsync(partidaMock);

        // Act & Assert
        await Assert.ThrowsAsync<PartidaExcepcion>(() => _partidaLogica.Actualizar(partidaDTOMock, usuarioMock));
    }

    [Fact]
    public async Task GuardarMapa_SinEstructuras_OK()
    {
        // Arrange
        Partida partidaMock = new Partida
        {
            Id = 1,
            UsuarioId = 7
        };
        GuardarMapaDTO gmdto = new GuardarMapaDTO
        {
            Estructuras = new List<EstructuraMapaDTO>(),
            JsonMapa = "{}",
            PartidaId = partidaMock.Id
        };
        
        _mockPartidaRepositorio
            .Setup(r => r.ObtenerPartidaConMapaAsync(partidaMock.Id))
            .ReturnsAsync(partidaMock);

        // Act
        await _partidaLogica.ActualizarMapaDePartidaAsync(gmdto);

        // Assert
        _mockPartidaRepositorio.Verify(r => r.ObtenerPartidaConMapaAsync(partidaMock.Id), Times.Once);
        _mockPartidaRepositorio.Verify(r => r.ActualizarMapaAsync(partidaMock), Times.Once);
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
        EstructuraMapaDTO e1 = new EstructuraMapaDTO
        {
            EstructuraId = 1,
        };
        List<EstructuraMapaDTO> estructuras = new List<EstructuraMapaDTO>();
        estructuras.Add(e1);
        GuardarMapaDTO gmdto = new GuardarMapaDTO
        {
            Estructuras = estructuras,
            JsonMapa = "{}",
            PartidaId = partidaMock.Id
        };
        List<EstructuraMapa> estructurasMock = new List<EstructuraMapa>()
        {
            new EstructuraMapa{PartidaId = partidaMock.Id},
            new EstructuraMapa{PartidaId = partidaMock.Id},
            new EstructuraMapa{PartidaId = partidaMock.Id},
            new EstructuraMapa { PartidaId = partidaMock.Id }
        };
        
        _mockPartidaRepositorio
            .Setup(r => r.ObtenerPartidaConMapaAsync(partidaMock.Id))
            .ReturnsAsync(partidaMock);
        _mockPartidaRepositorio
            .Setup(r => r.ObtenerEstructurasDeUnMapa(partidaMock.Id))
            .ReturnsAsync(estructurasMock);

        // Act
        await _partidaLogica.ActualizarMapaDePartidaAsync(gmdto);

        // Assert
        _mockPartidaRepositorio.Verify(r => r.ObtenerPartidaConMapaAsync(partidaMock.Id), Times.Once);
        _mockPartidaRepositorio.Verify(r => r.ActualizarMapaAsync(partidaMock), Times.Once);
        _mockEstructuraMapaRepositorio.Verify(r => r.EliminarPorPartidaIdAsync(partidaMock.Id), Times.Once);
        _mockEstructuraMapaRepositorio.Verify(r => r.AgregarNuevas(It.IsAny<List<EstructuraMapa>>()), Times.Once);
        _mockEstructuraMapaRepositorio.Verify(r => r.GuardarCambios(), Times.Once);
    }

    [Fact]
    public async Task GuardarMapa_ConPartidaNull_Falla()
    {
        // Arrange

        // Act & Assert
        await Assert.ThrowsAsync<PartidaExcepcion>(() => _partidaLogica.ActualizarMapaDePartidaAsync(null));
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


