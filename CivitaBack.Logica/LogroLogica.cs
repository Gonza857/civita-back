using AutoMapper;
using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Domain.Excepciones;
using CivitaBack.Utils;
using CivitaBack.Logica.Interfaces;

namespace CivitaBack.Logica;

public class LogroLogica : ILogroLogica
{
    private readonly ILogroRepositorio _repositorioLogro;
    private readonly ITipoLogroRepositorio repositorioTipoLogro;
    private readonly ICondicionRepositorio _condicionRepositorio;
    private readonly ILogroPartidaRepositorio _logroPartidaRepositorio;
    private IUnidadDeTrabajo _uow;
    private readonly IAccesoUsuarios _accesoUsuarios;

    private readonly List<string> recursos = new List<string> { "Energia", "Contaminacion", "EcoCoins", "Felicidad" };

    public LogroLogica(
        ILogroRepositorio rtl, 
        ITipoLogroRepositorio itlr, 
        ICondicionRepositorio icr,
        ILogroPartidaRepositorio ilpr,
        IUnidadDeTrabajo uow,
        IAccesoUsuarios accesoUsuarios
        )
    {
        _repositorioLogro = rtl;
        repositorioTipoLogro = itlr;
        _condicionRepositorio = icr;
        _logroPartidaRepositorio = ilpr;
        _uow = uow;
        _accesoUsuarios = accesoUsuarios;
    }

    /// <summary>
    /// Valida los datos entrantes del DTO
    /// </summary>
    /// <param name="logroDTO">LogroDTO</param>
    private void ValidarLogro(Logro logro)
    {
        if (logro == null) 
            throw new LogroExcepcion("Ocurrió un error al actualizar el Logro");
    }

    /// <summary>
    /// Actualiza un logro
    /// </summary>
    /// <param name="logroDTO">LogroDTO</param>
    /// /// <param name="id">Id Logro</param>
    public async Task Actualizar(Logro logro, int id)
    {
        ValidarAdmin();

        this.ValidarLogro(logro);
        Logro? logroBuscado = await this._repositorioLogro.ObtenerPorId(id);
        TipoLogro tipoLogroBuscado = await this.repositorioTipoLogro.ObtenerPorId(logro.TipoLogro.Id);
        
        if (logroBuscado == null || tipoLogroBuscado == null) 
            throw new LogroExcepcion("Ocurrió un error al actualizar el Logro");

        logroBuscado.Titulo = logro.Titulo;
        logroBuscado.Descripcion = logro.Descripcion;
        logroBuscado.Titulo = logro.Titulo;
        logroBuscado.TipoLogro = tipoLogroBuscado;

        await this._repositorioLogro.Actualizar(logroBuscado);

        await this._uow.CommitAsync();
    }

    public async Task<List<Logro>> ObtenerListadoInterno()
    {
        return await this._repositorioLogro.ObtenerTodos();
    }

    /// <summary>
    /// Elimina un logro
    /// </summary>
    /// <param name="id">Id Logro</param>
    public async Task Eliminar(int id)
    {
        ValidarAdmin();

        if (id <= 0) 
            throw new LogroExcepcion("No se pudo borrar el Logro");
        await this._repositorioLogro.Eliminar(id);

        await this._uow.CommitAsync();

    }

    /// <summary>
    /// Guarda un logro
    /// </summary>
    /// <param name="logroDTO">LogroDTO</param>
    public async Task Crear(Logro entidad)
    {
        ValidarAdmin();

        this.ValidarLogro(entidad);
        TipoLogro? tipoLogro = await this.repositorioTipoLogro.ObtenerPorId(entidad.TipoLogro.Id);
        if (tipoLogro == null) 
            throw new LogroExcepcion("No se proporcionó Tipo de Logro.");

        Condicion? condicion = await this._condicionRepositorio.ObtenerPorId(entidad.Condicion.Id);
        if (condicion == null)
            throw new LogroExcepcion("No se proporcionó condición.");

        var logro = new Logro
        {
            Titulo = entidad.Titulo,
            Descripcion = entidad.Descripcion,
            TipoLogro = tipoLogro,
            Condicion = condicion
            
        };
        
        await this._repositorioLogro.Agregar(logro);

        await this._uow.CommitAsync();

    }

    /// <summary>
    /// Obtener listado
    /// </summary>
    public async Task<List<Logro>> ObtenerListado()
    {
        var logros = await this._repositorioLogro.ObtenerTodos();
        return logros;
    }

    /// <summary>
    /// Obtener logro por Id
    /// </summary>
    /// <param name="id">Id de Logro</param>
    public async Task<Logro> ObtenerPorId(int id)
    {
        var logro = await this._repositorioLogro.ObtenerPorId(id);
        if (logro == null) 
            throw new LogroExcepcion("No se pudo obtener el logro");
        return logro;
    }

    public async Task<List<Logro>> ObtenerLogrosCumplidos(Partida partida)
    {
        _accesoUsuarios.ValidarAcceso(partida.UsuarioId);

        List<LogroPartida> logrosPartida = partida.LogroPartidas;
        if (logrosPartida.Count != 0) return logrosPartida.Select(lp => lp.Logro).ToList();

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
        return resultado;
    }

    public async Task<List<Logro>> ComprobarSiCumpleAlgunLogro(Partida? partida, List<Logro> logrosDB)
    {
        if (partida == null || partida.Recursos == null)
            throw new LogroExcepcion("No se pudo obtener si cumple algún logro.");

        _accesoUsuarios.ValidarAcceso(partida.UsuarioId);

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
        return logrosFiltrados;
    }
    
    public async Task MarcarLogrosComoCompletados(Partida? partida, List<Logro> logrosDB)
    {
        if (partida == null || logrosDB.Count == 0 || partida.Recursos == null)
            throw new LogroExcepcion("No se pudo obtener si cumple algún logro.");

        _accesoUsuarios.ValidarAcceso(partida.UsuarioId);

        /*
            por cada logro
            verifico que no exista en la bd
            si no esta lo pongo,
            si esta, no
            entro a logro

         */

        foreach (Logro logro in logrosDB)
        {
            bool existe = await this._repositorioLogro.ExisteLogroEnCumplidos(logro.Id);
            if (!existe)
            {
                var logroDB = await this._repositorioLogro.ObtenerPorId(logro.Id);
                await this._logroPartidaRepositorio.Agregar(new LogroPartida { Partida = partida, Logro = logroDB });
            }
        }

        await _uow.CommitAsync();
    }

    public async Task<List<Logro>> ObtenerLogrosParaObtenerRecompensa(int partidaId)
    {
        return await this._logroPartidaRepositorio.ObtenerLogrosNoCumplidos(partidaId);
    }

    private void ValidarAdmin()
    {
        if (!_accesoUsuarios.EsDios())
            throw new AccesoDenegadoExcepcion("Se requieren privilegios de administrador para modificar los logros.");
    }

}
