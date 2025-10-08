using CivitaBack.Data.BO;
using CivitaBack.Logica;
using Moq;

namespace CivitaBack.Tests;


public class TestServicioTest
{
    [Fact]
    public void ObtenerTest_DeberiaRetornarLista()
    {
        // Arrange
        var mockRepo = new Mock<ITestRepositorio>();

        // Configuramos el mock:
        mockRepo
            .Setup(r => r.ObtenerTests())
            .Returns(new List<Test>());

        var servicio = new TestServicio(mockRepo.Object);

        // Act
        var resultado = servicio.ObtenerTests();

        // Assert
        Assert.Empty(resultado);
    }

    [Fact]
    public void Test()
    {
        // Arrange
        var mockRepo = new Mock<ITestRepositorio>();

        Partida partida = new Partida();
        Usuario usuario = new Usuario { NombreUsuario = "Hola" };
        //usuario.Partida = partida;



        // Configuramos el mock:
        mockRepo
            .Setup(r => r.ObtenerTests())
            .Returns(new List<Test>());

        var servicio = new TestServicio(mockRepo.Object);

        // Act
        var resultado = servicio.ObtenerTests();

        // Assert
        Assert.Empty(resultado);
    }
}