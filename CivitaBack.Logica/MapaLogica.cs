using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Excepciones;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Logica.Interfaces;
using CivitaBack.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CivitaBack.Logica
{
    public class MapaLogica : IMapaLogica
    {
        private readonly IPartidaRepositorio _repositorioPartida;
        private readonly IEstructuraMapaRepositorio _repositorioEstructuraMapa;
        private readonly IAccesoUsuarios _accesoUsuarios;
        private readonly IUnidadDeTrabajo _uow;
        private readonly IServiceProvider _serviceProvider;

        public MapaLogica(
            IPartidaRepositorio repositorioPartida,
            IEstructuraMapaRepositorio repositorioEstructuraMapa,
            IAccesoUsuarios accesoUsuarios,
            IUnidadDeTrabajo uow,
            IServiceProvider serviceProvider)
        {
            _repositorioPartida = repositorioPartida;
            _repositorioEstructuraMapa = repositorioEstructuraMapa;
            _accesoUsuarios = accesoUsuarios;
            _uow = uow;
            _serviceProvider = serviceProvider;
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

            var x = await this._repositorioPartida.ObtenerPartidaConEstructuras(partida.Id);
            var estructuras = x!.EstructuraMapa ?? new List<EstructuraMapa>();

            string mapaReconstruido = await LectorMapa.ObtenerMapaFinal(estructuras);
            partida.JsonMapa = mapaReconstruido;

            await _repositorioPartida.ActualizarMapaAsync(partida);

            await _uow.CommitAsync();

            return partida;
        }

        /// <summary>
        /// Guarda el mapa de la partida. Si tiene estructuras, las actualiza.
        /// </summary>
        /// <param name="dto">GuardarMapaDTO</param>
        public async Task ActualizarMapaDePartidaAsync(
            int partidaId, 
            string jsonMapa,
            List<EstructuraMapa>? estructuras
            )
        {
            const int MAX_REINTENTOS = 3;
            
            if (jsonMapa == null || partidaId <= 0 || estructuras == null)
                throw new PartidaExcepcion("Datos inválidos.");

            for (int intento = 0; intento < MAX_REINTENTOS; intento++)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    
                    var repoPartida = services.GetRequiredService<IPartidaRepositorio>();
                    var repoEstructuraMapa = services.GetRequiredService<IEstructuraMapaRepositorio>();
                    var uow = services.GetRequiredService<IUnidadDeTrabajo>();

                    try
                    {
                        var partida = await repoPartida.ObtenerPartidaConMapaAsync(partidaId);

                        if (partida == null)
                            throw new PartidaExcepcion("Partida no encontrada.");

                        partida.JsonMapa = jsonMapa;
                        partida.UltimaVez = DateTime.UtcNow;

                        if (estructuras.Any())
                        {
                            await repoEstructuraMapa.EliminarPorPartidaIdAsync(partida.Id);
                            await repoEstructuraMapa.AgregarNuevas(estructuras, partida.Id);
                        }

                        await uow.CommitAsync();
                        return;
                    }
                    catch (DbUpdateConcurrencyException) when (intento < MAX_REINTENTOS - 1)
                    {
                        await Task.Delay(50);
                    }
                }
            }

            throw new PartidaExcepcion("Error de concurrencia al guardar el mapa. Reintentos agotados.");
        }
    }
}