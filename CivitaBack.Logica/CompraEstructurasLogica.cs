using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Excepciones;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Utils;

namespace CivitaBack.Logica
{
    public class CompraEstructurasLogica : ICompraEstructurasLogica
    {

        private readonly IPartidaRepositorio _repositorioPartida;
        private readonly IEstructuraRepositorio _estructuraRepositorio;
        private readonly IUnidadDeTrabajo _uow;

        public CompraEstructurasLogica(
            IPartidaRepositorio repositorioPartida,
            IEstructuraRepositorio estructuraRepositorio,
            IUnidadDeTrabajo uow)
        {
            _repositorioPartida = repositorioPartida;
            _estructuraRepositorio = estructuraRepositorio;
            _uow = uow;
        }

        public async Task<int> ComprarEstructuraAsync(int partidaId, int estructuraId)
        {
            Partida? partida = await _repositorioPartida.ObtenerPorId(partidaId);
            Estructura? estructura = await _estructuraRepositorio.ObtenerPorId(estructuraId);

            if (partida == null || partida.Recursos == null)
                throw new PartidaExcepcion("Partida inválida o recursos no encontrados.");
            if (estructura == null)
                throw new PartidaExcepcion("Estructura no encontrada.");

            int costo = estructura.CostoDinero;

            if (partida.Recursos.EcoCoins < costo)
                throw new PartidaExcepcion("Dinero insuficiente.");

            partida.Recursos.EcoCoins -= costo;

            await _repositorioPartida.Actualizar(partida);

            await _uow.CommitAsync();

            return partida.Recursos.EcoCoins;
        }
    }
}
