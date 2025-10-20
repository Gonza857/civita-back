using CivitaBack.Data.BO;
using CivitaBack.Data.DTO;
using CivitaBack.Data.Repositorio;
using CivitaBack.Logica.Excepciones;
using CivitaBack.Tests;
using CivitaBack.Utils;

namespace CivitaBack.Logica;

public interface ILogroLogica
{
    Task<LogroDTO> ObtenerPorId(int Id);
    Task Crear(LogroDTO entidad);
    Task<List<LogroDTO>> ObtenerListado();
    Task<List<Logro>> ObtenerListadoInterno();
    Task Eliminar(int Id);
    Task Actualizar(LogroDTO logroDTO, int id);
    Task<List<LogroDTO>> ObtenerLogrosCumplidos(Partida partida);
    List<Logro> ComprobarSiCumpleAlgunLogro(Partida? partida, List<Logro> logros);
}

public class LogroLogica : IParser<Logro, LogroDTO>, ILogroLogica
{
    private readonly ILogroRepositorio repositorioLogro;
    private readonly ITipoLogroRepositorio repositorioTipoLogro;
    private readonly ICondicionRepositorio _condicionRepositorio;

    private readonly List<string> recursos = new List<string> { "Energia", "Contaminacion", "EcoCoins", "Felicidad" };


    public LogroLogica(ILogroRepositorio rtl, ITipoLogroRepositorio itlr, ICondicionRepositorio icr)
    {
        repositorioLogro = rtl;
        repositorioTipoLogro = itlr;
        _condicionRepositorio = icr;
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

    public async Task<List<Logro>> ObtenerListadoInterno()
    {
        return await this.repositorioLogro.ObtenerTodos();
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
    public async Task Crear(LogroDTO entidad)
    {
        this.ValidarLogro(entidad);
        TipoLogro? tipoLogro = await this.repositorioTipoLogro.ObtenerPorId(entidad.TipoId);
        if (tipoLogro == null) 
            throw new LogroExcepcion("No se proporcionó Tipo de Logro.");

        Condicion? condicion = await this._condicionRepositorio.ObtenerPorId(entidad.CondicionId);
        if (condicion == null)
            throw new LogroExcepcion("No se proporcionó condición.");

        var logro = new Logro
        {
            Titulo = entidad.Titulo,
            Descripcion = entidad.Descripcion,
            TipoLogro = tipoLogro,
            Condicion = condicion
            
        };
        
        await this.repositorioLogro.Guardar(logro);
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

    public async Task<List<LogroDTO>> ObtenerLogrosCumplidos(Partida partida)
    {
        List<LogroPartida> logrosPartida = partida.LogroPartidas;
        if (logrosPartida.Count != 0) return logrosPartida.Select(lp => this.ToDto(lp.Logro)).ToList();

        List<Logro> logros = logrosPartida.Select(lp => lp.Logro).ToList();
        List<Logro> resultado = new List<Logro>();
        foreach (Logro logro in logros)
        {
            Condicion condicionParaCumplirLogro = logro.Condicion;
            var propiedad = typeof(Recurso).GetProperty(condicionParaCumplirLogro.NombreColumna);
            if (propiedad != null)
            {
                int valorRecursoActual = (int)propiedad.GetValue(partida.Recursos);
                if (valorRecursoActual >= condicionParaCumplirLogro.Cantidad)
                {
                    resultado.Add(logro);
                }
                
            }
        }
        return resultado.Select(lp => this.ToDto(lp)).ToList();
    }

    public List<Logro> ComprobarSiCumpleAlgunLogro(Partida? partida, List<Logro> logrosDB)
    {
        if (partida == null || logrosDB.Count == 0 || partida.Recursos == null)
            throw new LogroExcepcion("No se pudo obtener si cumple algún logro.");
        /*
            por cada recurso
            entro a logro 
            entro a condicion
            miro si la columna del recurso coincide con la condicion columna
            miro si el valor del recurso coincide con la condicion cantidad
            si cumple retorno ese logro
            si no cumple, no lo retorno
            por cada item de esa lista, valido con logro partida para ver si ya lo cumplió
         */
        List<Logro> logrosParaPasarACumplido = new List<Logro>();
        
        foreach (string recurso in this.recursos)
        {
            foreach (Logro logro in logrosDB)
            {
                Condicion condicion = logro.Condicion;
                if (condicion.NombreColumna.Equals(recurso))
                {
                    // Coincide la columna de condicion con la de recurso (Energia - Energia)
                    Recurso recursoPartida = partida.Recursos;
                    var columnaNombre = typeof(Recurso).GetProperty(condicion.NombreColumna);
                    int valorColumna = (int)columnaNombre.GetValue(recursoPartida);
                    if (valorColumna >= condicion.Cantidad)
                    {
                        // Cumple!
                        logrosParaPasarACumplido.Add(logro);
                    }

                }
            }
        }

        return logrosParaPasarACumplido;
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
            TipoId = entidad.TipoLogro.Id,
            Tipo = entidad.TipoLogro.Nombre,
            CondicionId = entidad.Condicion.Id,
            Condicion = new CondicionDTO
            {
                Id = entidad.Condicion.Id,
                Cantidad = entidad.Condicion.Cantidad,
                NombreColumna = entidad.Condicion.NombreColumna,
                EstructuraId = entidad.Condicion.Estructura?.Id,
                Estructura = entidad.Condicion.Estructura != null
                    ? new EstructuraCondicionDTO
                    {
                        Id = entidad.Condicion.Estructura.Id,
                        Nombre = entidad.Condicion.Estructura.Nombre
                    }
                    : null
            }
        };
    }
}
