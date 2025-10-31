using CivitaBack.Data.DTO;
using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Logica.Excepciones;

namespace CivitaBack.Logica;

public class TipoLogroLogica : ITipoLogroLogica
{
    private readonly ITipoLogroRepositorio repositorioTipoLogro;

    public TipoLogroLogica(ITipoLogroRepositorio rtl)
    {
        repositorioTipoLogro = rtl;
    }

    private void ValidarTipoLogro(TipoLogro TipoLogro, int idTipoLogro)
    {
        if (TipoLogro == null || idTipoLogro <= 0) 
            throw new TipoLogroException("Ocurrió un error al actualizar el Tipo de Logro");
    }

    /// <summary>
    /// Actualiza un Tipo de Logro
    /// </summary>
    /// <param name="tipoLogro">TipoLogro</param>
    /// <param name="idTipoLogro">Id de Tipo Logro</param>
    public async Task Actualizar(TipoLogro tipoLogro, int idTipoLogro)
    {
        this.ValidarTipoLogro(tipoLogro, idTipoLogro);
        var tipoLogroBuscado = await this.repositorioTipoLogro.ObtenerPorId(idTipoLogro);
        if (tipoLogroBuscado == null)
            throw new TipoLogroException("Tipo de Logro no encontrado");
        
        tipoLogroBuscado.Nombre = tipoLogro.Nombre;
        await this.repositorioTipoLogro.Actualizar(tipoLogroBuscado);
    }

    /// <summary>
    /// Elimina un Tipo de logro
    /// </summary>
    /// <param name="id">Id de Tipo Logro</param>
    public async Task Eliminar(int id)
    {
        if (id <= 0) 
            throw new TipoLogroException("No se pudo borrar el Tipo de Logro");
        await this.repositorioTipoLogro.Eliminar(id);
    }

    /// <summary>
    /// Guarda un Tipo de logro
    /// </summary>
    /// <param name="TipoLogro">TipoLogro</param>
    public async Task<TipoLogro> Guardar(TipoLogro TipoLogro)
    {
        this.ValidarTipoLogro(TipoLogro, TipoLogro.Id);
        
        var tipoLogro = new TipoLogro
        {
            Nombre = TipoLogro.Nombre
        }; 
        
        await this.repositorioTipoLogro.Agregar(tipoLogro);
        return this.TipoLogroToDTO(tipoLogro);
    }

    /// <summary>
    /// Obtiene un Tipo de Logro por Id
    /// </summary>
    /// <param name="id">Id de Tipo Logro</param>
    public async Task<TipoLogro> ObtenerPorId(int id)
    {
        var tipoLogro = await this.repositorioTipoLogro.ObtenerPorId(id);
        if (tipoLogro == null) 
            throw new TipoLogroException("No se pudo encontrar el Tipo de Logro");
        return this.TipoLogroToDTO(tipoLogro);
    }
    
    /// <summary>
    /// Obtiene listado de Tipos de Logro
    /// </summary>
    public async Task<List<TipoLogro>> ObtenerTiposLogro()
    {
        var tiposDeLogros = await this.repositorioTipoLogro.ObtenerTodos();
        return tiposDeLogros
          .Select(p => this.TipoLogroToDTO(p))
          .ToList();
    }

    /// <summary>
    /// Convierte entidad de dominio a DTO
    /// </summary>
    /// <param name="entidad">Tipo Logro</param>
    private TipoLogro TipoLogroToDTO(TipoLogro entidad)
    {
        return new TipoLogro
        {
            Id = entidad.Id,
            Nombre = entidad.Nombre,
        };
    }
}
