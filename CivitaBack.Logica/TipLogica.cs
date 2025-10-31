using CivitaBack.Data.DTO;
using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Excepciones;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Utils;

namespace CivitaBack.Logica
{
    public class TipLogica : ITipLogica
    {
        private readonly ITipsRepositorio _tipsRepositorio;
        private readonly ITipoTipRepositorio _tiposTipRepositorio;
        private readonly IUnidadDeTrabajo _unidadDeTrabajo;

        public TipLogica(ITipsRepositorio itr, ITipoTipRepositorio ittr, IUnidadDeTrabajo iudt)
        {
            _tipsRepositorio = itr;
            _tiposTipRepositorio = ittr;
            _unidadDeTrabajo = iudt;
        }

        public async Task<List<Tip>> ObtenerMsjPorIdTipo(int id)
        {
            return await _tipsRepositorio.ObtenerMsjPorIdTipo(id);
        }

        public async Task<List<Tip>> Listado()
        {
            return await _tipsRepositorio.ObtenerTodos();
        }

        public async Task Crear(Tip tip)
        {
            var tipoTip = await this._tiposTipRepositorio.ObtenerPorId(tip.TipoId);
            if (tipoTip == null)
                throw new Exception("No se pudo guardar el tip.");

            var nuevoTip = new Tip
            {
                TipoTip = tipoTip,
                Expresion = tip.Expresion,
                ElementoAdicional = tip.ElementoAdicional,
                Mensaje = tip.Mensaje,
                EfectoFiltro = tip.EfectoFiltro,
            };

            try
            {
                await this._tipsRepositorio.Agregar(nuevoTip);
                await this._unidadDeTrabajo.CommitAsync();
            }
            catch (Exception ex)
            {
                throw new ErrorInternoExcepction("Ocurrió un error al crear un Tip");
            }
        }

        public async Task Actualizar(Tip tip, int id)
        {
            var tipoTip = await this._tiposTipRepositorio.ObtenerPorId(tip.TipoId);
            if (tipoTip == null)
                throw new Exception("No se pudo guardar el tip.");

            var tipDb = await this._tipsRepositorio.ObtenerPorId(id);
            if (tipDb == null)
                throw new Exception("No se pudo guardar el tip.");

            tipDb.TipoTip = tipoTip;
            tipDb.Expresion = tip.Expresion;
            tipDb.ElementoAdicional = tip.ElementoAdicional;
            tipDb.Mensaje = tip.Mensaje;
            tipDb.EfectoFiltro = tip.EfectoFiltro;

            try
            {
                await this._tipsRepositorio.Actualizar(tipDb);
                await this._unidadDeTrabajo.CommitAsync();
            }
            catch (Exception ex)
            {
                throw new ErrorInternoExcepction("Ocurrió un error al Actualizar un Tip");
            }
        }

        public async Task<Tip?> ObtenerPorIdTipo(int id)
        {
            return await this._tipsRepositorio.ObtenerPorId(id);
        }
    }
}