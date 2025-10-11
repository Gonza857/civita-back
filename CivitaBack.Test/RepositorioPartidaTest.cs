using CivitaBack.Data.BO;
using CivitaBack.Data.EF;
using CivitaBack.Data.Repositorio;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System;
using System.Linq;

namespace CivitaBack.Tests
{
    public class RepositorioPartidaTest
    {
        private DbContextOptions<AppDbContext> CreateNewContextOptions()
        {
            return new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public void Constructor_ConDbContext_AsignaCorrectamente()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using var context = new AppDbContext(options);

            // Act
            var repositorio = new RepositorioPartida(context);

            // Assert
            Assert.NotNull(repositorio);
        }

        [Fact]
        public void CrearPartida_ConUsuarioValido_CreaPartidaYUsuario()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using var context = new AppDbContext(options);
            var repositorio = new RepositorioPartida(context);

            var usuario = new Usuario
            {
                NombreUsuario = "testuser",
                Mail = "test@example.com",
                HashDeContrasena = "hashedpassword"
            };

            // Act
            var partidaCreada = repositorio.CrearPartida(usuario);

            // Assert
            Assert.NotNull(partidaCreada);
            Assert.True(partidaCreada.Id > 0);
            Assert.True(usuario.Id > 0);
            Assert.Equal(usuario.Id, partidaCreada.UsuarioId);
        }

        [Fact]
        public void CrearPartida_GuardaEnBaseDeDatos()
        {
            // Arrange
            var options = CreateNewContextOptions();
            Usuario usuario;
            Partida partida;

            using (var context = new AppDbContext(options))
            {
                var repositorio = new RepositorioPartida(context);
                usuario = new Usuario
                {
                    NombreUsuario = "testuser2",
                    Mail = "test2@example.com",
                    HashDeContrasena = "hashedpassword2"
                };

                // Act
                partida = repositorio.CrearPartida(usuario);
            }

            // Assert - Verificar en nuevo contexto
            using (var context = new AppDbContext(options))
            {
                var usuarioGuardado = context.Usuario.Find(usuario.Id);
                var partidaGuardada = context.Partida.Find(partida.Id);

                Assert.NotNull(usuarioGuardado);
                Assert.NotNull(partidaGuardada);
                Assert.Equal("testuser2", usuarioGuardado.NombreUsuario);
                Assert.Equal(usuario.Id, partidaGuardada.UsuarioId);
            }
        }

        [Fact]
        public void ObtenerPartidas_ConPartidasEnDb_RetornaLista()
        {
            // Arrange
            var options = CreateNewContextOptions();
            
            using (var context = new AppDbContext(options))
            {
                var repositorio = new RepositorioPartida(context);
                var usuario1 = new Usuario { NombreUsuario = "user1", Mail = "user1@test.com", HashDeContrasena = "hash1" };
                var usuario2 = new Usuario { NombreUsuario = "user2", Mail = "user2@test.com", HashDeContrasena = "hash2" };
                
                repositorio.CrearPartida(usuario1);
                repositorio.CrearPartida(usuario2);
            }

            // Act
            using (var context = new AppDbContext(options))
            {
                var repositorio = new RepositorioPartida(context);
                var partidas = repositorio.ObtenerPartidas();

                // Assert
                Assert.NotNull(partidas);
                Assert.Equal(2, partidas.Count);
            }
        }

        [Fact]
        public void ObtenerPartidas_SinPartidas_RetornaListaVacia()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using var context = new AppDbContext(options);
            var repositorio = new RepositorioPartida(context);

            // Act
            var partidas = repositorio.ObtenerPartidas();

            // Assert
            Assert.NotNull(partidas);
            Assert.Empty(partidas);
        }

        [Fact]
        public void ObtenerPorUsuarioId_LanzaNotImplementedException()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using var context = new AppDbContext(options);
            var repositorio = new RepositorioPartida(context);

            // Act & Assert
            var exception = Assert.Throws<NotImplementedException>(() => 
                repositorio.ObtenerPorUsuarioId(1));
            
            Assert.NotNull(exception);
        }

        [Fact]
        public void CrearPartida_ConMultiplesUsuarios_CadaPartidaTieneUsuarioCorrect()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using var context = new AppDbContext(options);
            var repositorio = new RepositorioPartida(context);

            var usuario1 = new Usuario { NombreUsuario = "user1", Mail = "user1@test.com", HashDeContrasena = "hash1" };
            var usuario2 = new Usuario { NombreUsuario = "user2", Mail = "user2@test.com", HashDeContrasena = "hash2" };

            // Act
            var partida1 = repositorio.CrearPartida(usuario1);
            var partida2 = repositorio.CrearPartida(usuario2);

            // Assert
            Assert.NotEqual(partida1.UsuarioId, partida2.UsuarioId);
            Assert.Equal(usuario1.Id, partida1.UsuarioId);
            Assert.Equal(usuario2.Id, partida2.UsuarioId);
        }
    }
}
