using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Logica;
using CivitaBack.Domain.Excepciones;
using CivitaBack.Utils;
using Moq;

namespace CivitaBack.Tests
{
    public class CondicionTest
    {

        private readonly Mock<ICondicionRepositorio> _mockCondicionRepositorio;
        private readonly Mock<IEstructuraRepositorio> _mockEstructuraRepositorio;
        private readonly Mock<IRecompensaRepositorio> _mockRecompensaRepositorio;
        private readonly Mock<IUnidadDeTrabajo> _mockUow;

        private readonly ICondicionLogica _condicionLogica;

        public CondicionTest()
        {
            _mockCondicionRepositorio = new Mock<ICondicionRepositorio>();
            _mockEstructuraRepositorio = new Mock<IEstructuraRepositorio>();
            _mockRecompensaRepositorio = new Mock<IRecompensaRepositorio>();
            _mockUow = new Mock<IUnidadDeTrabajo>();

            _condicionLogica = new CondicionLogica(
                _mockCondicionRepositorio.Object,
                _mockEstructuraRepositorio.Object,
                _mockUow.Object,
                _mockRecompensaRepositorio.Object
            );
        }

        [Fact]
        public void Condicion_Constructor_InitializesProperties()
        {
            // Act
            var condicion = TestData.CrearCondicion("Prueba", 0);

            // Assert
            Assert.Equal(1, condicion.Id);
            Assert.Equal(0, condicion.Cantidad);
            Assert.Equal(0, condicion.EstructuraId);
            Assert.Null(condicion.Estructura);
        }

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
        
    }
}
