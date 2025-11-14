using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Excepciones;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Logica;
using CivitaBack.Logica.Interfaces;
using CivitaBack.Utils;
using Microsoft.Extensions.Logging;
using Moq;

namespace CivitaBack.Tests;

public class LogroPartidaLogicaTest
{
    // --- NUEVOS MOCKS REQUERIDOS ---
    private readonly Mock<ILogroPartidaRepositorio> _mockLogroPartidaRepo;
    private readonly Mock<IRecursoRepositorio> _mockRecursoRepo;
    private readonly Mock<IUnidadDeTrabajo> _mockUow;
    private readonly Mock<ILogger<LogroPartidaLogica>> _mockLogger;

    private readonly ILogroPartidaLogica _logroPartidaLogica;

    private readonly Mock<IAccesoUsuarios> _mockAccesoUsuarios;

    public LogroPartidaLogicaTest()
    {
        // Creamos TODOS los mocks de las dependencias
        _mockLogroPartidaRepo = new Mock<ILogroPartidaRepositorio>();
        _mockRecursoRepo = new Mock<IRecursoRepositorio>();
        _mockUow = new Mock<IUnidadDeTrabajo>();
        _mockLogger = new Mock<ILogger<LogroPartidaLogica>>(); // Logger
        _mockAccesoUsuarios = new Mock<IAccesoUsuarios>();

        // Inyectamos todos los mocks en la clase de lógica (SUT)
        _logroPartidaLogica = new LogroPartidaLogica(
            _mockLogroPartidaRepo.Object,
            _mockRecursoRepo.Object,
            _mockUow.Object,
            _mockLogger.Object,
            _mockAccesoUsuarios.Object
        );

        // --- Mock por defecto para el Unit of Work ---
        // Para evitar que los tests fallen, simulamos que el Commit siempre funciona.
        _mockUow.Setup(u => u.CommitAsync()).ReturnsAsync(1);
    }

    // --- TESTS EXISTENTES (AÚN VÁLIDOS) ---

    [Fact]
    public async Task Logros_ObtenerCompletados_OK()
    {
        // Arrange
        Usuario usuario = TestData.CrearUsuarioBase();
        Partida partida = TestData.CrearPartida(usuario);
        partida.Recursos = new Recurso();
        partida.EstructuraMapa = new List<EstructuraMapa>();

        TipoLogro tipoLogro = TestData.CrearTipoLogro("Recurso");
        Condicion condicion = TestData.CrearCondicion("Energia", 500);
        Logro logro1 = TestData.CrearLogro(1, tipoLogro, condicion, "Obtené 500 de energía");
        List<Logro> logrosDb = new List<Logro> { logro1 };

        _mockLogroPartidaRepo
            .Setup(l => l.ObtenerLogrosCompletos(partida.Id))
            .ReturnsAsync(logrosDb);

        // Act
        var listado = await _logroPartidaLogica.ObtenerLogrosCompletados(partida);

        // Assert
        Assert.Single(listado);
    }

    [Fact]
    public async Task Logros_ObtenerIncompletos_OK()
    {
        // Arrange
        Usuario usuario = TestData.CrearUsuarioBase();
        Partida partida = TestData.CrearPartida(usuario);
        partida.Recursos = new Recurso();
        partida.EstructuraMapa = new List<EstructuraMapa>();

        TipoLogro tipoLogro = TestData.CrearTipoLogro("Recurso");
        Condicion condicion = TestData.CrearCondicion("Energia", 500);
        Logro logro1 = TestData.CrearLogro(1, tipoLogro, condicion, "Obtené 500 de energía");
        List<Logro> logrosDb = new List<Logro> { logro1 };

        _mockLogroPartidaRepo
            .Setup(l => l.ObtenerLogrosIncompletos(partida.Id))
            .ReturnsAsync(logrosDb);

        // Act
        var listado = await _logroPartidaLogica.ObtenerLogrosIncompletos(partida);

        // Assert
        Assert.Single(listado);
    }

