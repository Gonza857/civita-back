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
            List<Partida> partidas = await _cicloRepositorio.ObtenerPartidasConEstructuras();

            if (partidas.Count == 0) return;

            foreach (var partida in partidas)
            {
                ProcesarPartida(partida);
            }

            await _cicloRepositorio.GuardarCambiosAsync();
        }

        private void ProcesarPartida(Partida partida)
        {
            if (partida == null || partida.Recursos == null)
                throw new Exception();
            
            Recurso recursosPartida = partida.Recursos;
            int nuevaPoblacion = 0;
                
            foreach (var estructuraEnMapa in partida.EstructuraMapa)
            {

                if (estructuraEnMapa.Estructura == null || estructuraEnMapa.Estructura.TipoEstructura == null)
                    continue;

                var estructura = estructuraEnMapa.Estructura;
                var tipoEstructura = estructura.TipoEstructura;

                recursosPartida.Energia = 
                    ActualizarRecurso(recursosPartida.Energia, tipoEstructura.EnergiaPorCiclo);
                recursosPartida.EcoCoins = 
                    ActualizarRecurso(recursosPartida.EcoCoins, tipoEstructura.DineroPorCiclo);
                recursosPartida.Felicidad = 
                    ActualizarRecurso(recursosPartida.Felicidad, estructura.FelicidadCiclo);
                recursosPartida.Contaminacion = 
                    ActualizarRecurso(recursosPartida.Contaminacion, estructura.ContaminacionCiclo);

                if (tipoEstructura.Capacidad > 0)
                    nuevaPoblacion += tipoEstructura.Capacidad;
            }

            partida.Recursos.Poblacion = nuevaPoblacion;
        }

        private int ActualizarRecurso(int cantidadInicial, int cambio)
        {
            // if (recursoPartida.Nombre.Equals("Contaminación") && recursoPartida.Cantidad > 90)
            // {
            //     var felicidad = partida.Recursos.FirstOrDefault(r => r.Nombre == "Felicidad");
            //  
            //     if (felicidad != null && felicidad.Cantidad >= 5)
            //         felicidad.Cantidad -= 5;
            // }

            cantidadInicial += cambio;

            if (cantidadInicial < 0)
                cantidadInicial = 0;
            
            return cantidadInicial;

        }
    }

}
