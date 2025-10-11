using CivitaBack.Data.BO;
using Xunit;

namespace CivitaBack.Tests
{
    public class UsuarioTest
    {
        [Fact]
        public void Usuario_Constructor_InitializesProperties()
        {
            // Act
            var usuario = new Usuario();

            // Assert
            Assert.Equal(0, usuario.Id);
            Assert.Null(usuario.NombreUsuario);
            Assert.Null(usuario.Mail);
            Assert.Null(usuario.HashDeContrasena);
            // La propiedad Partida no se inicializa automáticamente
        }

        [Fact]
        public void Usuario_SetProperties_ValuesAreSet()
        {
            // Arrange
            var usuario = new Usuario();

            // Act
            usuario.Id = 1;
            usuario.NombreUsuario = "testuser";
            usuario.Mail = "test@example.com";
            usuario.HashDeContrasena = "hashedpassword";

            // Assert
            Assert.Equal(1, usuario.Id);
            Assert.Equal("testuser", usuario.NombreUsuario);
            Assert.Equal("test@example.com", usuario.Mail);
            Assert.Equal("hashedpassword", usuario.HashDeContrasena);
        }

        [Fact]
        public void Usuario_WithNullValues_PropertiesCanBeNull()
        {
            // Arrange
            var usuario = new Usuario
            {
                Id = 1,
                NombreUsuario = null,
                Mail = null,
                HashDeContrasena = null
            };

            // Assert
            Assert.Equal(1, usuario.Id);
            Assert.Null(usuario.NombreUsuario);
            Assert.Null(usuario.Mail);
            Assert.Null(usuario.HashDeContrasena);
        }
    }
}
