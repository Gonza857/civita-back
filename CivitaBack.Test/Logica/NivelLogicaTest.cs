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
        int nivelActual = 0;
        this._nivelLogica.SubirNivel(ref nivelActual, 151);
    }
    
    [Fact]
    public void SaberSiPuedeSubirNivel_Sale_MAL()
    {
        int nivelActual = 0;
        Assert.Throws<Exception>(() => this._nivelLogica.SubirNivel(ref nivelActual, 99));
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