    // --- TESTS PARA 'ObtenerLogrosParaReclamar' (Lógica de Recursos) ---

    [Fact]
    public async Task Logros_ObtenerParaReclamar_PorRecurso_RetornaUno()
    {
        // Arrange
        Usuario usuario = TestData.CrearUsuarioBase();
        Partida partida = TestData.CrearPartida(usuario);
        partida.Recursos = TestData.CrearRecurso(partida, 500, 0, 0, 0); // Tiene 500 de Energía
        partida.EstructuraMapa = new List<EstructuraMapa>();

        TipoLogro tipoLogro = TestData.CrearTipoLogro("Recurso");
        Condicion condicion = TestData.CrearCondicion("Energia", 500); // Necesita 500
        Logro logro1 = TestData.CrearLogro(1, tipoLogro, condicion, "Obtené 500 de energía");
        List<Logro> logrosDb = new List<Logro> { logro1 };

        _mockLogroPartidaRepo
            .Setup(l => l.ObtenerLogrosIncompletos(partida.Id))
            .ReturnsAsync(logrosDb);

        // Act
        var listado = await _logroPartidaLogica.ObtenerLogrosParaReclamar(partida);

        // Assert
        Assert.Single(listado); // Cumple la condición
    }

    [Fact]
    public async Task Logros_ObtenerParaReclamar_PorRecurso_RetornaVacio()
    {
        // Arrange
        Usuario usuario = TestData.CrearUsuarioBase();
        Partida partida = TestData.CrearPartida(usuario);
        partida.Recursos = TestData.CrearRecurso(partida, 499, 0, 0, 0); // Tiene 499 de Energía
        partida.EstructuraMapa = new List<EstructuraMapa>();

        TipoLogro tipoLogro = TestData.CrearTipoLogro("Recurso");
        Condicion condicion = TestData.CrearCondicion("Energia", 500); // Necesita 500
        Logro logro1 = TestData.CrearLogro(1, tipoLogro, condicion, "Obtené 500 de energía");
        List<Logro> logrosDb = new List<Logro> { logro1 };

        _mockLogroPartidaRepo
            .Setup(l => l.ObtenerLogrosIncompletos(partida.Id))
            .ReturnsAsync(logrosDb);

        // Act
        var listado = await _logroPartidaLogica.ObtenerLogrosParaReclamar(partida);

        // Assert
        Assert.Empty(listado); // No cumple la condición
    }

    // --- TESTS PARA 'ObtenerLogrosParaReclamar' (Lógica de Estructuras) ---

    [Fact]
    public async Task Logros_ObtenerParaReclamar_PorEstructura_RetornaUno()
    {
        // Arrange
        Usuario usuario = TestData.CrearUsuarioBase();
        Partida partida = TestData.CrearPartida(usuario);
        partida.Recursos = TestData.CrearRecurso(partida, 0, 0, 0, 0);
        partida.EstructuraMapa = new List<EstructuraMapa>
         {
             new EstructuraMapa { EstructuraId = 5 }, // Tiene 2
             new EstructuraMapa { EstructuraId = 5 }
         };

        TipoLogro tipoLogro = TestData.CrearTipoLogro("Estructura");
        Condicion condicion = new Condicion { NombreColumna = null, EstructuraId = 5, Cantidad = 2 };
        Logro logro1 = TestData.CrearLogro(1, tipoLogro, condicion, "Construí 2 de X");
        List<Logro> logrosDb = new List<Logro> { logro1 };

        _mockLogroPartidaRepo
            .Setup(l => l.ObtenerLogrosIncompletos(partida.Id))
            .ReturnsAsync(logrosDb);

        // Act
        var listado = await _logroPartidaLogica.ObtenerLogrosParaReclamar(partida);

        // Assert
        Assert.Single(listado); // Cumple la condición
    }

