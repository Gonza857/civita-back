using Xunit;
using Moq;
using CivitaBack.Logica;
using CivitaBack.Data.Repositorio;
using CivitaBack.Data.BO;
using System.Threading.Tasks;

namespace CivitaBack.Tests
{
    public class ServicioAuthTests
    {
        [Fact]
        public async Task RegistrarUsuario_Correto_RetornaUsuario()
        {
            var mockRepo = new Mock<IRepositorioUsuario>();
            mockRepo.Setup(r => r.obtenerUsuarioPorMail(It.IsAny<string>())).ReturnsAsync((Usuario?)null);
            mockRepo.Setup(r => r.obtenerUsuarioPorNombre(It.IsAny<string>())).ReturnsAsync((Usuario?)null);
            mockRepo.Setup(r => r.CrearUsuario(It.IsAny<Usuario>())).ReturnsAsync((Usuario u) => u);

            var servicio = new AuthLogica(mockRepo.Object);

            var nombre = "Martin";
            var mail = "martin@ejemplo.com";
            var password = "123456";

            var resultado = await servicio.RegistrarUsuarioAsync(nombre, mail, password);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(nombre, resultado.NombreUsuario);
            Assert.Equal(mail, resultado.Mail);
            Assert.NotNull(resultado.HashDeContrasena);
        }

        [Fact]
        public async Task RegistrarUsuario_EmailExistente_LanzaExcepcion()
        {
            var mockRepo = new Mock<IRepositorioUsuario>();
            mockRepo.Setup(r => r.obtenerUsuarioPorMail(It.IsAny<string>())).ReturnsAsync(new Usuario());
            var servicio = new AuthLogica(mockRepo.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
                servicio.RegistrarUsuarioAsync("Martin", "martin@ejemplo.com", "123456")
            );
        }
    }
}
