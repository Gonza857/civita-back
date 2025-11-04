using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Excepciones;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Utils;

namespace CivitaBack.Logica;

public class MisionPartidaLogica : IMisionPartidaLogica
{
    private readonly IMisionPartidaRepositorio _misionPartidaRepositorio;
    private readonly IUnidadDeTrabajo _unidadDeTrabajo;

    public MisionPartidaLogica(IMisionPartidaRepositorio impr, IUnidadDeTrabajo unidadDeTrabajo)
    {
        _misionPartidaRepositorio = impr;
        _unidadDeTrabajo = unidadDeTrabajo;
    }
    
    public async Task<Mision> ObtenerMisionPartidaPorId(int idPartida, int idMision)
    {
        var mision = await this._misionPartidaRepositorio.ObtenerMisionPartidaPorId(idPartida, idMision);
        if (mision == null)
            throw new MisionPartidaExcepcion("No se encontró la misión");
        return mision;
    }
    
    public async Task AsignarMisiones(List<Mision> misionesActivas, Partida partida)
    {
        await this._misionPartidaRepositorio.AgregarMisionesPartida(misionesActivas, partida!);
        await this._unidadDeTrabajo.CommitAsync();
    }

    public async Task MarcarMisionCompletada(Mision mision, Partida partida)
    {
        MisionPartida mp = await this._misionPartidaRepositorio.ObtenerUnaMisionDePartida(partida.Id, mision.Id);
        mp.FechaCompletado = DateTime.UtcNow;
        mp.Reclamado = true;
        await this._misionPartidaRepositorio.Actualizar(mp);
        await this._unidadDeTrabajo.CommitAsync();
    }

    public async Task<List<Mision>> ObtenerMisionesDia(int idUsuario)
    {
        return await this._misionPartidaRepositorio.ObtenerMisionesDia(idUsuario);
    }

    public async Task<List<Mision>> ObtenerMisionesSemana(int idUsuario)
    {
        return await this._misionPartidaRepositorio.ObtenerMisionesSemana(idUsuario);
    }

    public async Task<List<Mision>> ObtenerMisionesMes(int idUsuario)
    {
        return await this._misionPartidaRepositorio.ObtenerMisionesMes(idUsuario);
    }
}