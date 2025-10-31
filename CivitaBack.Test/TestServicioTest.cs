// using CivitaBack.Data.BO;
// using CivitaBack.Logica;
// using Moq;
// using Xunit;
//
// namespace CivitaBack.Tests
// {
//     public class TestServicioTest
//     {
//         private readonly Mock<ITestRepositorio> mockRepo;
//         private readonly TestServicio testServicio;
//
//         public TestServicioTest()
//         {
//             mockRepo = new Mock<ITestRepositorio>();
//             testServicio = new TestServicio(mockRepo.Object);
//         }
//
//         [Fact]
//         public void Constructor_ConRepositorio_AsignaCorrectamente()
//         {
//             // Arrange
//             var mockRepositorio = new Mock<ITestRepositorio>();
//
//             // Act
//             var servicio = new TestServicio(mockRepositorio.Object);
//
//             // Assert
//             Assert.NotNull(servicio);
//         }
//
//         [Fact]
//         public void ObtenerTests_RetornaListaVacia()
//         {
//             // Act
//             var resultado = testServicio.ObtenerTests();
//
//             // Assert
//             Assert.NotNull(resultado);
//             Assert.Empty(resultado);
//         }
//
//         [Fact]
//         public void ObtenerTests_MultipleCalls_AlwaysReturnsEmptyList()
//         {
//             // Act
//             var resultado1 = testServicio.ObtenerTests();
//             var resultado2 = testServicio.ObtenerTests();
//             var resultado3 = testServicio.ObtenerTests();
//
//             // Assert
//             Assert.NotNull(resultado1);
//             Assert.NotNull(resultado2);
//             Assert.NotNull(resultado3);
//             Assert.Empty(resultado1);
//             Assert.Empty(resultado2);
//             Assert.Empty(resultado3);
//         }
//
//         [Fact]
//         public void ObtenerTests_ReturnsNewListInstance()
//         {
//             // Act
//             var resultado1 = testServicio.ObtenerTests();
//             var resultado2 = testServicio.ObtenerTests();
//
//             // Assert
//             Assert.NotSame(resultado1, resultado2);
//         }
//     }
// }