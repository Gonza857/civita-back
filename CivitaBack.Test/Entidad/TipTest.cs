// using CivitaBack.Data.BO;
// using Xunit;
//
// namespace CivitaBack.Tests
// {
//     public class TipTest
//     {
//         [Fact]
//         public void Tip_Constructor_InitializesProperties()
//         {
//             var tip = new Tip();
//
//             Assert.Equal(0, tip.Id);
//             Assert.Null(tip.Mensaje);
//             Assert.Equal(0, tip.TipoId); 
//             Assert.Null(tip.TipoTip);
//             Assert.Null(tip.TipEnPartida);
//         }
//
//         [Fact]
//         public void Tip_SetProperties_ValuesAreSet()
//         {
//             var tip = new Tip();
//             var tipoTip = new TipoTip { Id = 1 };
//
//             tip.Id = 1;
//             tip.Mensaje = "Test Tip Message";
//             tip.TipoId = 1; 
//             tip.TipoTip = tipoTip;
//
//             Assert.Equal(1, tip.Id);
//             Assert.Equal("Test Tip Message", tip.Mensaje);
//             Assert.Equal(1, tip.TipoId); 
//             Assert.Equal(tipoTip, tip.TipoTip);
//         }
//
//         [Fact]
//         public void Tip_WithNullValues_PropertiesCanBeNull()
//         {
//             var tip = new Tip
//             {
//                 Id = 1,
//                 Mensaje = null,
//                 TipoId = 0, 
//                 TipoTip = null,
//                 TipEnPartida = null
//             };
//
//             Assert.Equal(1, tip.Id);
//             Assert.Null(tip.Mensaje);
//             Assert.Equal(0, tip.TipoId); 
//             Assert.Null(tip.TipoTip);
//             Assert.Null(tip.TipEnPartida);
//         }
//
//     }
// }
