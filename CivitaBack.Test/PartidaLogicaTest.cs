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
    private readonly Mock<IPartidaRepositorio> _mockRepo;
    private readonly Mock<IRecursoLogica> _mockRecurso;
    private readonly IPartidaLogica _partidaLogica;

    public PartidaLogicaTest()
    {
        // Creamos los mocks de las dependencias
        _mockRepo = new Mock<IPartidaRepositorio>();
        _mockRecurso = new Mock<IRecursoLogica>();

        // Inyectamos los mocks en el constructor de PartidaLogica
        _partidaLogica = new PartidaLogica(_mockRepo.Object, _mockRecurso.Object);
    }

    [Fact]
    public void CrearPartida_ThrowError_Existente ()
    {
        // Arrange
        _mockRepo.Setup(r => r.ObtenerPorUsuarioId(7))
         .Returns(new Partida
         {
             Id = 0,
             UsuarioId = 7,
         });

        // Act & Assert
        var ex = Assert.Throws<PartidaExcepcion>(() => _partidaLogica.CrearPartida(7));
    }

    [Fact]
    public void CrearPartida_RetornaPartida_OK()
    {
        // Arrange
        _mockRepo.Setup(r => r.ObtenerPorUsuarioId(7))
         .Returns((Partida)null);
        _mockRepo.Setup(r => r.CrearPartida(It.IsAny<int>()))
         .Returns(new Partida
         {
             Id = 1,
             UsuarioId = 7
         });

        // Act
        Partida partida = _partidaLogica.CrearPartida(7);

        // Assert
        Assert.NotNull(partida); 
    }

    [Fact]
    public void Actualizar_SaleOK()
    {
        // Arrange
        Partida partidaMock = new Partida
        {
            Id = 1,
            UsuarioId = 7
        };
        List<Recurso> recursosMock = new List<Recurso>
        {
            new Recurso("Energía", 0, partidaMock),
            new Recurso("Felicidad", 0, partidaMock),
            new Recurso("EcoCoins", 0, partidaMock),
            new Recurso("Contaminación", 0, partidaMock)
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

        _mockRepo.Setup(r => r.ObtenerPorUsuarioId(7))
         .Returns(partidaMock);

        // Act
        _partidaLogica.Actualizar(partidaDTOMock, usuarioMock);

        var energia = partidaMock.Recursos
           .FirstOrDefault(r => r.Nombre == TipoRecurso.Energia.GetDescription());
        var felicidad = partidaMock.Recursos
            .FirstOrDefault(r => r.Nombre == TipoRecurso.Felicidad.GetDescription());
        var ecoCoins = partidaMock.Recursos
            .FirstOrDefault(r => r.Nombre == TipoRecurso.EcoCoins.GetDescription());
        var contaminacion = partidaMock.Recursos
            .FirstOrDefault(r => r.Nombre == TipoRecurso.Contaminacion.GetDescription());


        // Assert
        Assert.Equal(energia.Cantidad, partidaDTOMock.Energia);
        Assert.Equal(felicidad.Cantidad, partidaDTOMock.Felicidad);
        Assert.Equal(ecoCoins.Cantidad, partidaDTOMock.EcoCoins);
        Assert.Equal(contaminacion.Cantidad, partidaDTOMock.Contaminacion);
    }

    [Fact]
    public void Actualizar_CuandoPartidaYUsuarioSonNull_LanzaError()
    {
        // Arrange
        Partida partidaMock = new Partida
        {
            Id = 1,
            UsuarioId = 7
        };
        List<Recurso> recursosMock = new List<Recurso>
        {
            new Recurso("Energía", 0, partidaMock),
            new Recurso("Felicidad", 0, partidaMock),
            new Recurso("EcoCoins", 0, partidaMock),
            new Recurso("Contaminación", 0, partidaMock)
        };
        partidaMock.Recursos = recursosMock;
        Usuario usuarioMock = null;
        PartidaDTO partidaDTOMock = null;

        _mockRepo.Setup(r => r.ObtenerPorUsuarioId(7))
         .Returns(partidaMock);

        // Act & Assert
        var ex = Assert.Throws<ErrorInternoExcepction>(() => _partidaLogica.Actualizar(partidaDTOMock, usuarioMock));
    }

    [Fact]
    public void Actualizar_CuandoNoSeEncuentraPartida_LanzaError()
    {
        // Arrange
        Partida partidaMock = new Partida
        {
            Id = 1,
            UsuarioId = 7
        };
        List<Recurso> recursosMock = new List<Recurso>
        {
            new Recurso("Energía", 0, partidaMock),
            new Recurso("Felicidad", 0, partidaMock),
            new Recurso("EcoCoins", 0, partidaMock),
            new Recurso("Contaminación", 0, partidaMock)
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

        _mockRepo.Setup(r => r.ObtenerPorUsuarioId(7))
         .Returns((Partida)null);

        // Act & Assert
        var ex = Assert.Throws<ErrorInternoExcepction>(() => _partidaLogica.Actualizar(partidaDTOMock, usuarioMock));
    }

    [Fact]
    public void Actualizar_CuandoRecursosNegativos_LanzaError()
    {
        // Arrange
        Partida partidaMock = new Partida
        {
            Id = 1,
            UsuarioId = 7
        };
        List<Recurso> recursosMock = new List<Recurso>
        {
            new Recurso("Energía", 0, partidaMock),
            new Recurso("Felicidad", 0, partidaMock),
            new Recurso("EcoCoins", 0, partidaMock),
            new Recurso("Contaminación", 0, partidaMock)
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

        _mockRepo.Setup(r => r.ObtenerPorUsuarioId(7))
         .Returns(partidaMock);

        // Act & Assert
        var ex = Assert.Throws<PartidaExcepcion>(() => _partidaLogica.Actualizar(partidaDTOMock, usuarioMock));
    }
}


