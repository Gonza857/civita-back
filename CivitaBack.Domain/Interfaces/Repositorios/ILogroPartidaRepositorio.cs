using CivitaBack.Domain.Entidades;

namespace CivitaBack.Domain.Interfaces.Repositorios
{
    public interface ILogroPartidaRepositorio
    {
        Task<List<Logro>> ObtenerLogrosIncompletos(int partidaId);
        Task<List<Logro>> ObtenerLogrosCompletos(int partidaId);

        Task ReiniciarLogrosPartida(int partidaId);
        Task<List<Logro>> ObtenerLogrosParaReclamarQueNoEstenCumplidos(List<int> idsLogros);

        Task<List<Logro>> ObtenerLogrosNoCumplidos(int idPartida);

        Task AgregarVarios(List<LogroPartida> entidades);
    }
}
