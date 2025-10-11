using CivitaBack.Data.BO;
using Xunit;

namespace CivitaBack.Tests
{
    public class TestModeladoTest
    {
        [Fact]
        public void TestModelado_Constructor_InitializesProperties()
        {
            // Act
            var testModelado = new TestModelado();

            // Assert
            Assert.Equal(0, testModelado.Id);
            Assert.Null(testModelado.Correo);
        }

        [Fact]
        public void TestModelado_SetProperties_ValuesAreSet()
        {
            // Arrange
            var testModelado = new TestModelado();

            // Act
            testModelado.Id = 1;
            testModelado.Correo = "testmodelado@example.com";

            // Assert
            Assert.Equal(1, testModelado.Id);
            Assert.Equal("testmodelado@example.com", testModelado.Correo);
        }

        [Fact]
        public void TestModelado_WithNullCorreo_PropertyCanBeNull()
        {
            // Arrange
            var testModelado = new TestModelado
            {
                Id = 1,
                Correo = null
            };

            // Assert
            Assert.Equal(1, testModelado.Id);
            Assert.Null(testModelado.Correo);
        }

        [Fact]
        public void TestModelado_DefaultId_IsZero()
        {
            // Act
            var testModelado = new TestModelado();

            // Assert
            Assert.Equal(0, testModelado.Id);
        }
    }
}
