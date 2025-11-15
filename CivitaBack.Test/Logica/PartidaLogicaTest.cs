using CivitaBack.Data.DTO;
using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Excepciones;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Logica;
using CivitaBack.Logica.Interfaces;
using CivitaBack.Utils;
using Moq;
using System.Reflection;

namespace CivitaBack.Tests;

public class PartidaLogicaTest
{
    private readonly Mock<IPartidaRepositorio> _mockPartidaRepositorio;
    private readonly Mock<IRecursoRepositorio> _mockRecursoRepositorio;
    private readonly Mock<ILogroRepositorio> _mockLogroRepositorio;

    private readonly IPartidaLogica _partidaLogica;

    private readonly Mock<IAccesoUsuarios> _mockAccesoUsuarios;

    private readonly Mock<IUnidadDeTrabajo> _mockUow;

    public PartidaLogicaTest()
    {
        // Creamos los mocks de las dependencias
        _mockPartidaRepositorio = new Mock<IPartidaRepositorio>();
        _mockRecursoRepositorio = new Mock<IRecursoRepositorio>();
        _mockLogroRepositorio = new Mock<ILogroRepositorio>();
        _mockAccesoUsuarios = new Mock<IAccesoUsuarios>();
        _mockUow = new Mock<IUnidadDeTrabajo>();

        // Inyectamos los mocks en el constructor de PartidaLogica
        _partidaLogica = new PartidaLogica(
            rp: _mockPartidaRepositorio.Object,
            irr: _mockRecursoRepositorio.Object,
            ilr: _mockLogroRepositorio.Object,
            accesoUsuarios: _mockAccesoUsuarios.Object,
            uow: _mockUow.Object
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
        const int IdUsuario = 7;

        Usuario usuarioMock = new Usuario
        {
            Id = IdUsuario,
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

        _mockAccesoUsuarios.Setup(a => a.ObtenerIdUsuarioActual()).Returns(IdUsuario);
        _mockAccesoUsuarios.Setup(a => a.EsDios()).Returns(false);

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
        const int IdUsuario = 7;

        Usuario usuarioMock = new Usuario
        {
            Id = IdUsuario,
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

        _mockAccesoUsuarios.Setup(a => a.ObtenerIdUsuarioActual()).Returns(IdUsuario);
        _mockAccesoUsuarios.Setup(a => a.EsDios()).Returns(false);

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
    public async Task ObtenerPorId_AccesoDenegado_LanzaExcepcion()
    {
        // Arrange
        const int idUsuarioAutenticado = 100;
        const int idPropietarioPartida = 200;
        const int idPartida = 1;

        Partida partidaMock = new Partida { Id = idPartida, UsuarioId = idPropietarioPartida };

        // Simular que el usuario logueado NO es el dueño y NO es Dios
        _mockAccesoUsuarios.Setup(a => a.ObtenerIdUsuarioActual()).Returns(idUsuarioAutenticado);
        _mockAccesoUsuarios.Setup(a => a.EsDios()).Returns(false);

        // 🔑 MOCKEO PARA FORZAR LA EXCEPCIÓN:
        _mockAccesoUsuarios
            .Setup(a => a.ValidarAcceso(idPropietarioPartida)) // Cuando el ID 200 se pasa
            .Throws(new AccesoDenegadoExcepcion("Acceso denegado por test.")); 


        // Simular que el repositorio devuelve la partida ajena
        _mockPartidaRepositorio.Setup(r => r.ObtenerPorId(idPartida))
                               .ReturnsAsync(partidaMock);

        // Act & Assert

        var act = async () => await _partidaLogica.ObtenerPorId(idPartida);
        await Assert.ThrowsAsync<AccesoDenegadoExcepcion>(act);

        _mockAccesoUsuarios.Verify(a => a.ValidarAcceso(idPropietarioPartida), Times.Once);

        _mockPartidaRepositorio.Verify(r => r.ObtenerPorId(idPartida), Times.Once);
        _mockUow.Verify(u => u.CommitAsync(), Times.Never());
    }

    [Fact]
    public async Task ObtenerPorId_AccesoPermitido_RetornaPartida()
    {
        // Arrange
        const int idUsuario = 100;
        const int idPartida = 1;

        Partida partidaMock = new Partida { Id = idPartida, UsuarioId = idUsuario }; // Dueño = Logueado

        // Simular que el usuario logueado ES el dueño
        _mockAccesoUsuarios.Setup(a => a.ObtenerIdUsuarioActual()).Returns(idUsuario);
        _mockAccesoUsuarios.Setup(a => a.EsDios()).Returns(false);

        // Simular que el repositorio devuelve la partida propia
        _mockPartidaRepositorio.Setup(r => r.ObtenerPorId(idPartida))
                               .ReturnsAsync(partidaMock);

        // Act
        var resultado = await _partidaLogica.ObtenerPorId(idPartida);

        // Assert
        // La lógica no debe lanzar excepción y debe devolver el objeto
        Assert.NotNull(resultado);
        Assert.Equal(idUsuario, resultado.UsuarioId);
        _mockUow.Verify(u => u.CommitAsync(), Times.Never());
    }

    [Fact]
    public async Task ObtenerPorId_UsuarioDios_PermiteAccesoAjenos()
    {
        // Arrange
        const int idUsuarioDios = 999;
        const int idPropietarioPartida = 200; // Partida ajena
        const int idPartida = 50;

        Partida partidaMock = new Partida { Id = idPartida, UsuarioId = idPropietarioPartida };

        // Simular la identidad del usuario actual como DIOS
        _mockAccesoUsuarios.Setup(a => a.ObtenerIdUsuarioActual()).Returns(idUsuarioDios);
        _mockAccesoUsuarios.Setup(a => a.EsDios()).Returns(true); // 🔑 BYPASS DE DIOS

        // Simular el repositorio devolviendo la partida ajena
        _mockPartidaRepositorio.Setup(r => r.ObtenerPorId(idPartida))
                               .ReturnsAsync(partidaMock);

        // Act
        var resultado = await _partidaLogica.ObtenerPorId(idPartida);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(idPropietarioPartida, resultado.UsuarioId); // Accedió a la partida del ID 200
        _mockUow.Verify(u => u.CommitAsync(), Times.Never());
    }

    /*[Fact]
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
    }*/

    [Fact]
    public async Task ObtenerPartidas_NoEsAdmin_LanzaExcepcion()
    {
        // Arrange
        _mockAccesoUsuarios.Setup(a => a.EsDios()).Returns(false);

        // Act & Assert
        await Assert.ThrowsAsync<AccesoDenegadoExcepcion>(() => _partidaLogica.ObtenerPartidas());
        _mockPartidaRepositorio.Verify(r => r.ObtenerTodos(), Times.Never);
    }

    [Fact]
    public async Task ObtenerPartidas_EsAdmin_RetornaListaPartidas()
    {
        // Arrange
        var partidasMock = new List<Partida>
        {
            new Partida { Id = 1, UsuarioId = 1 },
            new Partida { Id = 2, UsuarioId = 2 }
        };

        _mockAccesoUsuarios.Setup(a => a.EsDios()).Returns(true);
        _mockPartidaRepositorio.Setup(r => r.ObtenerTodos())
            .ReturnsAsync(partidasMock);

        // Act
        var resultado = await _partidaLogica.ObtenerPartidas();

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(2, resultado.Count);
        _mockAccesoUsuarios.Verify(a => a.EsDios(), Times.Once);
        _mockPartidaRepositorio.Verify(r => r.ObtenerTodos(), Times.Once);
    }

    [Fact]
    public async Task ObtenerPartidaPorIdInterno_AccesoDenegado_LanzaExcepcion()
    {
        // Arrange
        const int idUsuarioPropietario = 200;
        const int idUsuarioAutenticado = 100; // Usuario diferente

        _mockAccesoUsuarios.Setup(a => a.ValidarAcceso(idUsuarioPropietario))
            .Throws(new AccesoDenegadoExcepcion("Acceso denegado"));

        // Act & Assert
        await Assert.ThrowsAsync<AccesoDenegadoExcepcion>(() => 
            _partidaLogica.ObtenerPartidaPorIdInterno(idUsuarioPropietario));
        _mockPartidaRepositorio.Verify(r => r.ObtenerPorUsuarioId(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task ObtenerPartidaPorIdInterno_AccesoPermitido_RetornaPartida()
    {
        // Arrange
        const int idUsuario = 100;
        var partidaMock = new Partida { Id = 1, UsuarioId = idUsuario };

        _mockAccesoUsuarios.Setup(a => a.ValidarAcceso(idUsuario))
            .Verifiable();
        _mockPartidaRepositorio.Setup(r => r.ObtenerPorUsuarioId(idUsuario))
            .ReturnsAsync(partidaMock);

        // Act
        var resultado = await _partidaLogica.ObtenerPartidaPorIdInterno(idUsuario);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(idUsuario, resultado.UsuarioId);
        _mockAccesoUsuarios.Verify(a => a.ValidarAcceso(idUsuario), Times.Once);
        _mockPartidaRepositorio.Verify(r => r.ObtenerPorUsuarioId(idUsuario), Times.Once);
    }

    [Fact]
    public async Task ObtenerPorUsuarioId_AccesoDenegado_LanzaExcepcion()
    {
        // Arrange
        const int idUsuarioPropietario = 200;
        const int idUsuarioAutenticado = 100; // Usuario diferente

        _mockAccesoUsuarios.Setup(a => a.ValidarAcceso(idUsuarioPropietario))
            .Throws(new AccesoDenegadoExcepcion("Acceso denegado"));

        // Act & Assert
        await Assert.ThrowsAsync<AccesoDenegadoExcepcion>(() => 
            _partidaLogica.ObtenerPorUsuarioId(idUsuarioPropietario));
        _mockPartidaRepositorio.Verify(r => r.ObtenerPorUsuarioId(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task ObtenerPorUsuarioId_AccesoPermitido_RetornaPartida()
    {
        // Arrange
        const int idUsuario = 100;
        var partidaMock = new Partida { Id = 1, UsuarioId = idUsuario };

        _mockAccesoUsuarios.Setup(a => a.ValidarAcceso(idUsuario))
            .Verifiable();
        _mockPartidaRepositorio.Setup(r => r.ObtenerPorUsuarioId(idUsuario))
            .ReturnsAsync(partidaMock);

        // Act
        var resultado = await _partidaLogica.ObtenerPorUsuarioId(idUsuario);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(idUsuario, resultado.UsuarioId);
        _mockAccesoUsuarios.Verify(a => a.ValidarAcceso(idUsuario), Times.Once);
        _mockPartidaRepositorio.Verify(r => r.ObtenerPorUsuarioId(idUsuario), Times.Once);
    }

    [Fact]
    public async Task ObtenerPorUsuarioId_PartidaNoExiste_LanzaExcepcion()
    {
        // Arrange
        const int idUsuario = 100;

        _mockAccesoUsuarios.Setup(a => a.ValidarAcceso(idUsuario))
            .Verifiable();
        _mockPartidaRepositorio.Setup(r => r.ObtenerPorUsuarioId(idUsuario))
            .ReturnsAsync((Partida?)null);

        // Act & Assert
        await Assert.ThrowsAsync<PartidaExcepcion>(() => 
            _partidaLogica.ObtenerPorUsuarioId(idUsuario));
        _mockAccesoUsuarios.Verify(a => a.ValidarAcceso(idUsuario), Times.Once);
    }

    [Fact]
    public async Task ReclamarLogros_AccesoDenegado_LanzaExcepcion()
    {
        // Arrange
        const int idUsuarioPropietario = 200;
        var partida = new Partida
        {
            Id = 1,
            UsuarioId = idUsuarioPropietario
        };
        var logros = new List<Logro>
        {
            new Logro { Id = 1, Titulo = "Logro 1" }
        };

        _mockAccesoUsuarios.Setup(a => a.ValidarAcceso(idUsuarioPropietario))
            .Throws(new AccesoDenegadoExcepcion("Acceso denegado"));

        // Act & Assert
        await Assert.ThrowsAsync<AccesoDenegadoExcepcion>(() => 
            _partidaLogica.ReclamarLogros(partida, logros));
        _mockLogroRepositorio.Verify(r => r.ObtenerTodos(), Times.Never);
    }

    [Fact]
    public async Task ReclamarLogros_AccesoPermitido_ReclamaLogros()
    {
        // Arrange
        const int idUsuario = 100;
        var partida = new Partida
        {
            Id = 1,
            UsuarioId = idUsuario
        };
        var logrosDto = new List<Logro>
        {
            new Logro { Id = 1, Titulo = "Logro 1" }
        };
        var logrosDb = new List<Logro>
        {
            new Logro
            {
                Id = 1,
                Titulo = "Logro 1",
                Condicion = new Condicion
                {
                    Recompensa = new Condicion
                    {
                        NombreColumna = "EcoCoins",
                        Cantidad = 50
                    }
                }
            }
        };
        var recurso = new Recurso
        {
            Id = 1,
            PartidaId = 1,
            EcoCoins = 100,
            Felicidad = 50,
            Energia = 50,
            Contaminacion = 50
        };

        _mockAccesoUsuarios.Setup(a => a.ValidarAcceso(idUsuario))
            .Verifiable();
        _mockLogroRepositorio.Setup(r => r.ObtenerTodos())
            .ReturnsAsync(logrosDb);
        _mockRecursoRepositorio.Setup(r => r.ObtenerPorId(1))
            .ReturnsAsync(recurso);
        _mockRecursoRepositorio.Setup(r => r.Actualizar(It.IsAny<Recurso>()))
            .Returns(Task.CompletedTask);
        _mockUow.Setup(u => u.CommitAsync()).ReturnsAsync(1);

        // Act
        await _partidaLogica.ReclamarLogros(partida, logrosDto);

        // Assert
        _mockAccesoUsuarios.Verify(a => a.ValidarAcceso(idUsuario), Times.Once);
        _mockLogroRepositorio.Verify(r => r.ObtenerTodos(), Times.Once);
        _mockRecursoRepositorio.Verify(r => r.ObtenerPorId(1), Times.Once);
        _mockRecursoRepositorio.Verify(r => r.Actualizar(It.IsAny<Recurso>()), Times.Once);
        _mockUow.Verify(u => u.CommitAsync(), Times.Once);
    }
}


