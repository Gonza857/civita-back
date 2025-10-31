using CivitaBack.Data.DTO;
using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Utils;

namespace CivitaBack.Logica;

public interface IEstructuraMapaLogica
{

    Task ReiniciarEstructurasDePartida(int idPartida);
    Task EliminarEstructuraAsync(EliminarEstructuraDTO dto);

}
public class EstructuraMapaLogica : IEstructuraMapaLogica
{
    private readonly IEstructuraMapaRepositorio _estructuraMapaRepositorio;
    private readonly IUnidadDeTrabajo _unidadDeTrabajo;

    public EstructuraMapaLogica(IEstructuraMapaRepositorio emr, IUnidadDeTrabajo unidadDeTrabajo)
    {
        _estructuraMapaRepositorio = emr;
        _unidadDeTrabajo = unidadDeTrabajo;
    }
    
    public async Task ReiniciarEstructurasDePartida(int idPartida)
    {
        await this._estructuraMapaRepositorio.EliminarPorPartidaIdAsync(idPartida);
    }

    public async Task EliminarEstructuraAsync(EliminarEstructuraDTO dto)
    {
        var entidad = await _estructuraMapaRepositorio.ObtenerCoincidenteAsync(dto.PartidaId, dto.EstructuraId, dto.X, dto.Y, dto.Width, 
            dto.Height);

        if (entidad is null)
            throw new InvalidOperationException("No se encontró la estructura a eliminar");

        await _estructuraMapaRepositorio.EliminarAsync(entidad);
        await this._unidadDeTrabajo.CommitAsync();
    }

}
