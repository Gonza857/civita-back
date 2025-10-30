using CivitaBack.Data.BO;
using CivitaBack.Data.DTO;
using CivitaBack.Data.Repositorio;
using CivitaBack.Utils;

namespace CivitaBack.Logica;

public interface ITipoEstructuraLogica
{
    Task<TipoEstructuraDTO> ObtenerPorId(int id);
    Task<TipoEstructuraDTO> Crear(TipoEstructuraDTO nuevoTipologro);

    Task Actualizar(TipoEstructuraDTO tipoLogro, int idTipoLogro);

    Task<List<TipoEstructuraDTO>> Listado();

    Task Eliminar(int id);
}
public class TipoEstructuraLogica : ITipoEstructuraLogica, IParser<TipoEstructura, TipoEstructuraDTO>
{
    private readonly ITipoEstructuraRepositorio _repositorioTipoEstructura;
    
    public TipoEstructuraLogica(ITipoEstructuraRepositorio rte)
    {
        _repositorioTipoEstructura = rte;
    }

    private void Validar(TipoEstructuraDTO tipoEstructuraDTO, int idTipoEstructura)
    {
        if (tipoEstructuraDTO == null || idTipoEstructura <= 0) 
            throw new Exception("Ocurrió un error al actualizar el Tipo de Estructura");
    }
    
    /// <summary>
    /// Actualiza un Tipo de Estructura
    /// </summary>
    /// <param name="tipoEstructura">TipoEstructuraDTO</param>
    /// <param name="idTipoEstructura">Id de Tipo Estructura</param>
    public async Task Actualizar(TipoEstructuraDTO tipoEstructura, int idTipoEstructura)
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
    }
    
    /// <summary>
    /// Guarda un Tipo de Estructura
    /// </summary>
    /// <param name="tipoEstructuraDTO">TipoEstructuraDTO</param>
    public async Task<TipoEstructuraDTO> Crear(TipoEstructuraDTO tipoEstructuraDTO)
    {
        this.Validar(tipoEstructuraDTO, 1);
        
        var tipoEstructura = new TipoEstructura
        {
            Nombre = tipoEstructuraDTO.Nombre,
            Capacidad = tipoEstructuraDTO.Capacidad,
            Ocupacion = tipoEstructuraDTO.Ocupacion,
            DineroPorCiclo = tipoEstructuraDTO.DineroPorCiclo,
            EnergiaPorCiclo = tipoEstructuraDTO.EnergiaPorCiclo,
        }; 
        
        await this._repositorioTipoEstructura.Agregar(tipoEstructura);
        return this.ToDto(tipoEstructura);
    }
    
    /// <summary>
    /// Obtiene un Tipo de Estructura por Id
    /// </summary>
    /// <param name="id">Id de Tipo Estructura</param>
    public async Task<TipoEstructuraDTO> ObtenerPorId(int id)
    {
        TipoEstructura? tipoEstructura = await this._repositorioTipoEstructura.ObtenerPorId(id);
        if (tipoEstructura == null) 
            throw new Exception("No se pudo encontrar el Tipo de Logro");
        return this.ToDto(tipoEstructura);
    }
    
    /// <summary>
    /// Obtiene listado de Tipos de Estructuras
    /// </summary>
    public async Task<List<TipoEstructuraDTO>> Listado()
    {
        var tiposDeEstructuras = await this._repositorioTipoEstructura.ObtenerTodos();
        return tiposDeEstructuras
            .Select(p => this.ToDto(p))
            .ToList();
    }
    
    
    /// <summary>
    /// Convierte entidad de dominio a DTO
    /// </summary>
    /// <param name="entidad">Tipo Estructura</param>
    public TipoEstructuraDTO ToDto(TipoEstructura entidad)
    {
        return new TipoEstructuraDTO
        {
            Id = entidad.Id,
            Nombre = entidad.Nombre,
            Capacidad = entidad.Capacidad,
            Ocupacion = entidad.Ocupacion,
            DineroPorCiclo = entidad.DineroPorCiclo,
            EnergiaPorCiclo = entidad.EnergiaPorCiclo
        };
    }
}