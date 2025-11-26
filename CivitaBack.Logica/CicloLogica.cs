using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Excepciones;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Logica.Backgrounds;
using CivitaBack.Utils;

namespace CivitaBack.Logica
{
    public class CicloLogica : ICicloLogica
    {

        private readonly IPartidaRepositorio _partidaRepositorio;
        private readonly IUnidadDeTrabajo _uow;
        private readonly IActualizarRecursosLogica _actualizarRecursosLogica;

        public CicloLogica(IPartidaRepositorio partidaRepositorio, IUnidadDeTrabajo uow, IActualizarRecursosLogica actualizarRecursosLogica)
        {
            _partidaRepositorio = partidaRepositorio;
            _uow = uow;
            _actualizarRecursosLogica = actualizarRecursosLogica;
        }

        public async Task<List<Partida>> EjecutarCicloAsync()
        {
            List<Partida> partidas = await _partidaRepositorio.ObtenerTodasConEstructurasYRecursosAsync();

            if (partidas == null || partidas.Count == 0) return new List<Partida>();

            foreach (var partida in partidas)
            {
                ProcesarRecursosPorEstructurasMapa(partida);

                partida.EstructuraMapa = null;
                partida.Evento = null;
                partida.Tienda = null;
                // partida.LogroPartidas = null;
                // partida.MisionPartidas = null;
                partida.TipEnPartida = null;
                partida.Usuario = null;

                await _partidaRepositorio.Actualizar(partida);
            }

            await _uow.CommitAsync();

            return partidas;
        }

        public async Task PausarCiclo(Partida partida)
        {
            if (partida == null)
                throw new PartidaExcepcion("La partida no puede ser nula.");

            partida.EstaPausada = true;

            await _partidaRepositorio.Actualizar(partida);

            await _uow.CommitAsync();
        }

        public async Task ContinuarCiclo(Partida partida)
        {
            if (partida == null)
                throw new PartidaExcepcion("La partida no puede ser nula.");

            partida.EstaPausada = false;

            await _partidaRepositorio.Actualizar(partida);

            await _uow.CommitAsync();
        }

        private void ProcesarRecursosPorEstructurasMapa(Partida partida)
        {
            if (partida == null || partida.Recursos == null)
                throw new Exception();

            int totalEnergia = 0;
            int totalEcoCoins = 0;
            int totalFelicidad = 0;
            int totalContaminacion = 0;
            int nuevaPoblacion = 0;

            if (partida.EstructuraMapa != null)
            {
                foreach (var estructuraMapa in partida.EstructuraMapa)
                {
                    if (estructuraMapa.Estructura == null || estructuraMapa.Estructura.TipoEstructura == null)
                        continue;

                    var estructura = estructuraMapa.Estructura;
                    var tipoEstructura = estructura.TipoEstructura;

                    totalEnergia += tipoEstructura.EnergiaPorCiclo;
                    totalEcoCoins += tipoEstructura.DineroPorCiclo;
                    totalFelicidad += estructura.FelicidadCiclo;
                    totalContaminacion += estructura.ContaminacionCiclo;

                    if (tipoEstructura.Capacidad > 0)
                        nuevaPoblacion += tipoEstructura.Capacidad;
                }
            }

            partida.Recursos.Poblacion = nuevaPoblacion;

            if (partida.Recursos.Contaminacion > 80)
            {
                totalFelicidad -= 3;
            }

            if (partida.Recursos.Contaminacion < 10)
            {
                totalFelicidad += 3;
            }

            _actualizarRecursosLogica.ActualizarRecursosAsync(
                partida,
                totalFelicidad,
                totalContaminacion,
                totalEcoCoins,
                totalEnergia
            );
        }



    }

}
