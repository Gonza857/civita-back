using CivitaBack.Data.BO;
using CivitaBack.Data.DTO;
using CivitaBack.Data.Repositorio;
using CivitaBack.Logica.Excepciones;
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
    Task<List<LogroDTO>> ComprobarSiCumpleAlgunLogro(Partida? partida, List<Logro> logrosDB);
    Task MarcarLogrosComoCompletados(Partida? partida, List<LogroDTO> logrosdto);
    
    Task<List<Logro>> ObtenerLogrosParaObtenerRecompensa(int partidaId);
}

public class LogroLogica : IParser<Logro, LogroDTO>, ILogroLogica
{
    private readonly ILogroRepositorio repositorioLogro;
    private readonly ITipoLogroRepositorio repositorioTipoLogro;
    private readonly ICondicionRepositorio _condicionRepositorio;
    private readonly ILogroPartidaRepositorio _logroPartidaRepositorio;

    private readonly List<string> recursos = new List<string> { "Energia", "Contaminacion", "EcoCoins", "Felicidad" };


    public LogroLogica(
        ILogroRepositorio rtl, 
        ITipoLogroRepositorio itlr, 
        ICondicionRepositorio icr,
        ILogroPartidaRepositorio ilpr
        )
    {
        repositorioLogro = rtl;
        repositorioTipoLogro = itlr;
        _condicionRepositorio = icr;
        _logroPartidaRepositorio = ilpr;
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

    public async Task<List<LogroDTO>> ComprobarSiCumpleAlgunLogro(Partida? partida, List<Logro> logrosDB)
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

        foreach (Logro logro in logrosDB)
        {
            Condicion condicion = logro.Condicion;

            if (!string.IsNullOrEmpty(condicion.NombreColumna))
            {
                // Condición de recurso
                Recurso recursoPartida = partida.Recursos;
                var propiedad = typeof(Recurso).GetProperty(condicion.NombreColumna);
                if (propiedad != null && propiedad.PropertyType == typeof(int))
                {
                    int valor = (int)propiedad.GetValue(recursoPartida)!;
                    if (valor >= condicion.Cantidad)
                        logrosParaPasarACumplido.Add(logro);
                }
            }
            else if (condicion.EstructuraId != null)
            {
                // Cada em en EstructuraMapa = 1 estructura, entonces
                int cantidadTotal = partida.EstructuraMapa
                    .Count(em => em.EstructuraId == condicion.EstructuraId);

                if (cantidadTotal >= condicion.Cantidad)
                    logrosParaPasarACumplido.Add(logro);

            }
        }

        var idsLogros = logrosParaPasarACumplido.Select(l => l.Id).ToList();
        var logrosFiltrados = await this._logroPartidaRepositorio
            .ObtenerLogrosParaReclamarQueNoEstenCumplidos(idsLogros);
        return logrosFiltrados.Select(l => this.ToDto(l)).ToList();
    }
    
    public async Task MarcarLogrosComoCompletados(Partida? partida, List<LogroDTO> logrosDB)
    {
        if (partida == null || logrosDB.Count == 0 || partida.Recursos == null)
            throw new LogroExcepcion("No se pudo obtener si cumple algún logro.");
        /*
            por cada logro
            verifico que no exista en la bd
            si no esta lo pongo,
            si esta, no
            entro a logro

         */

        foreach (LogroDTO logro in logrosDB)
        {
            bool existe = await this.repositorioLogro.ExisteLogroEnCumplidos(logro.Id);
            if (!existe)
            {
                var logroDB = await this.repositorioLogro.ObtenerPorId(logro.Id);
                await this._logroPartidaRepositorio.Guardar(new LogroPartida { Partida = partida, Logro = logroDB });
            }
        }
    }

    public async Task<List<Logro>> ObtenerLogrosParaObtenerRecompensa(int partidaId)
    {
        return await this._logroPartidaRepositorio.ObtenerLogrosNoCumplidos(partidaId);
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
                EsRecompensa = entidad.Condicion.EsRecompensa,
                EstructuraId = entidad.Condicion.Estructura?.Id,
                Estructura = entidad.Condicion.Estructura != null
                    ? new EstructuraCondicionDTO
                    {
                        Id = entidad.Condicion.Estructura.Id,
                        Nombre = entidad.Condicion.Estructura.Nombre
                    }
                    : null,

                // ✅ Agregamos el mapeo de la Recompensa (otra Condicion)
                Recompensa = entidad.Condicion.Recompensa != null
                    ? new CondicionDTO
                    {
                        Id = entidad.Condicion.Recompensa.Id,
                        Cantidad = entidad.Condicion.Recompensa.Cantidad,
                        NombreColumna = entidad.Condicion.Recompensa.NombreColumna,
                        EsRecompensa = entidad.Condicion.Recompensa.EsRecompensa,
                        EstructuraId = entidad.Condicion.Recompensa.Estructura?.Id,
                        Estructura = entidad.Condicion.Recompensa.Estructura != null
                            ? new EstructuraCondicionDTO
                            {
                                Id = entidad.Condicion.Recompensa.Estructura.Id,
                                Nombre = entidad.Condicion.Recompensa.Estructura.Nombre
                            }
                            : null
                    }
                    : null
            }
        };
    }
}
