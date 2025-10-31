// using CivitaBack.Data.BO;
// using Xunit;
//
// namespace CivitaBack.Tests
// {
//     public class TipEnPartidaTest
//     {
//         [Fact]
//         public void TipEnPartida_Constructor_InitializesProperties()
//         {
//             // Act
//             var tipEnPartida = new TipEnPartida();
//
//             // Assert
//             Assert.Equal(0, tipEnPartida.Id);
//             Assert.Equal(0, tipEnPartida.PartidaId);
//             Assert.Null(tipEnPartida.Partida);
//             Assert.Equal(0, tipEnPartida.TipId);
//             Assert.Null(tipEnPartida.Tip);
//         }
//
//         [Fact]
//         public void TipEnPartida_SetProperties_ValuesAreSet()
//         {
//             // Arrange
//             var tipEnPartida = new TipEnPartida();
//             var partida = new PartidaEF { Id = 1 };
//             var tip = new Tip { Id = 1 };
//
//             // Act
//             tipEnPartida.Id = 1;
//             tipEnPartida.PartidaId = 1;
//             tipEnPartida.Partida = partida;
//             tipEnPartida.TipId = 1;
//             tipEnPartida.Tip = tip;
//
//             // Assert
//             Assert.Equal(1, tipEnPartida.Id);
//             Assert.Equal(1, tipEnPartida.PartidaId);
//             Assert.Equal(partida, tipEnPartida.Partida);
//             Assert.Equal(1, tipEnPartida.TipId);
//             Assert.Equal(tip, tipEnPartida.Tip);
//         }
//
//         [Fact]
//         public void TipEnPartida_WithNullValues_PropertiesCanBeNull()
//         {
//             // Arrange
//             var tipEnPartida = new TipEnPartida
//             {
//                 Id = 1,
//                 PartidaId = 0,
//                 Partida = null,
//                 TipId = 0,
//                 Tip = null
//             };
//
//             // Assert
//             Assert.Equal(1, tipEnPartida.Id);
//             Assert.Equal(0, tipEnPartida.PartidaId);
//             Assert.Null(tipEnPartida.Partida);
//             Assert.Equal(0, tipEnPartida.TipId);
//             Assert.Null(tipEnPartida.Tip);
//         }
//     }
// }
