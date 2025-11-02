using CivitaBack.Data.Enum;
using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Logica.Excepciones;
using CivitaBack.Utils;

namespace CivitaBack.Logica;

public class CondicionLogica : ICondicionLogica
{
    private readonly ICondicionRepositorio _condicionRepositorio;
    private readonly IEstructuraRepositorio estructuraRepositorio;
    private readonly IUnidadDeTrabajo _uow;

    public CondicionLogica(ICondicionRepositorio icr, IEstructuraRepositorio ier, IUnidadDeTrabajo uow)
    {
        this._condicionRepositorio = icr;
        this.estructuraRepositorio = ier;
        _uow = uow;
    }

    /// <summary>
    /// Obtener Condicion
    /// </summary>
    /// <param name="id">Id de Condicion</param>
    public async Task<Condicion> ObtenerPorId(int id)
    {
        var condicion = await this._condicionRepositorio.ObtenerPorId(id);
        if (condicion == null)
            throw new LogroExcepcion("No se pudo obtener la Condicion");
        return condicion;
    }

    /// <summary>
    /// Guarda una Condicion
    /// </summary>
    /// <param name="condicionDto">CondicionDTO</param>
    public async Task Crear(Condicion condicion)
    {
        this.ValidarCondicion(condicion);

        if (condicion.EstructuraId.HasValue)
        {
            Estructura e = await estructuraRepositorio.ObtenerPorId(condicion.EstructuraId.Value);
            if (e != null)
            {
                condicion.EstructuraId = e.Id;
                condicion.NombreColumna = null;
            }
        }

        if (condicion.NombreColumna != null)
        {
            condicion.NombreColumna = TipoRecursoHelper.ParseTipoRecurso(condicion.NombreColumna).ToString();
            condicion.EstructuraId = null;
        }

        condicion.Cantidad = condicion.Cantidad;

        await _condicionRepositorio.Agregar(condicion);

        await _uow.CommitAsync();
    }

    /// <summary>
    /// Obtener listado
    /// </summary>
    public async Task<List<Condicion>> ObtenerListado()
    {
        return await this._condicionRepositorio.ObtenerTodos();
    }

    /// <summary>
    /// Elimina una Condicion
    /// </summary>
    /// <param name="id">Id Condicion</param>
    public async Task Eliminar(int id)
    {
        if (id <= 0)
            throw new LogroExcepcion("No se pudo borrar la Condicion");
        await this._condicionRepositorio.Eliminar(id);

        await _uow.CommitAsync();
    }

    /// <summary>
    /// Actualiza una Condicion
    /// </summary>
    /// <param name="condicionDto">CondicionDTO</param>
    /// /// <param name="id">Id Condicion</param>
    public async Task Actualizar(Condicion condicion, int id)
    {

        this.ValidarCondicion(condicion);

        Condicion? condicionDb = await this._condicionRepositorio.ObtenerPorId(id);
        if (condicionDb == null)
            throw new CondicionExcepcion("Ocurrió un error al actualizar la Condicion");


        if (condicion.EstructuraId.HasValue)
        {
            Estructura e = await this.estructuraRepositorio.ObtenerPorId(condicion.EstructuraId.Value);
            if (e != null)
            {
                condicionDb.EstructuraId = e.Id;
                condicionDb.NombreColumna = null;
            }
        }

        if (condicion.NombreColumna != null)
        {
            condicionDb.NombreColumna = TipoRecursoHelper.ParseTipoRecurso(condicion.NombreColumna).ToString();
            condicionDb.EstructuraId = null;
        }
        
        Condicion? recompensaDb;
        if (condicion.RecompensaId.HasValue)
        {
            recompensaDb = await this._condicionRepositorio.ObtenerPorId(condicion.RecompensaId.Value);
            if (recompensaDb == null)
                throw new CondicionExcepcion("No se encontró la recompensa para asociar");
            condicionDb.Recompensa = recompensaDb;
        }

        condicionDb.Cantidad = condicion.Cantidad;

        await this._condicionRepositorio.Actualizar(condicionDb);

        await _uow.CommitAsync();
    }

    /// <summary>
    /// Obtiene listado de recompensas
    /// </summary>
    public async Task<List<Condicion>> ObtenerListadoRecompensas()
    {
        var recompensas = await this._condicionRepositorio.ObtenerTodasRecompensas();
        return recompensas;
    }

    public async Task CrearRecompensa(Condicion recompensa)
    {
        this.ValidarRecompensa(recompensa);

        if (recompensa.EstructuraId.HasValue)
        {
            Estructura? e = await this.estructuraRepositorio.ObtenerPorId(recompensa.EstructuraId.Value);
            if (e != null)
            {
                recompensa.EstructuraId = e.Id;
                recompensa.NombreColumna = null;
            }
        }

        if (recompensa.NombreColumna != null)
        {
            recompensa.NombreColumna = TipoRecursoHelper.ParseTipoRecurso(recompensa.NombreColumna).ToString();
            recompensa.EstructuraId = null;
        }

        recompensa.Cantidad = recompensa.Cantidad;
        recompensa.EsRecompensa = true;

        await this._condicionRepositorio.Agregar(recompensa);
        await _uow.CommitAsync();
    }

    private void ValidarCondicion(Condicion condicion)
    {
        // Si no manda estructura ni recurso, rebotado
        if (!condicion.EstructuraId.HasValue && condicion.NombreColumna == null)
            throw new CondicionExcepcion("Debes seleccionar una estructura o un recurso");

        // Si manda las 2 estructura y recurso, rebotado
        if (condicion.EstructuraId.HasValue && condicion.NombreColumna != null)
            throw new CondicionExcepcion("Debes seleccionar una estructura o un recurso");

        // Si no mando estructura y el tipo de recurso enviado esta mal, rebotado
        if (!condicion.EstructuraId.HasValue && !TipoRecursoHelper.EsTipoRecursoValido(condicion.NombreColumna))
            throw new CondicionExcepcion("El nombre de columna proporcionado es inválido.");

        if (condicion.Cantidad < 0)
            throw new CondicionExcepcion("La cantidad no puede ser menor a 0");
    }

    private void ValidarRecompensa(Condicion recompensa)
    {
        // Si no manda estructura ni recurso,  -> Solo se permite 1 recompensa de 1 tipo
        if (!recompensa.EstructuraId.HasValue && recompensa.NombreColumna == null)
            throw new CondicionExcepcion("Debes seleccionar una estructura o un recurso");

        // Si manda las 2 estructura y recurso, rebotado -> Solo 1 de las 2 se permite
        if (recompensa.EstructuraId.HasValue && recompensa.NombreColumna != null)
            throw new CondicionExcepcion("Debes seleccionar una estructura o un recurso");

        // Si no mando estructura y el tipo de recurso enviado esta mal, rebotado
        if (!recompensa.EstructuraId.HasValue && !TipoRecursoHelper.EsTipoRecursoValido(recompensa.NombreColumna))
            throw new CondicionExcepcion("El nombre de columna proporcionado es inválido.");

        if (recompensa.Cantidad < 0)
            throw new CondicionExcepcion("La cantidad de la recompensa no puede ser menor a 0");
    }
}
