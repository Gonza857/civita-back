using CivitaBack.Data.BO;
using CivitaBack.Data.EF;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System;
using System.Linq;

namespace CivitaBack.Tests
{
    public class AppDbContextTest
    {
        private DbContextOptions<AppDbContext> CreateNewContextOptions()
        {
            return new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public void Constructor_WithOptions_CreatesContext()
        {
            // Arrange
            var options = CreateNewContextOptions();

            // Act
            using var context = new AppDbContext(options);

            // Assert
            Assert.NotNull(context);
        }

        [Fact]
        public void DbSets_AreInitialized()
        {
            // Arrange
            var options = CreateNewContextOptions();

            // Act
            using var context = new AppDbContext(options);

            // Assert
            Assert.NotNull(context.Partida);
            Assert.NotNull(context.Usuario);
            Assert.NotNull(context.Estructura);
            Assert.NotNull(context.TipoEstructura);
            Assert.NotNull(context.Recurso);
            Assert.NotNull(context.Evento);
            Assert.NotNull(context.EventoMaestro);
            Assert.NotNull(context.Tienda);
            Assert.NotNull(context.EstructuraMapa);
            Assert.NotNull(context.Tip);
            Assert.NotNull(context.TipEnPartida);
            Assert.NotNull(context.TipoTip);
            Assert.NotNull(context.Condicion);
            Assert.NotNull(context.TipoLogro);
            Assert.NotNull(context.Logro);
            Assert.NotNull(context.LogroPartida);
        }

        [Fact]
        public void CanAddAndRetrievePartida()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using var context = new AppDbContext(options);

            var usuario = new Usuario
            {
                NombreUsuario = "TestUser",
                Mail = "test@test.com",
                HashDeContrasena = "hash123"
            };
            context.Usuario.Add(usuario);
            context.SaveChanges();

            var partida = new Partida
            {
                UsuarioId = usuario.Id,
                UltimaVez = DateTime.Now,
                JsonMapa = "{}"
            };

            // Act
            context.Partida.Add(partida);
            context.SaveChanges();

            // Assert
            var retrievedPartida = context.Partida.FirstOrDefault(p => p.Id == partida.Id);
            Assert.NotNull(retrievedPartida);
            Assert.Equal(partida.UsuarioId, retrievedPartida.UsuarioId);
        }

        [Fact]
        public void CanAddAndRetrieveUsuario()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using var context = new AppDbContext(options);

            var usuario = new Usuario
            {
                NombreUsuario = "TestUser2",
                Mail = "test2@test.com",
                HashDeContrasena = "hash456"
            };

            // Act
            context.Usuario.Add(usuario);
            context.SaveChanges();

            // Assert
            var retrievedUsuario = context.Usuario.FirstOrDefault(u => u.Mail == "test2@test.com");
            Assert.NotNull(retrievedUsuario);
            Assert.Equal("TestUser2", retrievedUsuario.NombreUsuario);
        }

        [Fact]
        public void LogroPartida_HasCompositeKey()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using var context = new AppDbContext(options);

            // Get the entity type for LogroPartida
            var entityType = context.Model.FindEntityType(typeof(LogroPartida));

            // Act
            var primaryKey = entityType?.FindPrimaryKey();

            // Assert
            Assert.NotNull(primaryKey);
            Assert.Equal(2, primaryKey.Properties.Count);
            Assert.Contains(primaryKey.Properties, p => p.Name == "LogroId");
            Assert.Contains(primaryKey.Properties, p => p.Name == "PartidaId");
        }

        // [Fact]
        // public void CanAddMultipleEntities()
        // {
        //     // Arrange
        //     var options = CreateNewContextOptions();
        //     using var context = new AppDbContext(options);
        //
        //     var tipoEstructura = new TipoEstructura
        //     {
        //         Nombre = "Vivienda",
        //         Ocupacion = "Residencial",
        //         Capacidad = 100
        //     };
        //
        //     var estructura = new Estructura
        //     {
        //         Nombre = "Casa",
        //         EsMejorable = true,
        //         TipoEstructura = tipoEstructura
        //     };
        //
        //     // Act
        //     context.TipoEstructura.Add(tipoEstructura);
        //     context.Estructura.Add(estructura);
        //     context.SaveChanges();
        //
        //     // Assert
        //     Assert.Equal(1, context.TipoEstructura.Count());
        //     Assert.Equal(1, context.Estructura.Count());
        // }

        [Fact]
        public void CanAddTipoLogro()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using var context = new AppDbContext(options);

            var tipoLogro = new TipoLogro
            {
                Nombre = "Logro de Prueba"
            };

            // Act
            context.TipoLogro.Add(tipoLogro);
            context.SaveChanges();

            // Assert
            var retrieved = context.TipoLogro.FirstOrDefault(t => t.Nombre == "Logro de Prueba");
            Assert.NotNull(retrieved);
        }

        /*[Fact]
        public void CanAddEventoMaestro()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using var context = new AppDbContext(options);

            var eventoMaestro = new EventoMaestro
            {
                Descripcion = "Evento de prueba"
            };

            // Act
            context.EventoMaestro.Add(eventoMaestro);
            context.SaveChanges();

            // Assert
            Assert.Equal(1, context.EventoMaestro.Count());
        }*/

        [Fact]
        public void CanAddTipoTip()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using var context = new AppDbContext(options);

            var tipoTip = new TipoTip
            {
                Descripcion = "Tip de prueba"
            };

            // Act
            context.TipoTip.Add(tipoTip);
            context.SaveChanges();

            // Assert
            Assert.Equal(1, context.TipoTip.Count());
        }
    }
}

