using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Interfaces.Logica;

namespace CivitaBack.Logica
{
    public class ActualizarRecursosLogica : IActualizarRecursosLogica
    {

        public void ActualizarRecursosAsync(Partida partida, int cambioFelicidad, int cambioContaminacion, int cambioEcoCoins, int cambioEnergia)
        {
            if (partida.Recursos == null) return;

            partida.Recursos.Felicidad = AplicarCambio(partida.Recursos.Felicidad, cambioFelicidad, true);
            partida.Recursos.Contaminacion = AplicarCambio(partida.Recursos.Contaminacion, cambioContaminacion, true);
            partida.Recursos.EcoCoins = AplicarCambio(partida.Recursos.EcoCoins, cambioEcoCoins, false);
            partida.Recursos.Energia = AplicarCambio(partida.Recursos.Energia, cambioEnergia, true);
        }

        private int AplicarCambio(int cantidadInicial, int cambio, bool tieneLimite)
        {
            cantidadInicial += cambio;
            if (cantidadInicial < 0) cantidadInicial = 0;
            if (tieneLimite && cantidadInicial > 100) cantidadInicial = 100;
            return cantidadInicial;
        }
    }
}
