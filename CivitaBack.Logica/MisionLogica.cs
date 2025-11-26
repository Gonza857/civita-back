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

    public async Task<List<Mision>> Listado()
    {
        return await this._misionRepositorio.Listado();
    }

    public async Task Crear(Mision mision)
    {
        this.ValidarMision(mision);
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

    public async Task Actualizar(Mision mision, int idMision)
    {
        this.ValidarMision(mision);
        var condicion = await this._condicionRepositorio.ObtenerPorId(mision.CondicionId);
        this.ValidarCondicion(condicion);
        var misionDb = await this.ObtenerPorId(idMision);
        this.ValidarMision(misionDb);

        misionDb.Descripcion = mision.Descripcion;
        misionDb.Disponible = mision.Disponible;
        misionDb.Titulo = mision.Titulo;
        misionDb.Tipo = mision.Tipo;
        mision.CondicionId = condicion!.Id;
        
        await this._misionRepositorio.Actualizar(misionDb);
        await this._unidadDeTrabajo.CommitAsync();
    }

    public async Task<Mision> ObtenerPorId(int idMision)
    {
        var mision = await this._misionRepositorio.ObtenerPorId(idMision);
        if (mision == null)
            throw new MisionExcepcion("No se encontró la mision");
        return mision;
    }

    
    // retorna listado de misiones (disponibles) otorgadas a una partida (diarias, semanales y mensuales)
    public async Task<List<Mision>> ObtenerMisionesActivasParaPartida(Partida partida)
    {
        this.ValidarPartida(partida);
        List<MisionPartida> mps =  await this._misionPartidaRepositorio.ObtenerMisionesPartida(partida.Id);
        return mps.Select(mp => mp.Mision).ToList();
    }
    
    public async Task<List<Mision>> ObtenerMisionesDisponibles()
    {
        return await this._misionRepositorio.ListadoActivo();
    }

    public async Task AsignarMisiones(Partida? partida)
    {
        // this.ValidarPartida(partida);
        // var misiones = await this._misionRepositorio.ListadoActivo();
        // await this._misionPartidaRepositorio.AgregarMisionesPartida(misiones, partida!);
        // await this._unidadDeTrabajo.CommitAsync();
    }
    

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

    private void ValidarAdmin()
    {
        if (!_accesoUsuarios.EsDios())
            throw new AccesoDenegadoExcepcion("Se requieren privilegios de administrador para modificar las misiones.");
    }
}