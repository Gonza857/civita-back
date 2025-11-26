using CivitaBack.Data.Repositorio;
using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Logica;
using CivitaBack.Utils;
using Moq;

namespace CivitaBack.Tests.Logica;

public class NivelLogicaTest
{
    private readonly INivelLogica _nivelLogica;
    
    private readonly Mock<IPartidaRepositorio> _mockPartidarepositorio;
    private readonly Mock<IUnidadDeTrabajo> _mockUnidadDeTrabajo;


    public NivelLogicaTest()
    {
        _mockUnidadDeTrabajo = new Mock<IUnidadDeTrabajo>();
        _mockPartidarepositorio = new Mock<IPartidaRepositorio>();
        
        _nivelLogica = new NivelLogica(
                _mockPartidarepositorio.Object,
                _mockUnidadDeTrabajo.Object
            );
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
    public void CantidadNecesariaParaSubirNivel_Retorna_Uno()
    {
        // Assert
        int nivelActual = 0, experienciaActual = 99;
        int experienciaEsperada = -99;

        // Act
        int experienciaFaltanteObtenida = _nivelLogica.ObtenerExperienciaFaltanteParaSiguienteNivel(nivelActual, experienciaActual);

        // Arrange
        Assert.Equal(experienciaEsperada, experienciaFaltanteObtenida);
    }


}