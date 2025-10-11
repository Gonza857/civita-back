using CivitaBack.Data.BO;
using CivitaBack.Data.DTO;
using CivitaBack.Data.Repositorio;
using CivitaBack.Logica;
using Moq;
using Xunit;

namespace CivitaBack.Tests
{
    public class PartidaLogicaTest
    {
        private readonly Mock<IRepositorioPartida> mockRepo;
        private readonly PartidaLogica partidaLogica;

        public PartidaLogicaTest()
        {
            mockRepo = new Mock<IRepositorioPartida>();
            partidaLogica = new PartidaLogica(mockRepo.Object);
        }

        [Fact]
        public void Constructor_ConRepositorio_AsignaCorrectamente()
        {
            // Arrange
            var mockRepositorio = new Mock<IRepositorioPartida>();

            // Act
            var logica = new PartidaLogica(mockRepositorio.Object);

            // Assert
            Assert.NotNull(logica);
        }

        [Fact]
        public void CrearPartida_ConUsuarioHardcodeado_RetornaPartidaDTO()
        {
            // Arrange
            var partidaEsperada = new Partida
            {
                Id = 1,
                UsuarioId = 1,
                UltimaVez = DateTime.Now,
                JsonMapa = "{}"
            };

            mockRepo.Setup(r => r.CrearPartida(It.IsAny<Usuario>()))
                   .Returns(partidaEsperada);

            // Act
            var resultado = partidaLogica.CrearPartida();

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(partidaEsperada.Id, resultado.Id);
            Assert.Equal(partidaEsperada, resultado.Partida);
            
            // Verificar que se llamó al repositorio con un usuario hardcodeado
            mockRepo.Verify(r => r.CrearPartida(It.Is<Usuario>(u => 
                u.Mail == "hardcode@mail.com" && 
                u.NombreUsuario == "HardCodeUser123" && 
                u.HashDeContrasena == "abc123")), Times.Once);
        }

        [Fact]
        public void ObtenerPartidas_ConListaVacia_RetornaListaVacia()
        {
            // Arrange
            var partidasVacia = new List<Partida>();
            mockRepo.Setup(r => r.ObtenerPartidas()).Returns(partidasVacia);

            // Act
            var resultado = partidaLogica.ObtenerPartidas();

            // Assert
            Assert.NotNull(resultado);
            Assert.Empty(resultado);
            mockRepo.Verify(r => r.ObtenerPartidas(), Times.Once);
        }

        [Fact]
        public void ObtenerPartidas_ConPartidas_RetornaListaDTO()
        {
            // Arrange
            var partidas = new List<Partida>
            {
                new Partida { Id = 1, UsuarioId = 1, UltimaVez = DateTime.Now },
                new Partida { Id = 2, UsuarioId = 2, UltimaVez = DateTime.Now.AddDays(-1) }
            };

            mockRepo.Setup(r => r.ObtenerPartidas()).Returns(partidas);

            // Act
            var resultado = partidaLogica.ObtenerPartidas();

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count);
            
            // Verificar primer DTO
            Assert.Equal(1, resultado[0].Id);
            Assert.Equal(partidas[0], resultado[0].Partida);
            
            // Verificar segundo DTO
            Assert.Equal(2, resultado[1].Id);
            Assert.Equal(partidas[1], resultado[1].Partida);
            
            mockRepo.Verify(r => r.ObtenerPartidas(), Times.Once);
        }

        [Fact]
        public void ObtenerPorUsuarioId_ConIdUsuario_LanzaNotImplementedException()
        {
            // Arrange
            var idUsuario = 1;

            // Act & Assert
            var exception = Assert.Throws<NotImplementedException>(() => 
                partidaLogica.ObtenerPorUsuarioId(idUsuario));
            
            Assert.Equal("The method or operation is not implemented.", exception.Message);
        }

        [Fact]
        public void PartidaToDTO_ConPartida_RetornaDTOCorrecto()
        {
            // Arrange
            var partida = new Partida
            {
                Id = 5,
                UsuarioId = 10,
                UltimaVez = DateTime.Now,
                JsonMapa = "{\"test\": \"data\"}"
            };

            // Act - Usar reflexión para acceder al método privado
            var metodo = typeof(PartidaLogica).GetMethod("PartidaToDTO", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var resultado = (PartidaDTO)metodo!.Invoke(partidaLogica, new object[] { partida })!;

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(partida.Id, resultado.Id);
            Assert.Equal(partida, resultado.Partida);
        }

        [Fact]
        public void PartidaToDTO_ConPartidaNull_LanzaNullReferenceException()
        {
            // Arrange
            Partida? partida = null;

            // Act & Assert - Usar reflexión para acceder al método privado
            var metodo = typeof(PartidaLogica).GetMethod("PartidaToDTO", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            var exception = Assert.Throws<System.Reflection.TargetInvocationException>(() => 
                metodo!.Invoke(partidaLogica, new object[] { partida! }));
            
            Assert.IsType<NullReferenceException>(exception.InnerException);
        }

        [Fact]
        public void CrearPartida_RepositorioRetornaNull_LanzaNullReferenceException()
        {
            // Arrange
            mockRepo.Setup(r => r.CrearPartida(It.IsAny<Usuario>()))
                   .Returns((Partida)null!);

            // Act & Assert
            var exception = Assert.Throws<NullReferenceException>(() => 
                partidaLogica.CrearPartida());
            
            Assert.NotNull(exception);
        }

        [Fact]
        public void ObtenerPartidas_RepositorioRetornaNull_LanzaArgumentNullException()
        {
            // Arrange
            mockRepo.Setup(r => r.ObtenerPartidas()).Returns((List<Partida>)null!);

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => 
                partidaLogica.ObtenerPartidas());
            
            Assert.NotNull(exception);
            Assert.Equal("source", exception.ParamName);
        }
    }
}
