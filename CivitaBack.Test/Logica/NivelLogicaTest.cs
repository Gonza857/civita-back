using CivitaBack.Logica;

namespace CivitaBack.Tests.Logica;

public class NivelLogicaTest
{
    private readonly INivelLogica _nivelLogica;


    public NivelLogicaTest()
    {
        _nivelLogica = new NivelLogica();

    }

    [Fact]
    public void SaberSiPuedeSubirNivel_Sale_OK()
    {
        this._nivelLogica.SubirNivel(0, 151);
    }
    
    [Fact]
    public void SaberSiPuedeSubirNivel_Sale_MAL()
    {
        Assert.Throws<Exception>(() => this._nivelLogica.SubirNivel(0, 99));
    }
    
    [Fact]
    public void CantidadNecesariaParaSubirNivel_Retorna_Uno()
    {
        // Assert
        int nivelActual = 0, experienciaActual = 99;
        int experienciaEsperada = 1;

        // Act
        int experienciaFaltanteObtenida = _nivelLogica.ObtenerExperienciaFaltanteParaSiguienteNivel(nivelActual, experienciaActual);

        // Arrange
        Assert.Equal(experienciaEsperada, experienciaFaltanteObtenida);
    }


}