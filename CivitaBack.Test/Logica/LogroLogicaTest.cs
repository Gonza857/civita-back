using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Logica;
using CivitaBack.Domain.Excepciones;
using CivitaBack.Utils;
using Moq;

namespace CivitaBack.Tests;

public class LogroLogicaTest
{
    private readonly Mock<IPartidaRepositorio> _mockPartidaRepositorio;
    private readonly Mock<IRecursoRepositorio> _mockRecursoRepositorio;
    private readonly Mock<IEstructuraMapaRepositorio> _mockEstructuraMapaRepositorio;
    private readonly Mock<ILogroRepositorio> _mockLogroRepositorio;
    private readonly Mock<ITipoLogroRepositorio> _mockTipoLogroRepositorio;
    private readonly Mock<ICondicionRepositorio> _mockCondicionRepositorio;
    private readonly Mock<ILogroPartidaRepositorio> _mockLogroPartidaRepositorio;

    private readonly IPartidaLogica _partidaLogica;
    private readonly IRecursoLogica _recursoLogica;
    private readonly ILogroLogica _logroLogica;

    private readonly Mock<IUnidadDeTrabajo> _mockUow;

    public LogroLogicaTest()
    {
        // Creamos los mocks de las dependencias
        _mockPartidaRepositorio = new Mock<IPartidaRepositorio>();
        _mockRecursoRepositorio = new Mock<IRecursoRepositorio>();
        _mockEstructuraMapaRepositorio = new Mock<IEstructuraMapaRepositorio>();
        _mockTipoLogroRepositorio = new Mock<ITipoLogroRepositorio>();
        _mockLogroRepositorio = new Mock<ILogroRepositorio>();
        _mockLogroPartidaRepositorio = new Mock<ILogroPartidaRepositorio>();
        _mockCondicionRepositorio = new Mock<ICondicionRepositorio>();
        _mockUow = new Mock<IUnidadDeTrabajo>();

        _logroLogica = new LogroLogica(
            _mockLogroRepositorio.Object,
            _mockTipoLogroRepositorio.Object,
            _mockCondicionRepositorio.Object,
            _mockLogroPartidaRepositorio.Object,
            _mockUow.Object
        );

        // Inyectamos los mocks en el constructor de PartidaLogica
        _partidaLogica = new PartidaLogica(
            _mockPartidaRepositorio.Object,
            _mockRecursoRepositorio.Object,
            _mockEstructuraMapaRepositorio.Object,
            _mockLogroRepositorio.Object,
            _mockUow.Object
        );
    }


    [Fact]
    public async Task Condicion_ValidarSiTieneLogrosDiponibles_RetornaListadoLogroVacio()
    {
        // Arrange
        Usuario usuario = TestData.CrearUsuarioBase();
        Partida partida = TestData.CrearPartida(usuario);
        TipoLogro tipoLogro = TestData.CrearTipoLogro("Recurso");
        Condicion condicion = TestData.CrearCondicion("Energia", 500);
        Logro logro1 = TestData.CrearLogro(1, tipoLogro, condicion, "Obtené 500 de energía");
        Logro logro2 = TestData.CrearLogro(2, tipoLogro, condicion, "Obtené 500 de energía");
        Logro logro3 = TestData.CrearLogro(3, tipoLogro, condicion, "Obtené 500 de energía");


        // Act
        var listado = await _logroLogica.ObtenerLogrosCumplidos(partida);

        // Assert
        Assert.Empty(listado);
    }

    [Fact]
    public async Task Condicion_ValidarSiTieneLogrosDiponibles_RetornaListadoLogroLleno()
    {
        // Arrange
        TipoLogro tipoLogro = TestData.CrearTipoLogro("Recurso");
        Condicion condicion1 = TestData.CrearCondicion("Energia", 500);
        Condicion condicion2 = TestData.CrearCondicion("EcoCoins", 500);
        Logro logro1 = TestData.CrearLogro(1, tipoLogro, condicion1, "Obtené 500 de energía");
        Logro logro2 = TestData.CrearLogro(2, tipoLogro, condicion2, "Obtené 500 de dinero");
        Usuario usuario = TestData.CrearUsuarioBase();
        Partida partida = TestData.CrearPartida(usuario);
        TestData.LlenarPartidaConLogros(partida, new List<Logro> { logro1, logro2 });

        // Act
        var listado = await _logroLogica.ObtenerLogrosCumplidos(partida);

        // Assert
        Assert.NotEmpty(listado);
    }

