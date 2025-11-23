using CivitaBack.Domain.Entidades;

namespace CivitaBack.Domain.Interfaces.Logica
{
    public interface IMapaLogica
    {
        Task<Partida?> ObtenerMapaAsync(int partidaId);

        Task ActualizarMapaDePartidaAsync(int partidaId, string jsonMapa, List<EstructuraMapa>? estructuras);
    }
}
