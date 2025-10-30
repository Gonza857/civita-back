using CivitaBack.Data.BO;
using CivitaBack.Data.DTO;
using CivitaBack.Data.Repositorio;
using CivitaBack.Utils;

namespace CivitaBack.Logica
{

    public interface ITipsLogica
    {
        Task<List<TipDTO>> ObtenerMsjPorIdTipo(int id);
        Task<List<TipDTO>> Listado();
        Task Crear(TipDTO tip);
        Task Actualizar (TipDTO tip, int id);
        
        Task<TipDTO?> ObtenerPorIdTipo(int id);
    }
    public class TipsLogica : ITipsLogica, IParser<Tip, TipDTO>
    {
        private readonly ITipsRepositorio _tipsRepositorio;
        private readonly ITipoTipRepositorio _tiposTipRepositorio;
        public TipsLogica(ITipsRepositorio itr, ITipoTipRepositorio ittr)
        {
            _tipsRepositorio = itr;
            _tiposTipRepositorio = ittr;
        }

        public async Task<List<TipDTO>> ObtenerMsjPorIdTipo(int id)
        {
                List<Tip> listadoTipsSegunTipo = await _tipsRepositorio.ObtenerMsjPorIdTipo(id);
                return listadoTipsSegunTipo
                    .Select(t=> this.ToDto(t)).ToList();
        }

        public async Task<List<TipDTO>> Listado()
        {
            var listado = await _tipsRepositorio.ObtenerTodos();
            return listado.Select(l => this.ToDto(l)).ToList();
        }

        public async Task Crear(TipDTO tip)
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
            
            await this._tipsRepositorio.Agregar(nuevoTip);
        }

        public async Task Actualizar(TipDTO tip, int id)
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
            
            await this._tipsRepositorio.Actualizar(tipDb);
        }

        public async Task<TipDTO?> ObtenerPorIdTipo(int id)
        {
            var tipoTip = await this._tipsRepositorio.ObtenerPorId(id);
            if (tipoTip == null) return null;
            return this.ToDto(tipoTip);
        }

        public TipDTO ToDto(Tip entidad)
        {
            TipDTO tipDTO = new TipDTO();
            tipDTO.Id = entidad.Id;
            tipDTO.Mensaje = entidad.Mensaje;
            tipDTO.TipoId = entidad.TipoId;
            //tipDTO.TipoTipDescripcion = entidad.TipoTip.Descripcion;
            tipDTO.ElementoAdicional = entidad.ElementoAdicional;
            tipDTO.Expresion =  entidad.Expresion;
            tipDTO.EfectoFiltro = entidad.EfectoFiltro; 
            return tipDTO;
        }
    }
}
