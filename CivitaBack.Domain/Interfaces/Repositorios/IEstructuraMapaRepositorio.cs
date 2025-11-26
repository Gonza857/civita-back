using CivitaBack.Domain.Entidades;

namespace CivitaBack.Domain.Interfaces.Repositorios
{
    public interface IEstructuraMapaRepositorio : IRepositorioBase<EstructuraMapa>
    {
        void AgregarUnica(EstructuraMapa em);
        void RemoverEliminadas(List<EstructuraMapa> emList);
        Task AgregarNuevas(List<EstructuraMapa> emList);
        Task EliminarPorPartidaIdAsync(int partidaId);
        Task AgregarVariasAsync(List<EstructuraMapa> estructuras);
        Task<EstructuraMapa?> ObtenerCoincidenteAsync(
                int partidaId,
                int estructuraId,
                int x,
                int y); 
        Task EliminarAsync(EstructuraMapa entidad);
    }
}
