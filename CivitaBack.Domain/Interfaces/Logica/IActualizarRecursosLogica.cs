using CivitaBack.Domain.Entidades;

namespace CivitaBack.Domain.Interfaces.Logica
{
    public interface IActualizarRecursosLogica
    {
        void ActualizarRecursosAsync(
        Partida partida,
        int cambioFelicidad,
        int cambioContaminacion,
        int cambioEcoCoins,
        int cambioEnergia);

    }
}
