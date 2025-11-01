using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Utils;

namespace CivitaBack.Logica;

public class TipoTipLogica : ITipoTipLogica
{
    
    private readonly ITipoTipRepositorio _repositorioTipoTip;
    private readonly IUnidadDeTrabajo _uow;

    public TipoTipLogica(ITipoTipRepositorio rtt, IUnidadDeTrabajo uow)
    {
        _repositorioTipoTip = rtt;
        _uow = uow;
    }


    public async Task<List<TipoTip>> Listado()
    {
        return await this._repositorioTipoTip.ObtenerTodos(); 
    }

    public async Task Eliminar(int id)
    {
        var tipoTipDb = await this._repositorioTipoTip.ObtenerPorId(id);
        if (tipoTipDb == null)
            throw new Exception($"Tipo de tipo no encontrado: {id}");
        await this._repositorioTipoTip.Eliminar(id);
        await _uow.CommitAsync();
    }

    public async Task Guardar(TipoTip tipoTipDto)
    {
        TipoTip nuevo = new TipoTip
        {
            Descripcion = tipoTipDto.Descripcion,
        };
        await this._repositorioTipoTip.Agregar(nuevo);
        await _uow.CommitAsync();
    }

    public async Task<TipoTip> ObtenerPorId(int id)
    {
        TipoTip? buscado = await this._repositorioTipoTip.ObtenerPorId(id);
        if (buscado == null) 
            throw new Exception($"Tipo de tipo no encontrado: {id}");
        return buscado;
    }

    public async Task Actualizar(TipoTip tipoTipDto, int id)
    {
        TipoTip? buscado = await this._repositorioTipoTip.ObtenerPorId(id);
        if (buscado == null) 
            throw new Exception($"Tipo de tipo no encontrado: {id}");
        
        buscado.Descripcion = tipoTipDto.Descripcion;
        
        await this._repositorioTipoTip.Actualizar(buscado);

        await _uow.CommitAsync();
    }

}