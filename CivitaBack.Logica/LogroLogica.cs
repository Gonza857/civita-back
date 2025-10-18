using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CivitaBack.Data.BO;
using CivitaBack.Data.DTO;
using CivitaBack.Data.Repositorio;
using CivitaBack.Logica.Excepciones;
using CivitaBack.Utils;

namespace CivitaBack.Logica;

public interface ILogroLogica
{
    Task<LogroDTO> ObtenerPorId(int Id);
    Task<LogroDTO> Guardar(LogroDTO entidad);
    Task<List<LogroDTO>> ObtenerListado();
    Task Eliminar(int Id);

    Task Actualizar(LogroDTO logroDTO, int id);
}

public class LogroLogica : IParser<Logro, LogroDTO>, ILogroLogica
{
    private readonly ILogroRepositorio repositorioLogro;
    private readonly ITipoLogroRepositorio repositorioTipoLogro;

    public LogroLogica(ILogroRepositorio rtl, ITipoLogroRepositorio itlr)
    {
        repositorioLogro = rtl;
        repositorioTipoLogro = itlr;
    }

    /// <summary>
    /// Valida los datos entrantes del DTO
    /// </summary>
    /// <param name="logroDTO">LogroDTO</param>
    private void ValidarLogro(LogroDTO logroDTO)
    {
        if (logroDTO == null) 
            throw new LogroExcepcion("Ocurrió un error al actualizar el Logro");
    }

    /// <summary>
    /// Actualiza un logro
    /// </summary>
    /// <param name="logroDTO">LogroDTO</param>
    /// /// <param name="id">Id Logro</param>
    public async Task Actualizar(LogroDTO logroDTO, int id)
    {
        this.ValidarLogro(logroDTO);
        Logro logroBuscado = await this.repositorioLogro.ObtenerPorId(id);
        var tipoLogroBuscado = await this.repositorioTipoLogro.ObtenerPorId(logroDTO.TipoId);
        
        if (logroBuscado == null || tipoLogroBuscado == null) 
            throw new LogroExcepcion("Ocurrió un error al actualizar el Logro");

        logroBuscado.Titulo = logroDTO.Titulo;
        logroBuscado.Descripcion = logroDTO.Descripcion;
        logroBuscado.Titulo = logroDTO.Titulo;
        logroBuscado.TipoLogro = tipoLogroBuscado;

        await this.repositorioLogro.Actualizar(logroBuscado);
    }

    /// <summary>
    /// Elimina un logro
    /// </summary>
    /// <param name="id">Id Logro</param>
    public async Task Eliminar(int id)
    {
        if (id <= 0) 
            throw new LogroExcepcion("No se pudo borrar el Logro");
        await this.repositorioLogro.Eliminar(id);
    }

    /// <summary>
    /// Guarda un logro
    /// </summary>
    /// <param name="logroDTO">LogroDTO</param>
    public async Task<LogroDTO> Guardar(LogroDTO entidad)
    {
        this.ValidarLogro(entidad);
        TipoLogro? tipoLogro = await this.repositorioTipoLogro.ObtenerPorId(entidad.TipoId);
        if (tipoLogro == null) 
            throw new LogroExcepcion("No se pudo guardar el Logro");

        var logro = new Logro
        {
            Titulo = entidad.Titulo,
            Descripcion = entidad.Descripcion,
            TipoLogro = tipoLogro
            
        };
        
        await this.repositorioLogro.Guardar(logro);
        return this.ToDto(logro);
    }

    /// <summary>
    /// Obtener listado
    /// </summary>
    public async Task<List<LogroDTO>> ObtenerListado()
    {
        var logros = await this.repositorioLogro.ObtenerTodos();
        return logros
          .Select(p => this.ToDto(p))
          .ToList();
    }

    /// <summary>
    /// Obtener logro por Id
    /// </summary>
    /// <param name="id">Id de Logro</param>
    public async Task<LogroDTO> ObtenerPorId(int id)
    {
        var logro = await this.repositorioLogro.ObtenerPorId(id);
        if (logro == null) 
            throw new LogroExcepcion("No se pudo obtener el logro");
        return this.ToDto(logro);
    }

    /// <summary>
    /// Convierte entidad de dominio a DTO
    /// </summary>
    /// <param name="entidad">Logro</param>
    public LogroDTO ToDto(Logro entidad)
    {
        return new LogroDTO
        {
            Id = entidad.Id,
            Titulo = entidad.Titulo,
            Descripcion = entidad.Descripcion,
            TipoId =  entidad.TipoLogro.Id,
            Tipo = entidad.TipoLogro.Nombre
        };
    }
}
