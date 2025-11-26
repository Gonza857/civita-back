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
    
    /// <inheritdoc />
    public async Task<Mision> ObtenerMisionPartidaPorId(int idPartida, int idMision)
    {
        var mision = await this._misionPartidaRepositorio.ObtenerMisionPartidaPorId(idPartida, idMision);
        if (mision == null)
            throw new MisionPartidaExcepcion("No se encontró la misión");
        return mision;
    }
    
    /// <inheritdoc />
    public async Task AsignarMisiones(List<Mision> misionesActivas, Partida partida)
    {
        List<MisionPartida> misionesYaAsignadas = await _misionPartidaRepositorio.ObtenerMisionesAsignadas(partida.Id);
        
        var idsAsignados = misionesYaAsignadas
            .Select(mp => mp.MisionId) // Obtenemos solo el ID de la Misión
            .ToHashSet();
        
        List<Mision> misionesNuevas = misionesActivas
            .Where(m => !idsAsignados.Contains(m.Id))
            .ToList();
        
        if (misionesNuevas.Count == 0)
        {
            return;
        }
        
        partida.MisionPartidas = this._misionPartidaRepositorio.AgregarMisionesPartida(misionesNuevas, partida);
        this._misionPartidaRepositorio.GuardarMisionesPartida(partida);
        
        await this._unidadDeTrabajo.CommitAsync();
    }

    /// <inheritdoc />
    public List<MisionPartida> ProcesarMisionesPartida(List<MisionPartida> reclamables, List<MisionPartida> noReclamables)
    {
        
        var idsNoReclamables = noReclamables
            .Select(mp => mp.MisionId) // Obtenemos solo el ID de la Misión
            .ToHashSet();
        
        List<MisionPartida> misionesNoReclamables = reclamables
            .Where(m => !idsNoReclamables.Contains(m.MisionId))
            .ToList();

        return misionesNoReclamables;

    }
    
    /// <summary>
    /// Marca una MisionPartida específica como completada y reclamada.
    /// </summary>
    /// <inheritdoc />
    public async Task MarcarMisionCompletada(Mision mision, Partida partida)
    {
        MisionPartida mp = await this._misionPartidaRepositorio.ObtenerUnaMisionDePartida(partida.Id, mision.Id);
        mp.FechaCompletado = DateTime.UtcNow;
        mp.Reclamado = true;
        await this._misionPartidaRepositorio.MarcarCompletada(mp);
        // await this._unidadDeTrabajo.CommitAsync(); // Se asume que el commit lo hace la lógica superior
    }

    /// <inheritdoc />
    public async Task<List<Mision>> ObtenerMisionesDia(int idUsuario)
    {
        return await this._misionPartidaRepositorio.ObtenerMisionesDia(idUsuario);
    }

    /// <inheritdoc />
    public async Task<List<Mision>> ObtenerMisionesSemana(int idUsuario)
    {
        return await this._misionPartidaRepositorio.ObtenerMisionesSemana(idUsuario);
    }

    /// <inheritdoc />
    public async Task<List<Mision>> ObtenerMisionesMes(int idUsuario)
    {
        return await this._misionPartidaRepositorio.ObtenerMisionesMes(idUsuario);
    }
    
    /// <inheritdoc />
    public async Task<List<MisionPartida>> ObtenerMisionesActivasParaPartida(Partida partida)
    {
        this.ValidarPartida(partida);
        return await this._misionPartidaRepositorio.ObtenerMisionesPartida(partida.Id);
    }
    
    private void ValidarPartida(Partida? partida)
    {
        if (partida == null) throw new MisionExcepcion("No se encontró la partida");
    }
}