using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Enum;
using CivitaBack.Domain.Excepciones;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Logica.Interfaces;
using CivitaBack.Utils;

namespace CivitaBack.Logica;

public class MisionLogica : IMisionLogica
{
private readonly IMisionRepositorio _misionRepositorio;
    private readonly IMisionPartidaRepositorio _misionPartidaRepositorio;
    private readonly ICondicionRepositorio _condicionRepositorio;
    private readonly IUnidadDeTrabajo _unidadDeTrabajo;
    private readonly IAccesoUsuarios _accesoUsuarios;
    
    public MisionLogica(
        IMisionRepositorio imr, 
        ICondicionRepositorio condicionRepositorio, 
        IUnidadDeTrabajo unidadDeTrabajo,
        IMisionPartidaRepositorio impr,
        IAccesoUsuarios accesoUsuarios)
    {
        this._misionRepositorio = imr;
        this._condicionRepositorio = condicionRepositorio;
        this._unidadDeTrabajo = unidadDeTrabajo;
        this._misionPartidaRepositorio = impr;
        this._accesoUsuarios = accesoUsuarios;
    }

    /// <inheritdoc />
    public async Task ResetMisiones(TipoMision tipoMision)
    {
        List<MisionPartida> misionesPartida = await this._misionPartidaRepositorio.Listado();
        if (misionesPartida.Count == 0) return;
        foreach (MisionPartida misionPartida in misionesPartida)
        {
            misionPartida.Reclamado = false;
            misionPartida.FechaCompletado = null;
            misionPartida.FechaEntrega = DateTime.UtcNow;
        }
        
        await this._misionPartidaRepositorio.ActualizarVarios(misionesPartida);
        await this._unidadDeTrabajo.CommitAsync();

    }

    /// <inheritdoc />
    public async Task<List<Mision>> Listado()
    {
        return await this._misionRepositorio.Listado();
    }

    /// <inheritdoc />
    public async Task Crear(Mision mision)
    {
        this.ValidarMision(mision);
        // ValidarAdmin(); // Esto es un ejemplo si se necesita

        var condicion = await this._condicionRepositorio.ObtenerPorId(mision.CondicionId);
        this.ValidarCondicion(condicion);

        Mision nueva = new Mision
        {
            CondicionId = condicion.Id,
            Descripcion = mision.Descripcion,
            Disponible = mision.Disponible,
            Titulo = mision.Titulo,
        };

        await this._misionRepositorio.Agregar(nueva);
        await this._unidadDeTrabajo.CommitAsync();
    }

    /// <inheritdoc />
    public async Task Actualizar(Mision mision, int idMision)
    {
        this.ValidarMision(mision);
        // ValidarAdmin(); // Esto es un ejemplo si se necesita

        var condicion = await this._condicionRepositorio.ObtenerPorId(mision.CondicionId);
        this.ValidarCondicion(condicion);
        var misionDb = await this.ObtenerPorId(idMision);
        this.ValidarMision(misionDb);

        misionDb.Descripcion = mision.Descripcion;
        misionDb.Disponible = mision.Disponible;
        misionDb.Titulo = mision.Titulo;
        misionDb.Tipo = mision.Tipo;
        misionDb.CondicionId = condicion.Id; // Actualizamos el ID de la condición
        
        await this._misionRepositorio.Actualizar(misionDb);
        await this._unidadDeTrabajo.CommitAsync();
    }

    /// <inheritdoc />
    public async Task<Mision> ObtenerPorId(int idMision)
    {
        var mision = await this._misionRepositorio.ObtenerPorId(idMision);
        if (mision == null)
            throw new MisionExcepcion("No se encontró la mision");
        return mision;
    }

    
    // El método está mal nombrado, pero documenta lo que hace la implementación.
    // Aunque el nombre sugiere 'activas', la implementación devuelve todas las otorgadas.
    // Si la implementación no se toca, se documenta lo que hace.
    /// <summary>
    /// Retorna un listado de todas las misiones que han sido otorgadas a una partida específica.
    /// </summary>
    /// <param name="partida">La entidad Partida para buscar.</param>
    /// <returns>Una lista de entidades Mision asociadas a la partida.</returns>
    public async Task<List<Mision>> ObtenerMisionesActivasParaPartida(Partida partida)
    {
        this.ValidarPartida(partida);
        List<MisionPartida> mps =  await this._misionPartidaRepositorio.ObtenerMisionesPartida(partida.Id);
        return mps.Select(mp => mp.Mision).ToList();
    }
    
    /// <inheritdoc />
    public async Task<List<Mision>> ObtenerMisionesDisponibles()
    {
        return await this._misionRepositorio.ListadoActivo();
    }
    
    // --- MÉTODOS PRIVADOS ---

    private void ValidarPartida(Partida? partida)
    {
        if (partida == null) throw new MisionExcepcion("No se encontró la partida");
    }

    private void ValidarMision(Mision? mision)
    {
        if (mision == null)
            throw new MisionExcepcion("Ocurrió un error al guardar la misión");
        
        if (string.IsNullOrEmpty(mision.Descripcion))
            throw new MisionExcepcion("La descripción no puede estar vacia");
        
        if (string.IsNullOrEmpty(mision.Titulo))
            throw new MisionExcepcion("El título no puede estar vacio");
    }
    private void ValidarCondicion(Condicion? condicion)
    {
        if (condicion == null)
            throw new MisionExcepcion("Ocurrió un error al guardar la misión");
    }
    
}