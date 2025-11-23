using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Excepciones;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Logica.Interfaces;
using CivitaBack.Utils;

namespace CivitaBack.Logica
{
    public class TipLogica : ITipLogica
    {
        private readonly ITipsRepositorio _tipsRepositorio;
        private readonly ITipoTipRepositorio _tiposTipRepositorio;
        private readonly IUnidadDeTrabajo _uow;
        private readonly IAccesoUsuarios _accesoUsuarios;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="TipLogica"/>.
        /// </summary>
        /// <param name="itr">El repositorio de Tips.</param>
        /// <param name="ittr">El repositorio de TipoTip.</param>
        /// <param name="iudt">La unidad de trabajo.</param>
        public TipLogica(ITipsRepositorio itr, ITipoTipRepositorio ittr, IUnidadDeTrabajo iudt, IAccesoUsuarios accesoUsuarios)
        {
            _tipsRepositorio = itr;
            _tiposTipRepositorio = ittr;
            _uow = iudt;
            _accesoUsuarios = accesoUsuarios;
        }

        /// <inheritdoc />
        public async Task<List<Tip>> ObtenerMsjPorIdTipo(int id)
        {
            return await _tipsRepositorio.ObtenerMsjPorIdTipo(id);
        }

        /// <inheritdoc />
        public async Task<List<Tip>> Listado()
        {
            return await _tipsRepositorio.ObtenerTodos();
        }

        /// <inheritdoc />
        public async Task Crear(Tip tip)
        {
            var tipoTip = await this._tiposTipRepositorio.ObtenerPorId(tip.TipoId);
            if (tipoTip == null)
                throw new Exception("No se pudo guardar el tip.");

            ValidarAdmin();

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
                await this._uow.CommitAsync();
            }
            catch (Exception ex)
            {
                throw new ErrorInternoException("Ocurrió un error al crear un Tip");
            }
        }

        /// <inheritdoc />
        public async Task Actualizar(Tip tip, int id)
        {
            var tipoTip = await this._tiposTipRepositorio.ObtenerPorId(tip.TipoId);
            if (tipoTip == null)
                throw new DominioException("No se pudo guardar el tip.");

            var tipDb = await this._tipsRepositorio.ObtenerPorId(id);
            if (tipDb == null)
                throw new DominioException("No se pudo guardar el tip.");

            // ValidarAdmin();

            tipDb.TipoTip = tipoTip;
            tipDb.Expresion = tip.Expresion;
            tipDb.ElementoAdicional = tip.ElementoAdicional;
            tipDb.Mensaje = tip.Mensaje;
            tipDb.EfectoFiltro = tip.EfectoFiltro;

            try
            {
                await this._tipsRepositorio.Actualizar(tipDb);
                await this._uow.CommitAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al Actualizar un Tip");
            }
        }

        /// <inheritdoc />
        public async Task<Tip?> ObtenerPorIdTipo(int id)
        {
            return await this._tipsRepositorio.ObtenerPorId(id);
        }

        public async Task Eliminar(int id)
        {
            Tip? tip = await this._tipsRepositorio.ObtenerPorId(id);
            if (tip == null) throw new DominioException("No se encontró el tip");
            await this._tipsRepositorio.Eliminar(id);
            await this._uow.CommitAsync();
        }

        private void ValidarAdmin()
        {
            if (!_accesoUsuarios.EsDios())
                throw new AccesoDenegadoExcepcion("Se requieren privilegios de administrador para modificar el catálogo de estructuras.");
        }
    }
}