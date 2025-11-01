using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Utils;

namespace CivitaBack.Logica;

public class EstructuraMapaLogica : IEstructuraMapaLogica
{
    private readonly IEstructuraMapaRepositorio _estructuraMapaRepositorio;
    private readonly IUnidadDeTrabajo _uow;

    public EstructuraMapaLogica(IEstructuraMapaRepositorio emr, IUnidadDeTrabajo uow)
    {
        _estructuraMapaRepositorio = emr;
        _uow = uow;
    }
    
    public async Task ReiniciarEstructurasDePartida(int idPartida)
    {
        await this._estructuraMapaRepositorio.EliminarPorPartidaIdAsync(idPartida);

        await _uow.CommitAsync();
    }

    public async Task EliminarEstructuraAsync(EstructuraMapa em)
    {
        var entidad = await _estructuraMapaRepositorio.ObtenerCoincidenteAsync(em.PartidaId, em.EstructuraId, em.X, em.Y, em.Width,
            em.Height);

        if (entidad is null)
            throw new InvalidOperationException("No se encontró la estructura a eliminar");

        await _estructuraMapaRepositorio.EliminarAsync(entidad);
        
        await this._uow.CommitAsync();
    }

}
