using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Excepciones;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Logica;
using CivitaBack.Utils;
using Moq;

namespace CivitaBack.Tests;

public class RecompensaLogicaTest
{
    private readonly IRecompensaLogica _recompensaLogica;

    private readonly Mock<IPartidaRepositorio> _mockPartidaRepositorio;
    private readonly Mock<IUnidadDeTrabajo> _mockUnidadDeTrabajo;
    private readonly Mock<IRecompensaRepositorio> _mockRecompensaRepositorio;
    private readonly Mock<IEstructuraRepositorio> _mockEstructuraRepositorio;


    public RecompensaLogicaTest()
    {
        _mockPartidaRepositorio = new Mock<IPartidaRepositorio>();
        _mockUnidadDeTrabajo = new Mock<IUnidadDeTrabajo>();
        _mockRecompensaRepositorio = new Mock<IRecompensaRepositorio>();
        _mockEstructuraRepositorio = new Mock<IEstructuraRepositorio>();
        
        _recompensaLogica = new RecompensaLogica(
            _mockPartidaRepositorio.Object,
            _mockUnidadDeTrabajo.Object,
            _mockRecompensaRepositorio.Object,
            _mockEstructuraRepositorio.Object
        );
    }

    [Fact]
    public void ReclamarExperiencia_Sale_OK()
    {
        // Arrange
        var usuario = TestData.CrearUsuarioBase();
        var partida = TestData.CrearPartida(usuario);
        partida.Recursos = TestData.CrearRecurso(partida, 500, 500, 500, 500);
        var recompensa = TestData.CrearRecompensa(50, "Experiencia", null);
        List<Recompensa> recompensas = new List<Recompensa>{recompensa};
        
        // Act
        this._recompensaLogica.ReclamarRecompensas(recompensas, partida);

        // Assert
        _mockPartidaRepositorio.Verify(
            r => r.Actualizar(partida), 
            Times.Once() 
        );
    }
    
    [Fact]
    public async void Recompensa_Crear_Correcto()
    {
        // Arrange
        Recompensa recompensa = TestData.CrearRecompensa(500, "Energia");

        // Act
        await _recompensaLogica.CrearRecompensa(recompensa);

        // Assert
        _mockRecompensaRepositorio.Verify(r => r.Agregar(It.IsAny<Recompensa>()), Times.Once);
    }
    
    [Fact]
        public async void Recompensa_Crear_FallaSiEligeRecuroAndEstructura()
        {
            // Arrange
            Recompensa recompensa = TestData.CrearRecompensa(500, "Energia");
            recompensa.EstructuraId = 1;

            // Act & Assert
            await Assert.ThrowsAsync<DominioException>(() => _recompensaLogica.CrearRecompensa(recompensa));
            _mockRecompensaRepositorio.Verify(r => r.Agregar(It.IsAny<Recompensa>()), Times.Never);
        }

        [Fact]
        public async void Crear_ConColumnaMal_Falla()
        {
            // Arrange
            Recompensa recompensa = TestData.CrearRecompensa(500, "MAL");

            // Act & Assert
            await Assert.ThrowsAsync<DominioException>(() => _recompensaLogica.CrearRecompensa(recompensa));
            _mockRecompensaRepositorio.Verify(r => r.Agregar(It.IsAny<Recompensa>()), Times.Never);
        }

        [Fact]
        public async void Crear_ConColumnaAndEstructuraNull_Falla()
        {
            // Arrange
            Recompensa recompensa = TestData.CrearRecompensa(500);

            // Act & Assert
            await Assert.ThrowsAsync<DominioException>(() => _recompensaLogica.CrearRecompensa(recompensa));
            _mockRecompensaRepositorio.Verify(r => r.Agregar(It.IsAny<Recompensa>()), Times.Never);
        }

        [Fact]
        public async void Crear_ConCantidadNegativa_Falla()
        {
            // Arrange
            Recompensa recompensa = TestData.CrearRecompensa(-500, "Energia");

            // Act & Assert
            await Assert.ThrowsAsync<DominioException>(() => _recompensaLogica.CrearRecompensa(recompensa));
            _mockRecompensaRepositorio.Verify(r => r.Agregar(It.IsAny<Recompensa>()), Times.Never);
        }

        [Fact]
        public async void Obtener_Recompensa_OK()
        {
            // Arrange
            Recompensa recompensa1 = TestData.CrearRecompensa(500, "Energia");
            Recompensa recompensa2 = TestData.CrearRecompensa(300, "Energia");
            Recompensa recompensa3 = TestData.CrearRecompensa(400, "Energia");
            var listaRecompensas = new List<Recompensa> { recompensa1, recompensa2, recompensa3 };
            _mockRecompensaRepositorio
                .Setup(r => r.ObtenerTodos())
                .ReturnsAsync(listaRecompensas);

            // Act
            var resultado = await _recompensaLogica.Listado();

            // Assert
            Assert.Equal(listaRecompensas.Count, resultado.Count);
            Assert.Equal(listaRecompensas.First().NombreColumna, resultado.First().NombreColumna);
        }
        