    [Fact]
    public async Task Logros_ObtenerParaReclamar_PorEstructura_RetornaVacio()
    {
        // Arrange
        Usuario usuario = TestData.CrearUsuarioBase();
        Partida partida = TestData.CrearPartida(usuario);
        partida.Recursos = TestData.CrearRecurso(partida, 0, 0, 0, 0);
        partida.EstructuraMapa = new List<EstructuraMapa>
         {
             new EstructuraMapa { EstructuraId = 5 } // Tiene 1
         };

        TipoLogro tipoLogro = TestData.CrearTipoLogro("Estructura");
        Condicion condicion = new Condicion { NombreColumna = null, EstructuraId = 5, Cantidad = 2 }; // Necesita 2
        Logro logro1 = TestData.CrearLogro(1, tipoLogro, condicion, "Construí 2 de X");
        List<Logro> logrosDb = new List<Logro> { logro1 };

        _mockLogroPartidaRepo
            .Setup(l => l.ObtenerLogrosIncompletos(partida.Id))
            .ReturnsAsync(logrosDb);

        // Act
        var listado = await _logroPartidaLogica.ObtenerLogrosParaReclamar(partida);

        // Assert
        Assert.Empty(listado); // No cumple la condición
    }

    // --- TESTS DE EXCEPCIONES Y CASOS BORDE ---

    [Fact]
    public async Task Logros_ObtenerCompletados_ConPartidaNull_Falla()
    {
        // Arrange
        Partida? partida = null;
        // Act & Assert
        await Assert.ThrowsAsync<LogroPartidaExcepcion>(
            () => _logroPartidaLogica.ObtenerLogrosCompletados(partida)
        );
    }

    [Fact]
    public async Task Logros_ObtenerParaReclamar_ConRecursosNull_Falla()
    {
        // Arrange
        Usuario usuario = TestData.CrearUsuarioBase();
        Partida partida = TestData.CrearPartida(usuario);
        partida.Recursos = null; // Forzamos el estado inválido
        partida.EstructuraMapa = new List<EstructuraMapa>();

        // Act & Assert
        await Assert.ThrowsAsync<LogroPartidaExcepcion>(
            () => _logroPartidaLogica.ObtenerLogrosParaReclamar(partida)
        );
    }

    // --- TEST MODIFICADO (YA NO LANZA EXCEPCIÓN) ---
    [Fact]
    public async Task Logros_ObtenerParaReclamarConCondicionColumnaInvalida_RetornaVacio()
    {
        // Arrange
        Usuario usuario = TestData.CrearUsuarioBase();
        Partida partida = TestData.CrearPartida(usuario);
        partida.Recursos = TestData.CrearRecurso(partida, 1000, 0, 0, 0); // Tiene recursos de sobra
        partida.EstructuraMapa = new List<EstructuraMapa>();

        TipoLogro tipoLogro = TestData.CrearTipoLogro("Recurso");
        Condicion condicion = TestData.CrearCondicion("ColumnaInvalida", 500); // Columna Rota
        Logro logro1 = TestData.CrearLogro(1, tipoLogro, condicion, "Logro Roto");
        List<Logro> logrosDb = new List<Logro> { logro1 };

        _mockLogroPartidaRepo
            .Setup(l => l.ObtenerLogrosIncompletos(partida.Id))
            .ReturnsAsync(logrosDb);

        // Act
        var listado = await _logroPartidaLogica.ObtenerLogrosParaReclamar(partida);

        // Assert
        // Tu nuevo código (con TryGetValue) es robusto y no lanza excepción,
        // simplemente no añade el logro. La lista debe estar vacía.
        Assert.Empty(listado);
    }

    // --- 💎 NUEVOS TESTS PARA 'ReclamarLogros' 💎 ---

