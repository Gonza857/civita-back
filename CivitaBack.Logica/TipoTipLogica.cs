using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Excepciones;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Logica.Interfaces;
using CivitaBack.Utils;

namespace CivitaBack.Logica;

public class TipoTipLogica : ITipoTipLogica
{
    
    private readonly ITipoTipRepositorio _repositorioTipoTip;
    private readonly IUnidadDeTrabajo _uow;
    private readonly IAccesoUsuarios _accesoUsuarios;

    /// <summary>
    /// Inicializa una new instancia de la clase <see cref="TipoTipLogica"/>.
    /// </summary>
    /// <param name="rtt">El repositorio de TipoTip.</param>
    /// <param name="uow">La unidad de trabajo.</param>
    public TipoTipLogica(ITipoTipRepositorio rtt, IUnidadDeTrabajo uow, IAccesoUsuarios accesoUsuarios)
    {
        _repositorioTipoTip = rtt;
        _uow = uow;
        _accesoUsuarios = accesoUsuarios;
    }

    /// <inheritdoc />
    public async Task<List<TipoTip>> Listado()
    {
        return await this._repositorioTipoTip.ObtenerTodos(); 
    }

    /// <inheritdoc />
    public async Task Eliminar(int id)
    {
        var tipoTipDb = await this._repositorioTipoTip.ObtenerPorId(id);
        if (tipoTipDb == null)
            throw new TipoTipException($"Tipo de tipo no encontrado: {id}");

        ValidarAdmin();
        
        await this._repositorioTipoTip.Eliminar(id);
        await _uow.CommitAsync();
    }

    /// <inheritdoc />
    public async Task Guardar(TipoTip tipoTipDto)
    {
        ValidarAdmin();

        TipoTip nuevo = new TipoTip
        {
            Descripcion = tipoTipDto.Descripcion,
        };
        await this._repositorioTipoTip.Agregar(nuevo);
        await _uow.CommitAsync();
    }

    /// <inheritdoc />
    public async Task<TipoTip?> ObtenerPorId(int id)
    {
        return  await this._repositorioTipoTip.ObtenerPorId(id);
    }

    /// <inheritdoc />
    public async Task Actualizar(TipoTip tipoTipDto, int id)
    {
        TipoTip? buscado = await this._repositorioTipoTip.ObtenerPorId(id);
        if (buscado == null) 
            throw new TipoTipException($"Tipo de tip no encontrado: {id}");

        ValidarAdmin();

        buscado.Descripcion = tipoTipDto.Descripcion;
        await this._repositorioTipoTip.Actualizar(buscado);
        await _uow.CommitAsync();
    }

    private void ValidarAdmin()
    {
        if (!_accesoUsuarios.EsDios())
            throw new AccesoDenegadoExcepcion("Se requieren privilegios de administrador para modificar el catálogo de estructuras.");
    }
}