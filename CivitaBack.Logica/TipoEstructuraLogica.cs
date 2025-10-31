using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Utils;

namespace CivitaBack.Logica;

public class TipoEstructuraLogica : ITipoEstructuraLogica
{
    private readonly ITipoEstructuraRepositorio _repositorioTipoEstructura;
    private readonly IUnidadDeTrabajo _unidadDeTrabajo;
    
    public TipoEstructuraLogica(ITipoEstructuraRepositorio rte, IUnidadDeTrabajo unidadDeTrabajo)
    {
        _repositorioTipoEstructura = rte;
        _unidadDeTrabajo = unidadDeTrabajo;
    }

    private void Validar(TipoEstructura tipoEstructura, int idTipoEstructura)
    {
        if (tipoEstructura == null || idTipoEstructura <= 0) 
            throw new Exception("Ocurrió un error al actualizar el Tipo de Estructura");
    }
    
    /// <summary>
    /// Actualiza un Tipo de Estructura
    /// </summary>
    /// <param name="tipoEstructura">TipoEstructura</param>
    /// <param name="idTipoEstructura">Id de Tipo Estructura</param>
    public async Task Actualizar(TipoEstructura tipoEstructura, int idTipoEstructura)
    {
        this.Validar(tipoEstructura, idTipoEstructura);
        var tipoEstructuraBuscada = await this._repositorioTipoEstructura.ObtenerPorId(idTipoEstructura);
        if (tipoEstructuraBuscada == null)
            throw new Exception("Tipo de Estructura no encontrada");
        
        tipoEstructuraBuscada.Nombre = tipoEstructura.Nombre;
        tipoEstructuraBuscada.Capacidad = tipoEstructura.Capacidad;
        tipoEstructuraBuscada.Ocupacion = tipoEstructura.Ocupacion;
        tipoEstructuraBuscada.EnergiaPorCiclo = tipoEstructura.EnergiaPorCiclo;
        tipoEstructuraBuscada.DineroPorCiclo = tipoEstructura.DineroPorCiclo;
        
        await this._repositorioTipoEstructura.Actualizar(tipoEstructuraBuscada);
        await this._unidadDeTrabajo.CommitAsync();
    }
    
    /// <summary>
    /// Elimina un Tipo de Estructura
    /// </summary>
    /// <param name="id">Id de Tipo Estructura</param>
    public async Task Eliminar(int id)
    {
        if (id <= 0) 
            throw new Exception("No se pudo borrar el Tipo de Estructura");
        await this._repositorioTipoEstructura.Eliminar(id);
        await this._unidadDeTrabajo.CommitAsync();
    }
    
    /// <summary>
    /// Guarda un Tipo de Estructura
    /// </summary>
    /// <param name="TipoEstructura">TipoEstructura</param>
    public async Task<TipoEstructura> Crear(TipoEstructura TipoEstructura)
    {
        this.Validar(TipoEstructura, 1);
        
        var tipoEstructura = new TipoEstructura
        {
            Nombre = TipoEstructura.Nombre,
            Capacidad = TipoEstructura.Capacidad,
            Ocupacion = TipoEstructura.Ocupacion,
            DineroPorCiclo = TipoEstructura.DineroPorCiclo,
            EnergiaPorCiclo = TipoEstructura.EnergiaPorCiclo,
        }; 
        
        await this._repositorioTipoEstructura.Agregar(tipoEstructura);
        await this._unidadDeTrabajo.CommitAsync();
        return tipoEstructura;
    }
    
    /// <summary>
    /// Obtiene un Tipo de Estructura por Id
    /// </summary>
    /// <param name="id">Id de Tipo Estructura</param>
    public async Task<TipoEstructura> ObtenerPorId(int id)
    {
        TipoEstructura? tipoEstructura = await this._repositorioTipoEstructura.ObtenerPorId(id);
        if (tipoEstructura == null) 
            throw new Exception("No se pudo encontrar el Tipo de Logro");
        return tipoEstructura;
    }
    
    /// <summary>
    /// Obtiene listado de Tipos de Estructuras
    /// </summary>
    public async Task<List<TipoEstructura>> Listado()
    {
        return await this._repositorioTipoEstructura.ObtenerTodos();
    }
    
}