// using CivitaBack.Data.DTO;
// using CivitaBack.Domain.Entidades;
// using CivitaBack.Domain.Interfaces.Repositorios;
// using CivitaBack.Logica;
// using CivitaBack.Logica.Excepciones;
// using CivitaBack.Logica.Helpers;
// using CivitaBack.Utils;
// using Microsoft.Extensions.Configuration;
// using Moq;
//
// namespace CivitaBack.Tests
// {
//     public class AuthLogicaTest
//     {
//
//         private readonly Mock<IUsuarioRepositorio> mockRepo;
//         private readonly IConfiguration configuration;
//         private readonly AuthLogica servicio;
//         private readonly Mock<IUnidadDeTrabajo> _mockUow;
//
//         public AuthLogicaTest()
//         {
//             mockRepo = new Mock<IUsuarioRepositorio>();
//             _mockUow = new Mock<IUnidadDeTrabajo>();
//
//
//             configuration = new ConfigurationBuilder()
//                 .AddInMemoryCollection(new Dictionary<string, string> { { "Jwt:Key", "S3gura!ClaveDeJWT2025!A7k@dF#9hL$3gT%qW&8zV^1sP*4bX" } })
//                 .Build();
//
//             servicio = new AuthLogica(mockRepo.Object, configuration, _mockUow.Object);
//         }
//
//         [Fact]
//         public async Task RegistrarUsuario_Correto_RetornaUsuario()
//         {
//
//             mockRepo.Setup(r => r.ObtenerUsuarioPorMail(It.IsAny<string>())).ReturnsAsync((Usuario?)null);
//             mockRepo.Setup(r => r.ObtenerUsuarioPorNombre(It.IsAny<string>())).ReturnsAsync((Usuario?)null);
//             mockRepo.Setup(r => r.CrearUsuario(It.IsAny<Usuario>())).ReturnsAsync((Usuario u) => u);
//
//             var nombre = "Martin";
//             var mail = "martin@ejemplo.com";
//             var password = "123456";
//
//             var resultado = await servicio.CrearUsuario(nombre, mail, password);
//
//             // Assert
//             Assert.NotNull(resultado);
//             Assert.Equal(nombre, resultado.NombreUsuario);
//             Assert.Equal(mail, resultado.Mail);
//         }
//
//         [Fact]
//         public async Task RegistrarUsuario_EmailExistente_LanzaExcepcion()
//         {
//             mockRepo.Setup(r => r.ObtenerUsuarioPorMail(It.IsAny<string>())).ReturnsAsync(new Usuario());
//
//             // Act & Assert
//             await Assert.ThrowsAsync<ValidacionRegistroException>(() =>
//                 servicio.CrearUsuario("Martin", "martin@ejemplo.com", "123456")
//             );
//         }
//
//         [Fact]
//         public async Task LoginAsync_UsuarioValido_RetornaToken()
//         {
//             // Arrange
//             var usuario = new Usuario
//             {
//                 NombreUsuario = "martin",
//                 Mail = "martin@test.com",
//                 HashDeContrasena = PasswordHelper.HashPassword("password123")
//             };
//
//             mockRepo.Setup(r => r.ObtenerUsuarioPorMail("martin@test.com"))
//                     .ReturnsAsync(usuario);
//
//             var loginRequest = new IniciarSesionDTO
//             {
//                 Mail = "martin@test.com",
//                 Password = "password123"
//             };
//
//             // Act
//             var resultado = await servicio.LoginAsync(loginRequest);
//
//             // Assert
//             Assert.NotNull(resultado.Token);
//             Assert.Equal("martin", resultado.NombreUsuario);
//             Assert.Equal("martin@test.com", resultado.Mail);
//         }
//     }
// }
