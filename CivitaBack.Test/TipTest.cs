using CivitaBack.Data.BO;
using Xunit;

namespace CivitaBack.Tests
{
    public class TipTest
    {
        [Fact]
        public void Tip_Constructor_InitializesProperties()
        {
            // Act
            var tip = new Tip();

            // Assert
            Assert.Equal(0, tip.Id);
            Assert.Null(tip.Mensaje);
            Assert.Equal(0, tip.TipoTipId);
            Assert.Null(tip.TipoTip);
            Assert.Null(tip.TipEnPartida);
        }

        [Fact]
        public void Tip_SetProperties_ValuesAreSet()
        {
            // Arrange
            var tip = new Tip();
            var tipoTip = new TipoTip { Id = 1 };

            // Act
            tip.Id = 1;
            tip.Mensaje = "Test Tip Message";
            tip.TipoTipId = 1;
            tip.TipoTip = tipoTip;

            // Assert
            Assert.Equal(1, tip.Id);
            Assert.Equal("Test Tip Message", tip.Mensaje);
            Assert.Equal(1, tip.TipoTipId);
            Assert.Equal(tipoTip, tip.TipoTip);
        }

        [Fact]
        public void Tip_WithNullValues_PropertiesCanBeNull()
        {
            // Arrange
            var tip = new Tip
            {
                Id = 1,
                Mensaje = null,
                TipoTipId = 0,
                TipoTip = null,
                TipEnPartida = null
            };

            // Assert
            Assert.Equal(1, tip.Id);
            Assert.Null(tip.Mensaje);
            Assert.Equal(0, tip.TipoTipId);
            Assert.Null(tip.TipoTip);
            Assert.Null(tip.TipEnPartida);
        }
    }
}
