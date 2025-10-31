// using CivitaBack.Data.BO;
// using Xunit;
//
// namespace CivitaBack.Tests
// {
//     public class TipoTipTest
//     {
//         [Fact]
//         public void TipoTip_Constructor_InitializesProperties()
//         {
//             // Act
//             var tipoTip = new TipoTip();
//
//             // Assert
//             Assert.Equal(0, tipoTip.Id);
//             Assert.Null(tipoTip.Descripcion);
//             
//         }
//
//         [Fact]
//         public void TipoTip_SetProperties_ValuesAreSet()
//         {
//             // Arrange
//             var tipoTip = new TipoTip();
//
//             // Act
//             tipoTip.Id = 1;
//             tipoTip.Descripcion = "Test Tip Type";
//
//             // Assert
//             Assert.Equal(1, tipoTip.Id);
//             Assert.Equal("Test Tip Type", tipoTip.Descripcion);
//         }
//
//         [Fact]
//         public void TipoTip_WithNullValues_PropertiesCanBeNull()
//         {
//             // Arrange
//             var tipoTip = new TipoTip
//             {
//                 Id = 1,
//                 Descripcion = null,
//                 
//             };
//
//             // Assert
//             Assert.Equal(1, tipoTip.Id);
//             Assert.Null(tipoTip.Descripcion);
//             
//         }
//     }
// }
