using CivitaBack.Domain.Entidades;

namespace CivitaBack.Logica;

public interface INivelLogica
{
    Task SaberSiPuedeSubirNivel(Recurso recursos);

}

public class NivelLogica : INivelLogica
{
    public Task SaberSiPuedeSubirNivel(Recurso recursos)
    {
        throw new NotImplementedException();
    }
    
    private void 
}