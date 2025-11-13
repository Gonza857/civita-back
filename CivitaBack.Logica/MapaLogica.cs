using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Excepciones;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Logica.Interfaces;
using CivitaBack.Utils;

namespace CivitaBack.Logica
{
    public class MapaLogica : IMapaLogica
    {

        private readonly IPartidaRepositorio _repositorioPartida;
        private readonly IEstructuraMapaRepositorio _repositorioEstructuraMapa;
        private readonly IAccesoUsuarios _accesoUsuarios;
        private readonly IUnidadDeTrabajo _uow;

        public MapaLogica(
            IPartidaRepositorio repositorioPartida,
            IEstructuraMapaRepositorio repositorioEstructuraMapa,
            IAccesoUsuarios accesoUsuarios,
            IUnidadDeTrabajo uow)
        {
            _repositorioPartida = repositorioPartida;
            _repositorioEstructuraMapa = repositorioEstructuraMapa;
            _accesoUsuarios = accesoUsuarios;
            _uow = uow;
        }

        /// <summary>
        /// Obtiene el mapa de una partida.
        /// </summary>  
        /// <param name="partidaId">ID de partida</param>
        public async Task<Partida?> ObtenerMapaAsync(int partidaId)
        {
            var partida = await _repositorioPartida.ObtenerPartidaConMapaAsync(partidaId);
            if (partida == null) return null;

            this._accesoUsuarios.ValidarAcceso(partida.UsuarioId);

            if (!string.IsNullOrWhiteSpace(partida.JsonMapa)) return partida;

            var mapaReconstruido = await _repositorioPartida.ObtenerMapaJsonPorPartidaIdAsync(partidaId);
            partida.JsonMapa = mapaReconstruido;

            await _repositorioPartida.ActualizarMapaAsync(partida);

            await _uow.CommitAsync();

            return partida;
        }

        /// <summary>
        /// Guarda el mapa de la partida. Si tiene estructuras, las actualiza.
        /// </summary>
        /// <param name="dto">GuardarMapaDTO</param>
        public async Task ActualizarMapaDePartidaAsync(int partidaId, string jsonMapa, List<EstructuraMapa>? estructuras)
        {
            if (jsonMapa == null || partidaId <= 0 || estructuras == null)
                throw new PartidaExcepcion("Ocurrió un error al guardar el mapa: Datos inválidos.");

            var partida = await _repositorioPartida.ObtenerPartidaConMapaAsync(partidaId);
            if (partida == null)
                throw new PartidaExcepcion("Ocurrió un error al guardar el mapa: No existe la partida.");

            this._accesoUsuarios.ValidarAcceso(partida.UsuarioId);

            partida.JsonMapa = jsonMapa;
            partida.UltimaVez = DateTime.UtcNow;
            await _repositorioPartida.ActualizarMapaAsync(partida);

            if (estructuras.Any())
            {
                // 1️⃣ Eliminar estructuras viejas de esa partida
                await _repositorioEstructuraMapa.EliminarPorPartidaIdAsync(partida.Id);

                // 2️⃣ Agregar las nuevas
                var nuevas = estructuras.Select(e => new EstructuraMapa
                {
                    PartidaId = partida.Id,
                    EstructuraId = e.EstructuraId,
                    X = e.X,
                    Y = e.Y,
                    Width = e.Width,
                    Height = e.Height
                }).ToList();

                await _repositorioEstructuraMapa.AgregarNuevas(nuevas);

                // 3️⃣ Guardar cambios
                await this._uow.CommitAsync();
            }
            else
            {
                await this._uow.CommitAsync();
            }
        }
    }
}
