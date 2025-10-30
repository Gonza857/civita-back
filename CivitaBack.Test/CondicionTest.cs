using CivitaBack.Data.BO;
using CivitaBack.Data.Repositorio;
using CivitaBack.Logica;
using CivitaBack.Logica.Excepciones;
using Moq;
using Xunit;

namespace CivitaBack.Tests
{
    public class CondicionTest
    {
        
        private readonly Mock<ICondicionRepositorio> _mockCondicionRepositorio;
        private readonly Mock<IEstructuraRepositorio> _mockEstructuraRepositorio;
        
        private readonly ICondicionLogica _condicionLogica;

        public CondicionTest()
        {
            _mockCondicionRepositorio = new Mock<ICondicionRepositorio>();
            _mockEstructuraRepositorio = new Mock<IEstructuraRepositorio>();
            
            _condicionLogica = new CondicionLogica(
                _mockCondicionRepositorio.Object,
                _mockEstructuraRepositorio.Object
            );
        }
        
        // [Fact]
        // public void Condicion_Constructor_InitializesProperties()
        // {
        //     // Act
        //     var condicion = new Condicion();
        //
        //     // Assert
        //     Assert.Equal(0, condicion.Id);
        //     Assert.Equal(0, condicion.Cantidad);
        //     Assert.Equal(0, condicion.EstructuraId);
        //     Assert.Null(condicion.Estructura);
        // }

        [Fact]
        public void Condicion_SetProperties_ValuesAreSet()
        {
            // Arrange
            var condicion = new Condicion();
            var estructura = new Estructura { Id = 1 };
            var recurso = new Recurso { Id = 1 };

            // Act
            condicion.Id = 1;
            condicion.Cantidad = 10;
            condicion.EstructuraId = 5;
            condicion.Estructura = estructura;

            // Assert
            Assert.Equal(1, condicion.Id);
            Assert.Equal(10, condicion.Cantidad);
            Assert.Equal(5, condicion.EstructuraId);
            Assert.Equal(estructura, condicion.Estructura);
        }

        [Fact]
        public void Condicion_WithNullReferences_PropertiesCanBeNull()
        {
            // Arrange
            var condicion = new Condicion
            {
                Id = 1,
                Cantidad = 5,
                EstructuraId = 2,
                Estructura = null,
            };

            // Assert
            Assert.Equal(1, condicion.Id);
            Assert.Equal(5, condicion.Cantidad);
            Assert.Equal(2, condicion.EstructuraId);
            Assert.Null(condicion.Estructura);
        }

        [Fact]
        public void Condicion_WithZeroValues_PropertiesCanBeZero()
        {
            // Arrange
            var condicion = new Condicion
            {
                Id = 0,
                Cantidad = 0,
                EstructuraId = 0,
            };

            // Assert
            Assert.Equal(0, condicion.Id);
            Assert.Equal(0, condicion.Cantidad);
            Assert.Equal(0, condicion.EstructuraId);
        }
        
        [Fact]
        public async void Recompensa_Crear_Correcto()
        {
            // Arrange
            CondicionDTO recompensa = TestData.CrearRecompensaDTO(500, "Energia");
        
            // Act
            await _condicionLogica.CrearRecompensa(recompensa);
        
            // Assert
            _mockCondicionRepositorio.Verify(r => r.Agregar(It.IsAny<Condicion>()), Times.Once);
        }
        
        [Fact]
        public async void Recompensa_Crear_FallaSiEligeRecuroAndEstructura()
        {
            // Arrange
            CondicionDTO recompensa = TestData.CrearRecompensaDTO( 500, "Energia");
            recompensa.EstructuraId = 1;
        
            // Act & Assert
            await Assert.ThrowsAsync<CondicionExcepcion>(() => _condicionLogica.CrearRecompensa(recompensa));
            _mockCondicionRepositorio.Verify(r => r.Agregar(It.IsAny<Condicion>()), Times.Never);
        }
        
        [Fact]
        public async void Crear_ConColumnaMal_Falla()
        {
            // Arrange
            CondicionDTO recompensa = TestData.CrearRecompensaDTO( 500, "MAL");
        
            // Act & Assert
            await Assert.ThrowsAsync<CondicionExcepcion>(() => _condicionLogica.CrearRecompensa(recompensa));
            _mockCondicionRepositorio.Verify(r => r.Agregar(It.IsAny<Condicion>()), Times.Never);
        }
        
        [Fact]
        public async void Crear_ConColumnaAndEstructuraNull_Falla()
        {
            // Arrange
            CondicionDTO recompensa = TestData.CrearRecompensaDTO( 500);
        
            // Act & Assert
            await Assert.ThrowsAsync<CondicionExcepcion>(() => _condicionLogica.CrearRecompensa(recompensa));
            _mockCondicionRepositorio.Verify(r => r.Agregar(It.IsAny<Condicion>()), Times.Never);
        }
        
        [Fact]
        public async void Crear_ConCantidadNegativa_Falla()
        {
            // Arrange
            CondicionDTO recompensa = TestData.CrearRecompensaDTO( -500, "Energia");
        
            // Act & Assert
            await Assert.ThrowsAsync<CondicionExcepcion>(() => _condicionLogica.CrearRecompensa(recompensa));
            _mockCondicionRepositorio.Verify(r => r.Agregar(It.IsAny<Condicion>()), Times.Never);
        }
        
        [Fact]
        public async void Obtener_Recompensa_OK()
        {
            // Arrange
            Condicion recompensa1 = TestData.CrearRecompensa( 500, "Energia");
            Condicion recompensa2 = TestData.CrearRecompensa( 300, "Energia");
            Condicion recompensa3 = TestData.CrearRecompensa( 400, "Energia");
            var listaRecompensas = new List<Condicion>{recompensa1, recompensa2, recompensa3};
            _mockCondicionRepositorio
                .Setup(r => r.ObtenerTodasRecompensas())
                .ReturnsAsync(listaRecompensas);
            
            // Act
            var resultado = await _condicionLogica.ObtenerListadoRecompensas();
            
            // Assert
            Assert.Equal(listaRecompensas.Count, resultado.Count);
            Assert.Equal(listaRecompensas[0].NombreColumna, resultado[0].NombreColumna);
        }
    }
}