    [Fact]
    public async Task ReclamarLogros_CuandoNoHayLogros_NoLlamaAGuardar()
    {
        // Arrange
        Usuario usuario = TestData.CrearUsuarioBase();
        Partida partida = TestData.CrearPartida(usuario);
        partida.Recursos = TestData.CrearRecurso(partida, 0, 0, 0, 0);
        partida.EstructuraMapa = new List<EstructuraMapa>();

        // Simulamos que ObtenerLogrosIncompletos devuelve una lista vacía
        _mockLogroPartidaRepo
            .Setup(l => l.ObtenerLogrosIncompletos(partida.Id))
            .ReturnsAsync(new List<Logro>());

        // Act
        await _logroPartidaLogica.ReclamarLogros(partida);

        // Assert
        // Verificamos que NUNCA se intentó guardar nada
        _mockRecursoRepo.Verify(r => r.Actualizar(It.IsAny<Recurso>()), Times.Never);
        _mockLogroPartidaRepo.Verify(r => r.AgregarVarios(It.IsAny<List<LogroPartida>>()), Times.Never);
        _mockUow.Verify(u => u.CommitAsync(), Times.Never);
    }

    [Fact]
    public async Task ReclamarLogros_CuandoHayLogros_AplicaRecompensasYGuardaAtomicamente()
    {
        // Arrange
        int partidaId = 1;
        int logroId = 10;

        Usuario usuario = TestData.CrearUsuarioBase();
        Partida partida = TestData.CrearPartida(usuario);
        partida.Id = partidaId;
        partida.Recursos = TestData.CrearRecurso(partida, 500, 0, 0, 0); // 500 Energía, 0 Oro
        partida.EstructuraMapa = new List<EstructuraMapa>();

        // La recompensa (que es de tipo Condicion) da 100 de Oro
        Condicion recompensa = TestData.CrearCondicion("EcoCoins", 100);
        Condicion condicion = TestData.CrearCondicion("Energia", 500);
        condicion.Recompensa = recompensa; // Asignamos la recompensa

        TipoLogro tipoLogro = TestData.CrearTipoLogro("Recurso");
        Logro logro1 = TestData.CrearLogro(logroId, tipoLogro, condicion, "Logro de Energía");
        List<Logro> logrosDb = new List<Logro> { logro1 };

        // Simulamos que SÍ hay un logro para reclamar
        _mockLogroPartidaRepo
            .Setup(l => l.ObtenerLogrosIncompletos(partida.Id))
            .ReturnsAsync(logrosDb);

        // Act
        await _logroPartidaLogica.ReclamarLogros(partida);

        // Assert

        // 1. ¿Se aplicó la recompensa? (El Oro subió a 100)
        Assert.Equal(100, partida.Recursos.EcoCoins);

        // 2. ¿Se preparó la actualización de Recursos UNA VEZ?
        _mockRecursoRepo.Verify(
            r => r.Actualizar(partida.Recursos), // Verifica que fue el objeto Recurso modificado
            Times.Once
        );

        // 3. ¿Se preparó el Agregado de LogroPartida UNA VEZ?
        _mockLogroPartidaRepo.Verify(
            r => r.AgregarVarios(It.Is<List<LogroPartida>>(lista =>
                lista.Count == 1 &&
                lista[0].LogroId == logroId &&
                lista[0].PartidaId == partidaId
            )),
            Times.Once
        );

        // 4. ¿Se llamó al Commit de Unit of Work UNA VEZ? (La transacción atómica)
        _mockUow.Verify(u => u.CommitAsync(), Times.Once);
    }

    // --- NUEVO TEST PARA 'ReiniciarLogros' ---

    [Fact]
    public async Task ReiniciarLogros_LlamaAlMetodoDelRepositorio()
    {
        // Arrange
        int partidaId = 99;

        // Act
        await _logroPartidaLogica.ReiniciarLogros(partidaId);

        // Assert
        // Verifica que se llamó al método específico del repositorio (caja negra)
        _mockLogroPartidaRepo.Verify(
            r => r.ReiniciarLogrosPartida(partidaId),
            Times.Once
        );

        // OJO: Este test asume que ReiniciarLogrosPartida HACE su propio SaveChanges.
        // Como te comenté, lo ideal sería que ReiniciarLogros orqueste el borrado
        // y llame a _unitOfWork.CommitAsync() él mismo.
    }
}