using CivitaBack.Data.BO;
using CivitaBack.Logica;
using Moq;

namespace CivitaBack.Tests;


public class TestServicioTest
{
    [Fact]
    public void ObtenerTesto_DeberiaRetornarLista()
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
}