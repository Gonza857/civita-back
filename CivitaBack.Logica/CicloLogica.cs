using CivitaBack.Data.BO;
using CivitaBack.Data.Repositorio;

namespace CivitaBack.Logica
{
    public interface ICicloLogica
    {
        Task EjecutarCicloAsync();
    }
    public class CicloLogica : ICicloLogica
    {

        private readonly ICicloRepositorio _cicloRepositorio;

        public CicloLogica(ICicloRepositorio cicloRepositorio)
        {
            _cicloRepositorio = cicloRepositorio;
        }

        public async Task EjecutarCicloAsync()
        {
            var partidas = await _cicloRepositorio.ObtenerPartidasConEstructuras();

            foreach (var partida in partidas)
            {
                ProcesarPartida(partida);
            }

            await _cicloRepositorio.GuardarCambiosAsync();
        }

        private void ProcesarPartida(Partida partida)
        {
            int nuevaPoblacion = 0;
                
            foreach (var estructuraEnMapa in partida.EstructuraEnMapa)
            {
                var estructura = estructuraEnMapa.Estructura;
                var tipoEstructura = estructura.TipoEstructura;

                ActualizarRecurso(partida, "Energia", tipoEstructura.EnergiaPorCiclo);
                ActualizarRecurso(partida, "EcoCoins", tipoEstructura.DineroPorCiclo);
                ActualizarRecurso(partida, "Felicidad", estructura.FelicidadCiclo);
                ActualizarRecurso(partida, "Contaminacion", estructura.ContaminacionCiclo);

                if (tipoEstructura.Capacidad > 0)
                    nuevaPoblacion += tipoEstructura.Capacidad;

            }

            var poblacionFinal = partida.Recursos.FirstOrDefault(r => r.Nombre == "Poblacion");
            if (poblacionFinal != null)
            {
                poblacionFinal.Cantidad = nuevaPoblacion;
            }
        }

        private void ActualizarRecurso(Partida partida, string recurso, int cambio)
        {
            var recursoPartida = partida.Recursos.FirstOrDefault(r => r.Nombre == recurso);

            if (recursoPartida == null) return;

            recursoPartida.Cantidad += cambio;

            if (recursoPartida.Cantidad < 0)
            {
                recursoPartida.Cantidad = 0; // Por ahora voy a evitar que los recursos sean negativos
            }

        }
    }

}
