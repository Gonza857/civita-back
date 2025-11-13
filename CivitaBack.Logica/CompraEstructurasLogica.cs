using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Excepciones;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Logica.Interfaces;
using CivitaBack.Utils;

namespace CivitaBack.Logica
{
    public class CompraEstructurasLogica : ICompraEstructurasLogica
    {

        private readonly IPartidaRepositorio _repositorioPartida;
        private readonly IEstructuraRepositorio _estructuraRepositorio;
        private readonly IAccesoUsuarios _accesoUsuarios;
        private readonly IActualizarRecursosLogica _actualizarRecursosLogica;
        private readonly IUnidadDeTrabajo _uow;

        public CompraEstructurasLogica(
            IPartidaRepositorio repositorioPartida,
            IEstructuraRepositorio estructuraRepositorio,
            IAccesoUsuarios accesoUsuarios,
            IActualizarRecursosLogica actualizarRecursosLogica,
            IUnidadDeTrabajo uow)
        {
            _repositorioPartida = repositorioPartida;
            _estructuraRepositorio = estructuraRepositorio;
            _accesoUsuarios = accesoUsuarios;
            _actualizarRecursosLogica = actualizarRecursosLogica;
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

            _accesoUsuarios.ValidarAcceso(partida.UsuarioId);

            int costo = estructura.CostoDinero;

            if (partida.Recursos.EcoCoins < costo)
                throw new PartidaExcepcion("Dinero insuficiente.");

            int cambioEcoCoins = -costo;

            _actualizarRecursosLogica.ActualizarRecursosAsync(partida,0,0, cambioEcoCoins, 0);

            await _repositorioPartida.Actualizar(partida);

            await _uow.CommitAsync();

            return partida.Recursos.EcoCoins;
        }
    }
}
