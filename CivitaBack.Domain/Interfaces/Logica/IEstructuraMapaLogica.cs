using CivitaBack.Domain.Entidades;

namespace CivitaBack.Domain.Interfaces.Logica
{
    public interface IEstructuraMapaLogica
    {
        Task ReiniciarEstructurasDePartida(int idPartida);
        Task EliminarEstructuraAsync(EstructuraMapa dto);
    }
}
