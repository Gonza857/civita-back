using CivitaBack.Domain.Entidades;
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
        Usuario u = TestData.CrearUsuarioBase();
        Partida p = TestData.CrearPartida(u);
        p.Experiencia = 151;
        this._nivelLogica.SubirNivel(p);
    }
    
    [Fact]
    public void SaberSiPuedeSubirNivel_Sale_MAL()
    {
        Usuario u = TestData.CrearUsuarioBase();
        Partida p = TestData.CrearPartida(u);
        p.Experiencia = 99;
        Assert.Throws<Exception>(() => this._nivelLogica.SubirNivel(p));
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