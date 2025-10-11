using CivitaBack.Data.BO;
using Xunit;

namespace CivitaBack.Tests
{
    public class TestBOTest
    {
        [Fact]
        public void Test_Constructor_InitializesProperties()
        {
            // Act
            var test = new Test();

            // Assert
            Assert.Equal(0, test.Id);
            Assert.Null(test.Correo);
        }

        [Fact]
        public void Test_SetProperties_ValuesAreSet()
        {
            // Arrange
            var test = new Test();

            // Act
            test.Id = 1;
            test.Correo = "test@example.com";

            // Assert
            Assert.Equal(1, test.Id);
            Assert.Equal("test@example.com", test.Correo);
        }

        [Fact]
        public void Test_WithNullCorreo_PropertyCanBeNull()
        {
            // Arrange
            var test = new Test
            {
                Id = 1,
                Correo = null
            };

            // Assert
            Assert.Equal(1, test.Id);
            Assert.Null(test.Correo);
        }

        [Fact]
        public void Test_DefaultId_IsZero()
        {
            // Act
            var test = new Test();

            // Assert
            Assert.Equal(0, test.Id);
        }
    }
}
