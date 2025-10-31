using CivitaBack.Domain.Entidades;

namespace CivitaBack.Domain.Interfaces.Repositorios
{
    public interface IEstructuraMapaRepositorio
    {
        void AgregarUnica(EstructuraMapa em);
        void RemoverEliminadas(List<EstructuraMapa> emList);
        void AgregarNuevas(List<EstructuraMapa> emList);
        Task GuardarCambios();
        Task EliminarPorPartidaIdAsync(int partidaId);
        Task AgregarVariasAsync(List<EstructuraMapa> estructuras);
        Task<EstructuraMapa?> ObtenerCoincidenteAsync(
                int partidaId,
                int estructuraId,
                int x,
                int y,
                int width,
                int height); 
        Task EliminarAsync(EstructuraMapa entidad);


    }
}