    [Fact]
    public async Task Condicion_RevisarSiCumpleAlgunLogroSegunSuSituacion_RetornaListadoLogroLlenoAsync()
    {
        // Arrange
        TipoLogro tipoLogro = TestData.CrearTipoLogro("Recurso");
        Condicion condicion1 = TestData.CrearCondicion("Energia", 500);
        Condicion condicion2 = TestData.CrearCondicion("EcoCoins", 500);
        Logro logro1 = TestData.CrearLogro(1, tipoLogro, condicion1, "Obtené 500 de energía");
        Logro logro2 = TestData.CrearLogro(2, tipoLogro, condicion2, "Obtené 500 de dinero");
        List<Logro> logrosDB = new List<Logro> { logro1, logro2 };
        Usuario usuario = TestData.CrearUsuarioBase();
        Partida partida = TestData.CrearPartida(usuario);
        partida.EstructuraMapa = new List<EstructuraMapa>();
        Recurso recurso = TestData.CrearRecurso(partida, 500, 0, 0, 0);
        partida.Recursos = recurso;

        TestData.LlenarPartidaConLogros(partida, new List<Logro> { logro1, logro2 });
        _mockLogroPartidaRepositorio
        .Setup(r => r.ObtenerLogrosParaReclamarQueNoEstenCumplidos(It.IsAny<List<int>>()))
        .ReturnsAsync(new List<Logro> { logro1 });

        // Act
        var listado = await _logroLogica.ComprobarSiCumpleAlgunLogro(partida, logrosDB);

        // Assert
        Assert.NotEmpty(listado);
        Assert.Single(listado);
        Assert.Equal(logro1.Id, listado[0].Id);
    }

    [Fact]
    public async Task Condicion_RevisarSiCumpleAlgunLogroSegunSuSituacion_RetornaListadoLogroVacioAsync()
    {
        // Arrange
        TipoLogro tipoLogro = TestData.CrearTipoLogro("Recurso");
        Condicion condicion1 = TestData.CrearCondicion("Energia", 500);
        Condicion condicion2 = TestData.CrearCondicion("EcoCoins", 500);
        Logro logro1 = TestData.CrearLogro(1, tipoLogro, condicion1, "Obtené 500 de energía");
        Logro logro2 = TestData.CrearLogro(2, tipoLogro, condicion2, "Obtené 500 de dinero");
        List<Logro> logrosDB = new List<Logro> { logro1, logro2 };
        Usuario usuario = TestData.CrearUsuarioBase();
        Partida partida = TestData.CrearPartida(usuario);
        partida.EstructuraMapa = new List<EstructuraMapa>();
        Recurso recurso = TestData.CrearRecurso(partida, 0, 0, 0, 0);
        partida.Recursos = recurso;

        _mockLogroPartidaRepositorio
        .Setup(r => r.ObtenerLogrosParaReclamarQueNoEstenCumplidos(It.IsAny<List<int>>()))
        .ReturnsAsync(new List<Logro>());

        // Act
        var listado = await _logroLogica.ComprobarSiCumpleAlgunLogro(partida, logrosDB);

        // Assert
        Assert.Empty(listado);
    }

    [Fact]
    public void Condicion_RevisarSiCumpleAlgunLogroSegunSuSituacion_FallaSinRecursos()
    {
        // Arrange
        TipoLogro tipoLogro = TestData.CrearTipoLogro("Recurso");
        Condicion condicion1 = TestData.CrearCondicion("Energia", 500);
        Condicion condicion2 = TestData.CrearCondicion("EcoCoins", 500);
        Logro logro1 = TestData.CrearLogro(1, tipoLogro, condicion1, "Obtené 500 de energía");
        Logro logro2 = TestData.CrearLogro(2, tipoLogro, condicion2, "Obtené 500 de dinero");
        List<Logro> logrosDB = new List<Logro> { logro1, logro2 };
        Usuario usuario = TestData.CrearUsuarioBase();
        Partida partida = TestData.CrearPartida(usuario);
        Recurso recurso = TestData.CrearRecurso(partida, 0, 0, 0, 0);
        // partida.Recursos = recurso;

        // Act & Assert
        Assert.ThrowsAsync<LogroExcepcion>(async () => await _logroLogica.ComprobarSiCumpleAlgunLogro(partida, logrosDB));
    }

    [Fact]
    public void Condicion_RevisarSiCumpleAlgunLogroSegunSuSituacion_FallaSinLogros()
    {
        // Arrange
        List<Logro> logrosDB = new List<Logro>();
        Usuario usuario = TestData.CrearUsuarioBase();
        Partida partida = TestData.CrearPartida(usuario);

        // Act & Assert
        Assert.ThrowsAsync<LogroExcepcion>(async () => await _logroLogica.ComprobarSiCumpleAlgunLogro(partida, logrosDB));
    }
}