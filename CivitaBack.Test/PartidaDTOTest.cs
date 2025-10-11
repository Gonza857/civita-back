using CivitaBack.Data.BO;
using CivitaBack.Data.DTO;
using Xunit;
using System;

namespace CivitaBack.Tests
{
    public class PartidaDTOTest
    {
        [Fact]
        public void PartidaDTO_Constructor_InitializesProperties()
        {
            // Act
            var partidaDTO = new PartidaDTO();

            // Assert
            Assert.NotNull(partidaDTO);
            Assert.Equal(0, partidaDTO.Id);
            Assert.Null(partidaDTO.Partida);
        }

        [Fact]
        public void PartidaDTO_SetProperties_ValuesAreSet()
        {
            // Arrange
            var partidaDTO = new PartidaDTO();
            var partida = new Partida
            {
                Id = 1,
                UsuarioId = 10,
                UltimaVez = DateTime.Now,
                JsonMapa = "{\"test\": \"data\"}"
            };

            // Act
            partidaDTO.Id = 1;
            partidaDTO.Partida = partida;

            // Assert
            Assert.Equal(1, partidaDTO.Id);
            Assert.NotNull(partidaDTO.Partida);
            Assert.Equal(partida, partidaDTO.Partida);
            Assert.Equal(1, partidaDTO.Partida.Id);
            Assert.Equal(10, partidaDTO.Partida.UsuarioId);
        }

        [Fact]
        public void PartidaDTO_WithNullPartida_PropertyCanBeNull()
        {
            // Arrange
            var partidaDTO = new PartidaDTO
            {
                Id = 1,
                Partida = null
            };

            // Assert
            Assert.Equal(1, partidaDTO.Id);
            Assert.Null(partidaDTO.Partida);
        }

        [Fact]
        public void PartidaDTO_WithCompletePartida_MapsAllProperties()
        {
            // Arrange
            var usuario = new Usuario
            {
                Id = 5,
                NombreUsuario = "testuser",
                Mail = "test@example.com"
            };

            var partida = new Partida
            {
                Id = 3,
                UsuarioId = 5,
                Usuario = usuario,
                UltimaVez = new DateTime(2025, 10, 11, 10, 30, 0),
                JsonMapa = "{\"map\": \"data\"}"
            };

            var partidaDTO = new PartidaDTO
            {
                Id = 3,
                Partida = partida
            };

            // Assert
            Assert.Equal(3, partidaDTO.Id);
            Assert.NotNull(partidaDTO.Partida);
            Assert.Equal(partida, partidaDTO.Partida);
            Assert.Equal(5, partidaDTO.Partida.UsuarioId);
            Assert.NotNull(partidaDTO.Partida.Usuario);
            Assert.Equal("testuser", partidaDTO.Partida.Usuario.NombreUsuario);
        }

        [Fact]
        public void PartidaDTO_MultipleInstances_AreIndependent()
        {
            // Arrange
            var partida1 = new Partida { Id = 1 };
            var partida2 = new Partida { Id = 2 };

            var dto1 = new PartidaDTO { Id = 1, Partida = partida1 };
            var dto2 = new PartidaDTO { Id = 2, Partida = partida2 };

            // Assert
            Assert.NotEqual(dto1.Id, dto2.Id);
            Assert.NotEqual(dto1.Partida, dto2.Partida);
            Assert.Equal(1, dto1.Partida.Id);
            Assert.Equal(2, dto2.Partida.Id);
        }
    }
}
