using CivitaBack.Data.BO;
using CivitaBack.Data.DTO;
using CivitaBack.Data.Repositorio;
using CivitaBack.Utils;

namespace CivitaBack.Logica;

public interface ITipoTipLogica
{
    Task<List<TipoTipDTO>> Listado();
    Task Eliminar(int id);
    Task Guardar(TipoTipDTO tipoTipDto);
    Task<TipoTipDTO> ObtenerPorId(int id);
    Task Actualizar(TipoTipDTO tipoTipDto, int id);
}

public class TipoTipLogica : ITipoTipLogica, IParser<TipoTip, TipoTipDTO>
{
    
    private readonly ITipoTipRepositorio _repositorioTipoTip;

    public TipoTipLogica(ITipoTipRepositorio rtt)
    {
        _repositorioTipoTip = rtt;
    }


    public async Task<List<TipoTipDTO>> Listado()
    {
        var tiposTip = await this._repositorioTipoTip.ObtenerTodos();
        return tiposTip
            .Select(tt => this.ToDto(tt))
            .ToList();
    }

    public async Task Eliminar(int id)
    {
        var tipoTipDb = await this._repositorioTipoTip.ObtenerPorId(id);
        if (tipoTipDb == null)
            throw new Exception($"Tipo de tipo no encontrado: {id}");
        await this._repositorioTipoTip.Eliminar(id);
    }

    public async Task Guardar(TipoTipDTO tipoTipDto)
    {
        TipoTip nuevo = new TipoTip
        {
            Descripcion = tipoTipDto.Descripcion,
        };
        await this._repositorioTipoTip.Guardar(nuevo);
    }

    public async Task<TipoTipDTO> ObtenerPorId(int id)
    {
        TipoTip? buscado = await this._repositorioTipoTip.ObtenerPorId(id);
        if (buscado == null) 
            throw new Exception($"Tipo de tipo no encontrado: {id}");
        return this.ToDto(buscado);
    }

    public async Task Actualizar(TipoTipDTO tipoTipDto, int id)
    {
        TipoTip? buscado = await this._repositorioTipoTip.ObtenerPorId(id);
        if (buscado == null) 
            throw new Exception($"Tipo de tipo no encontrado: {id}");
        
        buscado.Descripcion = tipoTipDto.Descripcion;
        
        await this._repositorioTipoTip.Actualizar(buscado);
    }

    public TipoTipDTO ToDto(TipoTip entidad)
    {
        return new TipoTipDTO
        {
            Id = entidad.Id,
            Descripcion = entidad.Descripcion,
        };
    }
}