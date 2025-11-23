using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Excepciones;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Logica;
using CivitaBack.Logica.Interfaces;
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
    private readonly Mock<IEstructuraRepositorio> _mockEstructuraRepositorio;

    private readonly IPartidaLogica _partidaLogica;
    private readonly IRecursoLogica _recursoLogica;
    private readonly ILogroLogica _logroLogica;

    private readonly Mock<IUnidadDeTrabajo> _mockUow;
    private readonly Mock<IAccesoUsuarios> _mockAccesoUsuarios;


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
        _mockEstructuraRepositorio = new Mock<IEstructuraRepositorio>();
        _mockUow = new Mock<IUnidadDeTrabajo>();
        _mockAccesoUsuarios = new Mock<IAccesoUsuarios>();

        _logroLogica = new LogroLogica(
            _mockLogroRepositorio.Object,
            _mockTipoLogroRepositorio.Object,
            _mockCondicionRepositorio.Object,
            _mockLogroPartidaRepositorio.Object,
            _mockUow.Object,
            _mockAccesoUsuarios.Object
        );

        // Inyectamos los mocks en el constructor de PartidaLogica
        _partidaLogica = new PartidaLogica(
            _mockPartidaRepositorio.Object,
            _mockRecursoRepositorio.Object,
            _mockLogroRepositorio.Object,
            _mockAccesoUsuarios.Object,
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

    [Fact]
    public async Task ObtenerListado_HayLogros_RetornaLista()
    {
        // Arrange
        var logrosMock = new List<Logro>
        {
            new Logro { Id = 1, Titulo = "Logro 1" },
            new Logro { Id = 2, Titulo = "Logro 2" }
        };

        _mockLogroRepositorio.Setup(r => r.ObtenerTodos())
            .ReturnsAsync(logrosMock);

        // Act
        var resultado = await _logroLogica.ObtenerListado();

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(2, resultado.Count);
        _mockLogroRepositorio.Verify(r => r.ObtenerTodos(), Times.Once);
    }

    [Fact]
    public async Task ObtenerPorId_LogroExiste_RetornaLogro()
    {
        // Arrange
        const int id = 1;
        var logroMock = new Logro
        {
            Id = id,
            Titulo = "Test Logro"
        };

        _mockLogroRepositorio.Setup(r => r.ObtenerPorId(id))
            .ReturnsAsync(logroMock);

        // Act
        var resultado = await _logroLogica.ObtenerPorId(id);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(id, resultado.Id);
        _mockLogroRepositorio.Verify(r => r.ObtenerPorId(id), Times.Once);
    }

    [Fact]
    public async Task ObtenerPorId_LogroNoExiste_LanzaExcepcion()
    {
        // Arrange
        const int id = 999;

        _mockLogroRepositorio.Setup(r => r.ObtenerPorId(id))
            .ReturnsAsync((Logro?)null);

        // Act & Assert
        await Assert.ThrowsAsync<LogroExcepcion>(() => _logroLogica.ObtenerPorId(id));
    }

    [Fact]
    public async Task Crear_LogroValido_CreaLogro()
    {
        // Arrange
        var tipoLogro = new TipoLogro { Id = 1, Nombre = "Tipo Test" };
        var condicion = new Condicion { Id = 1, NombreColumna = "EcoCoins", Cantidad = 100 };
        var logroNuevo = new Logro
        {
            Titulo = "Nuevo Logro",
            Descripcion = "Descripción",
            TipoLogroId = tipoLogro.Id,
            CondicionId = condicion.Id
        };

        _mockAccesoUsuarios.Setup(a => a.EsDios()).Returns(true);
        _mockTipoLogroRepositorio.Setup(r => r.ObtenerPorId(1))
            .ReturnsAsync(tipoLogro);
        _mockCondicionRepositorio.Setup(r => r.ObtenerPorId(1))
            .ReturnsAsync(condicion);
        _mockLogroRepositorio.Setup(r => r.Agregar(It.IsAny<Logro>()))
            .Returns(Task.CompletedTask);
        _mockUow.Setup(u => u.CommitAsync()).ReturnsAsync(1);

        // Act
        await _logroLogica.Crear(logroNuevo);

        // Assert
        _mockTipoLogroRepositorio.Verify(r => r.ObtenerPorId(1), Times.Once);
        _mockCondicionRepositorio.Verify(r => r.ObtenerPorId(1), Times.Once);
        _mockLogroRepositorio.Verify(r => r.Agregar(It.IsAny<Logro>()), Times.Once);
        _mockUow.Verify(u => u.CommitAsync(), Times.Once);
    }

    // [Fact]
    // public async Task Crear_NoEsAdmin_LanzaExcepcion()
    // {
    //     // Arrange
    //     var logroNuevo = new Logro { Titulo = "Test" };
    //
    //     _mockAccesoUsuarios.Setup(a => a.EsDios()).Returns(false);
    //
    //     // Act & Assert
    //     await Assert.ThrowsAsync<AccesoDenegadoExcepcion>(() => _logroLogica.Crear(logroNuevo));
    //     _mockLogroRepositorio.Verify(r => r.Agregar(It.IsAny<Logro>()), Times.Never);
    // }

    [Fact]
    public async Task Crear_LogroNull_LanzaExcepcion()
    {
        // Arrange
        _mockAccesoUsuarios.Setup(a => a.EsDios()).Returns(true);

        // Act & Assert
        await Assert.ThrowsAsync<LogroExcepcion>(() => _logroLogica.Crear(null!));
    }

    [Fact]
    public async Task Crear_TipoLogroNoExiste_LanzaExcepcion()
    {
        // Arrange
        var logroNuevo = new Logro
        {
            TipoLogro = new TipoLogro { Id = 999 },
            Condicion = new Condicion { Id = 1 }
        };

        _mockAccesoUsuarios.Setup(a => a.EsDios()).Returns(true);
        _mockTipoLogroRepositorio.Setup(r => r.ObtenerPorId(999))
            .ReturnsAsync((TipoLogro?)null);

        // Act & Assert
        await Assert.ThrowsAsync<LogroExcepcion>(() => _logroLogica.Crear(logroNuevo));
    }

    [Fact]
    public async Task Crear_CondicionNoExiste_LanzaExcepcion()
    {
        // Arrange
        var tipoLogro = new TipoLogro { Id = 1 };
        var logroNuevo = new Logro
        {
            TipoLogro = tipoLogro,
            Condicion = new Condicion { Id = 999 }
        };

        _mockAccesoUsuarios.Setup(a => a.EsDios()).Returns(true);
        _mockTipoLogroRepositorio.Setup(r => r.ObtenerPorId(1))
            .ReturnsAsync(tipoLogro);
        _mockCondicionRepositorio.Setup(r => r.ObtenerPorId(999))
            .ReturnsAsync((Condicion?)null);

        // Act & Assert
        await Assert.ThrowsAsync<LogroExcepcion>(() => _logroLogica.Crear(logroNuevo));
    }

    [Fact]
    public async Task Actualizar_LogroValido_ActualizaLogro()
    {
        // Arrange
        const int id = 1;
        var tipoLogro = new TipoLogro { Id = 1, Nombre = "Tipo Test" };
        var logroExistente = new Logro
        {
            Id = id,
            Titulo = "Logro Original",
            TipoLogro = tipoLogro
        };
        var logroActualizado = new Logro
        {
            Titulo = "Logro Actualizado",
            Descripcion = "Nueva Descripción",
            TipoLogro = tipoLogro
        };

        _mockAccesoUsuarios.Setup(a => a.EsDios()).Returns(true);
        _mockLogroRepositorio.Setup(r => r.ObtenerPorId(id))
            .ReturnsAsync(logroExistente);
        _mockTipoLogroRepositorio.Setup(r => r.ObtenerPorId(1))
            .ReturnsAsync(tipoLogro);
        _mockLogroRepositorio.Setup(r => r.Actualizar(It.IsAny<Logro>()))
            .Returns(Task.CompletedTask);
        _mockUow.Setup(u => u.CommitAsync()).ReturnsAsync(1);

        // Act
        await _logroLogica.Actualizar(logroActualizado, id);

        // Assert
        _mockLogroRepositorio.Verify(r => r.ObtenerPorId(id), Times.Once);
        _mockTipoLogroRepositorio.Verify(r => r.ObtenerPorId(1), Times.Once);
        _mockLogroRepositorio.Verify(r => r.Actualizar(It.IsAny<Logro>()), Times.Once);
        _mockUow.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task Actualizar_NoEsAdmin_LanzaExcepcion()
    {
        // Arrange
        var logroActualizado = new Logro { Titulo = "Test" };

        _mockAccesoUsuarios.Setup(a => a.EsDios()).Returns(false);

        // Act & Assert
        await Assert.ThrowsAsync<AccesoDenegadoExcepcion>(() => _logroLogica.Actualizar(logroActualizado, 1));
    }

    [Fact]
    public async Task Actualizar_LogroNoExiste_LanzaExcepcion()
    {
        // Arrange
        var logroActualizado = new Logro 
        { 
            Titulo = "Test",
            TipoLogro = new TipoLogro { Id = 1 }
        };

        _mockAccesoUsuarios.Setup(a => a.EsDios()).Returns(true);
        _mockLogroRepositorio.Setup(r => r.ObtenerPorId(1))
            .ReturnsAsync((Logro?)null);
        // Mockeamos que TipoLogro existe para evitar NullReferenceException
        _mockTipoLogroRepositorio.Setup(r => r.ObtenerPorId(1))
            .ReturnsAsync(new TipoLogro { Id = 1 });

        // Act & Assert
        // El código verifica si logroBuscado es null después de obtener TipoLogro
        // y lanza LogroExcepcion cuando logroBuscado es null
        await Assert.ThrowsAsync<LogroExcepcion>(() => _logroLogica.Actualizar(logroActualizado, 1));
    }

    [Fact]
    public async Task Eliminar_IdValido_EliminaLogro()
    {
        // Arrange
        const int id = 1;

        _mockAccesoUsuarios.Setup(a => a.EsDios()).Returns(true);
        _mockLogroRepositorio.Setup(r => r.Eliminar(id))
            .Returns(Task.CompletedTask);
        _mockUow.Setup(u => u.CommitAsync()).ReturnsAsync(1);

        // Act
        await _logroLogica.Eliminar(id);

        // Assert
        _mockLogroRepositorio.Verify(r => r.Eliminar(id), Times.Once);
        _mockUow.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task Eliminar_IdInvalido_LanzaExcepcion()
    {
        // Arrange
        const int idInvalido = 0;

        _mockAccesoUsuarios.Setup(a => a.EsDios()).Returns(true);

        // Act & Assert
        await Assert.ThrowsAsync<LogroExcepcion>(() => _logroLogica.Eliminar(idInvalido));
        _mockLogroRepositorio.Verify(r => r.Eliminar(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task Eliminar_NoEsAdmin_LanzaExcepcion()
    {
        // Arrange
        const int id = 1;

        _mockAccesoUsuarios.Setup(a => a.EsDios()).Returns(false);

        // Act & Assert
        await Assert.ThrowsAsync<AccesoDenegadoExcepcion>(() => _logroLogica.Eliminar(id));
        _mockLogroRepositorio.Verify(r => r.Eliminar(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task ObtenerLogrosCumplidos_ConLogrosPartida_RetornaLogros()
    {
        // Arrange
        var usuario = new Usuario { Id = 1 };
        var partida = new Partida
        {
            Id = 1,
            UsuarioId = 1,
            LogroPartidas = new List<LogroPartida>
            {
                new LogroPartida { Logro = new Logro { Id = 1, Titulo = "Logro 1" } },
                new LogroPartida { Logro = new Logro { Id = 2, Titulo = "Logro 2" } }
            }
        };

        // Act
        var resultado = await _logroLogica.ObtenerLogrosCumplidos(partida);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(2, resultado.Count);
    }

    [Fact]
    public async Task ObtenerLogrosCumplidos_SinLogrosPartida_RetornaListaVacia()
    {
        // Arrange
        var partida = new Partida
        {
            Id = 1,
            UsuarioId = 1,
            LogroPartidas = new List<LogroPartida>()
        };

        _mockAccesoUsuarios.Setup(a => a.ValidarAcceso(1))
            .Verifiable();

        // Act
        var resultado = await _logroLogica.ObtenerLogrosCumplidos(partida);

        // Assert
        Assert.NotNull(resultado);
        Assert.Empty(resultado);
    }

    [Fact]
    public async Task ObtenerLogrosCumplidos_ConLogrosPartidaYRecursos_RetornaLogrosCumplidos()
    {
        // Arrange
        var partida = new Partida
        {
            Id = 1,
            UsuarioId = 1,
            Recursos = new Recurso
            {
                EcoCoins = 500,
                Felicidad = 100
            },
            LogroPartidas = new List<LogroPartida>
            {
                new LogroPartida
                {
                    Logro = new Logro
                    {
                        Id = 1,
                        Condicion = new Condicion { NombreColumna = "EcoCoins", Cantidad = 400 }
                    }
                },
                new LogroPartida
                {
                    Logro = new Logro
                    {
                        Id = 2,
                        Condicion = new Condicion { NombreColumna = "Felicidad", Cantidad = 150 }
                    }
                }
            }
        };

        _mockAccesoUsuarios.Setup(a => a.ValidarAcceso(1))
            .Verifiable();

        // Act
        var resultado = await _logroLogica.ObtenerLogrosCumplidos(partida);

        // Assert
        // Cuando hay logros partida, retorna todos los logros de logrosPartida
        Assert.NotNull(resultado);
        Assert.Equal(2, resultado.Count);
    }

    [Fact]
    public async Task ComprobarSiCumpleAlgunLogro_ConEstructura_Cumple()
    {
        // Arrange
        var usuario = new Usuario { Id = 1 };
        var partida = new Partida
        {
            Id = 1,
            UsuarioId = 1,
            Recursos = new Recurso { EcoCoins = 100 },
            EstructuraMapa = new List<EstructuraMapa>
            {
                new EstructuraMapa { EstructuraId = 1 },
                new EstructuraMapa { EstructuraId = 1 },
                new EstructuraMapa { EstructuraId = 1 }
            }
        };
        var logros = new List<Logro>
        {
            new Logro
            {
                Id = 1,
                Condicion = new Condicion { EstructuraId = 1, Cantidad = 2 }
            }
        };

        _mockLogroPartidaRepositorio.Setup(r => r.ObtenerLogrosParaReclamarQueNoEstenCumplidos(It.IsAny<List<int>>()))
            .ReturnsAsync(logros);

        // Act
        var resultado = await _logroLogica.ComprobarSiCumpleAlgunLogro(partida, logros);

        // Assert
        Assert.NotNull(resultado);
        Assert.Single(resultado);
    }

    [Fact]
    public async Task MarcarLogrosComoCompletados_LogrosValidos_MarcaCompletados()
    {
        // Arrange
        var usuario = new Usuario { Id = 1 };
        var partida = new Partida
        {
            Id = 1,
            UsuarioId = 1,
            Recursos = new Recurso { EcoCoins = 100 }
        };
        var logros = new List<Logro>
        {
            new Logro { Id = 1, Titulo = "Logro 1" },
            new Logro { Id = 2, Titulo = "Logro 2" }
        };

        _mockAccesoUsuarios.Setup(a => a.ValidarAcceso(1))
            .Verifiable();
        _mockLogroRepositorio.Setup(r => r.ExisteLogroEnCumplidos(1))
            .ReturnsAsync(false);
        _mockLogroRepositorio.Setup(r => r.ExisteLogroEnCumplidos(2))
            .ReturnsAsync(false);
        _mockLogroRepositorio.Setup(r => r.ObtenerPorId(1))
            .ReturnsAsync(logros[0]);
        _mockLogroRepositorio.Setup(r => r.ObtenerPorId(2))
            .ReturnsAsync(logros[1]);
        _mockLogroPartidaRepositorio.Setup(r => r.Agregar(It.IsAny<LogroPartida>()))
            .Returns(Task.CompletedTask);
        _mockUow.Setup(u => u.CommitAsync()).ReturnsAsync(1);

        // Act
        await _logroLogica.MarcarLogrosComoCompletados(partida, logros);

        // Assert
        _mockLogroRepositorio.Verify(r => r.ExisteLogroEnCumplidos(1), Times.Once);
        _mockLogroRepositorio.Verify(r => r.ExisteLogroEnCumplidos(2), Times.Once);
        _mockLogroPartidaRepositorio.Verify(r => r.Agregar(It.IsAny<LogroPartida>()), Times.Exactly(2));
        _mockUow.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task MarcarLogrosComoCompletados_LogroYaExiste_NoLoAgrega()
    {
        // Arrange
        var usuario = new Usuario { Id = 1 };
        var partida = new Partida
        {
            Id = 1,
            UsuarioId = 1,
            Recursos = new Recurso { EcoCoins = 100 }
        };
        var logros = new List<Logro>
        {
            new Logro { Id = 1, Titulo = "Logro 1" }
        };

        _mockAccesoUsuarios.Setup(a => a.ValidarAcceso(1))
            .Verifiable();
        _mockLogroRepositorio.Setup(r => r.ExisteLogroEnCumplidos(1))
            .ReturnsAsync(true);
        _mockUow.Setup(u => u.CommitAsync()).ReturnsAsync(1);

        // Act
        await _logroLogica.MarcarLogrosComoCompletados(partida, logros);

        // Assert
        _mockLogroRepositorio.Verify(r => r.ExisteLogroEnCumplidos(1), Times.Once);
        _mockLogroPartidaRepositorio.Verify(r => r.Agregar(It.IsAny<LogroPartida>()), Times.Never);
    }

    [Fact]
    public async Task MarcarLogrosComoCompletados_PartidaNull_LanzaExcepcion()
    {
        // Arrange
        var logros = new List<Logro> { new Logro { Id = 1 } };

        // Act & Assert
        await Assert.ThrowsAsync<LogroExcepcion>(() => 
            _logroLogica.MarcarLogrosComoCompletados(null!, logros));
    }

    [Fact]
    public async Task MarcarLogrosComoCompletados_ListaVacia_LanzaExcepcion()
    {
        // Arrange
        var partida = new Partida
        {
            Id = 1,
            UsuarioId = 1,
            Recursos = new Recurso { EcoCoins = 100 }
        };
        var logros = new List<Logro>();

        // Act & Assert
        await Assert.ThrowsAsync<LogroExcepcion>(() => 
            _logroLogica.MarcarLogrosComoCompletados(partida, logros));
    }

    [Fact]
    public async Task ObtenerLogrosParaObtenerRecompensa_PartidaValida_RetornaLogros()
    {
        // Arrange
        const int partidaId = 1;
        var logrosMock = new List<Logro>
        {
            new Logro { Id = 1, Titulo = "Logro 1" },
            new Logro { Id = 2, Titulo = "Logro 2" }
        };

        _mockLogroPartidaRepositorio.Setup(r => r.ObtenerLogrosNoCumplidos(partidaId))
            .ReturnsAsync(logrosMock);

        // Act
        var resultado = await _logroLogica.ObtenerLogrosParaObtenerRecompensa(partidaId);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(2, resultado.Count);
        _mockLogroPartidaRepositorio.Verify(r => r.ObtenerLogrosNoCumplidos(partidaId), Times.Once);
    }
}