        [Fact]
    public async Task ReclamarRecompensas_RecompensasValidas_AplicaRecompensas()
    {
        // Arrange
        var partida = new Partida
        {
            Id = 1,
            Recursos = new Recurso
            {
                EcoCoins = 100,
                Felicidad = 50,
                Energia = 75,
                Contaminacion = 30
            }
        };
        var recompensas = new List<Recompensa>
        {
            new Recompensa { NombreColumna = "EcoCoins", Cantidad = 50 },
            new Recompensa { NombreColumna = "Felicidad", Cantidad = 10 }
        };

        _mockPartidaRepositorio.Setup(r => r.Actualizar(It.IsAny<Partida>()))
            .Returns(Task.CompletedTask);

        // Act
        await _recompensaLogica.ReclamarRecompensas(recompensas, partida);

        // Assert
        Assert.Equal(150, partida.Recursos.EcoCoins);
        Assert.Equal(60, partida.Recursos.Felicidad);
        _mockPartidaRepositorio.Verify(r => r.Actualizar(partida), Times.Once);
        _mockUnidadDeTrabajo.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task ReclamarRecompensas_PartidaNull_LanzaExcepcion()
    {
        // Arrange
        var recompensas = new List<Condicion>
        {
            new Condicion { EsRecompensa = true }
        };

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _recompensaLogica.ReclamarRecompensas(recompensas, null!));
    }

    [Fact]
    public async Task ReclamarRecompensas_RecursosNull_LanzaExcepcion()
    {
        // Arrange
        var partida = new Partida
        {
            Id = 1,
            Recursos = null
        };
        var recompensas = new List<Condicion>
        {
            new Condicion { EsRecompensa = true }
        };

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _recompensaLogica.ReclamarRecompensas(recompensas, partida));
    }

    [Fact]
    public async Task ReclamarRecompensas_RecompensaInvalida_LanzaExcepcion()
    {
        // Arrange
        var partida = new Partida
        {
            Id = 1,
            Recursos = new Recurso()
        };
        var recompensas = new List<Condicion>
        {
            new Condicion { EsRecompensa = false }
        };

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _recompensaLogica.ReclamarRecompensas(recompensas, partida));
    }

    [Fact]
    public async Task ReclamarRecompensa_RecompensaValida_AplicaRecompensa()
    {
        // Arrange
        var partida = new Partida
        {
            Id = 1,
            Recursos = new Recurso
            {
                Energia = 50
            }
        };
        var recompensa = new Condicion
        {
            NombreColumna = "Energia",
            Cantidad = 25,
            EsRecompensa = true
        };

        _mockPartidaRepositorio.Setup(r => r.Actualizar(It.IsAny<Partida>()))
            .Returns(Task.CompletedTask);

        // Act
        await _recompensaLogica.ReclamarRecompensa(recompensa, partida);

        // Assert
        Assert.Equal(75, partida.Recursos.Energia);
        _mockPartidaRepositorio.Verify(r => r.Actualizar(partida), Times.Once);
    }

    [Fact]
    public async Task ReclamarRecompensa_PartidaNull_LanzaExcepcion()
    {
        // Arrange
        var recompensa = new Condicion { EsRecompensa = true };

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _recompensaLogica.ReclamarRecompensa(recompensa, null!));
    }

    [Fact]
    public async Task ReclamarRecompensa_RecompensaInvalida_LanzaExcepcion()
    {
        // Arrange
        var partida = new Partida
        {
            Id = 1,
            Recursos = new Recurso()
        };
        var recompensa = new Condicion { EsRecompensa = false };

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _recompensaLogica.ReclamarRecompensa(recompensa, partida));
    }

    [Fact]
    public async Task ReclamarRecompensas_RecompensaConColumnaInvalida_IgnoraRecompensa()
    {
        // Arrange
        var partida = new Partida
        {
            Id = 1,
            Recursos = new Recurso
            {
                EcoCoins = 100
            }
        };
        var recompensas = new List<Condicion>
        {
            new Condicion { NombreColumna = "EcoCoins", Cantidad = 50, EsRecompensa = true },
            new Condicion { NombreColumna = "ColumnaInexistente", Cantidad = 10, EsRecompensa = true }
        };

        _mockPartidaRepositorio.Setup(r => r.Actualizar(It.IsAny<Partida>()))
            .Returns(Task.CompletedTask);

        // Act
        await _recompensaLogica.ReclamarRecompensas(recompensas, partida);

        // Assert
        Assert.Equal(150, partida.Recursos.EcoCoins);
        _mockPartidaRepositorio.Verify(r => r.Actualizar(partida), Times.Once);
    }

}