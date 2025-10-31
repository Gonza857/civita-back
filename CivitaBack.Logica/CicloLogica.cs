using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Utils;

namespace CivitaBack.Logica
{
    public class CicloLogica : ICicloLogica
    {

        private readonly IPartidaRepositorio _partidaRepositorio;
        private readonly IUnidadDeTrabajo _uow;

        public CicloLogica(IPartidaRepositorio partidaRepositorio, IUnidadDeTrabajo uow)
        {
            _partidaRepositorio = partidaRepositorio;
            _uow = uow;
        }

        public async Task<List<Partida>> EjecutarCicloAsync()
        {
            List<Partida> partidas = await _partidaRepositorio.ObtenerTodasConEstructurasYRecursosAsync();

            if (partidas == null || partidas.Count == 0) return new List<Partida>();

            foreach (var partida in partidas)
            {
                ProcesarPartida(partida);
                await _partidaRepositorio.Actualizar(partida);
            }

            await _uow.CommitAsync();

            return partidas;
        }

        private void ProcesarPartida(Partida partida)
        {
            if (partida == null || partida.Recursos == null)
                throw new Exception();

            Recurso recursosPartida = partida.Recursos;
            int nuevaPoblacion = 0;

            if (partida.EstructuraMapa != null)
            {
                foreach (var estructuraEnMapa in partida.EstructuraMapa)
                {
                    if (estructuraEnMapa.Estructura == null || estructuraEnMapa.Estructura.TipoEstructura == null)
                        continue;

                    var estructura = estructuraEnMapa.Estructura;
                    var tipoEstructura = estructura.TipoEstructura;

                    recursosPartida.Energia =
                        ActualizarRecurso(recursosPartida.Energia, tipoEstructura.EnergiaPorCiclo, true);
                    recursosPartida.EcoCoins =
                        ActualizarRecurso(recursosPartida.EcoCoins, tipoEstructura.DineroPorCiclo, false);
                    recursosPartida.Felicidad =
                        ActualizarRecurso(recursosPartida.Felicidad, estructura.FelicidadCiclo, true);
                    recursosPartida.Contaminacion =
                        ActualizarRecurso(recursosPartida.Contaminacion, estructura.ContaminacionCiclo, true);

                    if (tipoEstructura.Capacidad > 0)
                        nuevaPoblacion += tipoEstructura.Capacidad;
                }
            }

            if (recursosPartida.Contaminacion > 70)
            {
                recursosPartida.Felicidad = ActualizarRecurso(recursosPartida.Felicidad, -5, true);
            }

            if (recursosPartida.Contaminacion < 10)
            {
                recursosPartida.Felicidad = ActualizarRecurso(recursosPartida.Felicidad, 3, true);
            }

            partida.Recursos.Poblacion = nuevaPoblacion;
        }

        private int ActualizarRecurso(int cantidadInicial, int cambio, bool tieneLimite)
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

            if (tieneLimite && cantidadInicial > 100)
                cantidadInicial = 100;

            return cantidadInicial;

        }
    }

}
