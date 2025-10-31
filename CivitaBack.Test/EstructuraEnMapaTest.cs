// using CivitaBack.Data.BO;
// using Xunit;
//
// namespace CivitaBack.Tests
// {
//     public class EstructuraEnMapaTest
//     {
//         [Fact]
//         public void EstructuraEnMapa_Constructor_InitializesProperties()
//         {
//             // Act
//             var estructuraEnMapa = new EstructuraMapa();
//
//             // Assert
//             Assert.Equal(0, estructuraEnMapa.Id);
//             Assert.Equal(0, estructuraEnMapa.PartidaId);
//             Assert.Null(estructuraEnMapa.Partida);
//             Assert.Equal(0, estructuraEnMapa.EstructuraId);
//             Assert.Null(estructuraEnMapa.Estructura);
//         }
//
//         [Fact]
//         public void EstructuraEnMapa_SetProperties_ValuesAreSet()
//         {
//             // Arrange
//             var estructuraEnMapa = new EstructuraMapa();
//             var partida = new PartidaEF { Id = 1 };
//             var estructura = new EstructuraEF { Id = 1 };
//
//             // Act
//             estructuraEnMapa.Id = 1;
//             estructuraEnMapa.PartidaId = 1;
//             estructuraEnMapa.Partida = partida;
//             estructuraEnMapa.EstructuraId = 1;
//             estructuraEnMapa.Estructura = estructura;
//
//             // Assert
//             Assert.Equal(1, estructuraEnMapa.Id);
//             Assert.Equal(1, estructuraEnMapa.PartidaId);
//             Assert.Equal(partida, estructuraEnMapa.Partida);
//             Assert.Equal(1, estructuraEnMapa.EstructuraId);
//             Assert.Equal(estructura, estructuraEnMapa.Estructura);
//         }
//
//         [Fact]
//         public void EstructuraEnMapa_WithNullValues_PropertiesCanBeNull()
//         {
//             // Arrange
//             var estructuraEnMapa = new EstructuraMapa
//             {
//                 Id = 1,
//                 PartidaId = 0,
//                 Partida = null,
//                 EstructuraId = 0,
//                 Estructura = null
//             };
//
//             // Assert
//             Assert.Equal(1, estructuraEnMapa.Id);
//             Assert.Equal(0, estructuraEnMapa.PartidaId);
//             Assert.Null(estructuraEnMapa.Partida);
//             Assert.Equal(0, estructuraEnMapa.EstructuraId);
//             Assert.Null(estructuraEnMapa.Estructura);
//         }
//     }
// }
