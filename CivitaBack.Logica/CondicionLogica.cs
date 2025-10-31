using CivitaBack.Data.Enum;
using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Logica.Excepciones;
using CivitaBack.Utils;

namespace CivitaBack.Logica;

public interface ICondicionLogica
{
    Task<CondicionDTO> ObtenerPorId(int id);
    Task Crear(CondicionDTO entidad);
    Task<List<CondicionDTO>> ObtenerListado();
    Task Eliminar(int id);
    Task Actualizar(CondicionDTO condicionDto, int id);

    Task<List<CondicionDTO>> ObtenerListadoRecompensas();
    Task CrearRecompensa(CondicionDTO recompensa);
    
}

public class CondicionLogica : ICondicionLogica, IParser<Condicion, CondicionDTO>
{
    private readonly ICondicionRepositorio _condicionRepositorio;
    private readonly IEstructuraRepositorio estructuraRepositorio;

    public CondicionLogica(ICondicionRepositorio icr, IEstructuraRepositorio ier)
    {
        this._condicionRepositorio = icr;
        this.estructuraRepositorio = ier;
    }

    /// <summary>
    /// Obtener Condicion
    /// </summary>
    /// <param name="id">Id de Condicion</param>
    public async Task<CondicionDTO> ObtenerPorId(int id)
    {
        var condicion = await this._condicionRepositorio.ObtenerPorId(id);
        if (condicion == null)
            throw new LogroExcepcion("No se pudo obtener la Condicion");
        return this.ToDto(condicion);
    }

    /// <summary>
    /// Guarda una Condicion
    /// </summary>
    /// <param name="condicionDto">CondicionDTO</param>
    public async Task Crear(CondicionDTO condicionDto)
    {
        this.ValidarCondicionDTO(condicionDto);

        var condicion = new Condicion();

        if (condicionDto.EstructuraId.HasValue)
        {
            Estructura e = await estructuraRepositorio.ObtenerPorId(condicionDto.EstructuraId.Value);
            if (e != null)
            {
                condicion.EstructuraId = e.Id;
                condicion.NombreColumna = null;
            }
        }

        if (condicionDto.NombreColumna != null)
        {
            condicion.NombreColumna = TipoRecursoHelper.ParseTipoRecurso(condicionDto.NombreColumna).ToString();
            condicion.EstructuraId = null;
        }

        condicion.Cantidad = condicionDto.Cantidad;

        await _condicionRepositorio.Agregar(condicion);
    }

    /// <summary>
    /// Obtener listado
    /// </summary>
    public async Task<List<CondicionDTO>> ObtenerListado()
    {
        var condiciones = await this._condicionRepositorio.ObtenerTodos();
        return condiciones
            .Select(p => this.ToDto(p))
            .ToList();
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
    }

    /// <summary>
    /// Actualiza una Condicion
    /// </summary>
    /// <param name="condicionDto">CondicionDTO</param>
    /// /// <param name="id">Id Condicion</param>
    public async Task Actualizar(CondicionDTO condicionDto, int id)
    {

        this.ValidarCondicionDTO(condicionDto);

        Condicion? condicionDb = await this._condicionRepositorio.ObtenerPorId(id);
        if (condicionDb == null)
            throw new CondicionExcepcion("Ocurrió un error al actualizar la Condicion");


        if (condicionDto.EstructuraId.HasValue)
        {
            Estructura e = await this.estructuraRepositorio.ObtenerPorId(condicionDto.EstructuraId.Value);
            if (e != null)
            {
                condicionDb.EstructuraId = e.Id;
                condicionDb.NombreColumna = null;
            }
        }

        if (condicionDto.NombreColumna != null)
        {
            condicionDb.NombreColumna = TipoRecursoHelper.ParseTipoRecurso(condicionDto.NombreColumna).ToString();
            condicionDb.EstructuraId = null;
        }
        
        Condicion? recompensaDb;
        if (condicionDto.RecompensaId.HasValue)
        {
            recompensaDb = await this._condicionRepositorio.ObtenerPorId(condicionDto.RecompensaId.Value);
            if (recompensaDb == null)
                throw new CondicionExcepcion("No se encontró la recompensa para asociar");
            condicionDb.Recompensa = recompensaDb;
        }

        condicionDb.Cantidad = condicionDto.Cantidad;

        await this._condicionRepositorio.Actualizar(condicionDb);
    }

    /// <summary>
    /// Obtiene listado de recompensas
    /// </summary>
    public async Task<List<CondicionDTO>> ObtenerListadoRecompensas()
    {
        var recompensas = await this._condicionRepositorio.ObtenerTodasRecompensas();
        return recompensas
            .Select(p => this.ToRecompensaDto(p))
            .ToList();
    }

    public async Task CrearRecompensa(CondicionDTO recompensaDto)
    {
        this.ValidarRecompensaDTO(recompensaDto);

        var recompensa = new Condicion();

        if (recompensaDto.EstructuraId.HasValue)
        {
            Estructura? e = await this.estructuraRepositorio.ObtenerPorId(recompensaDto.EstructuraId.Value);
            if (e != null)
            {
                recompensa.EstructuraId = e.Id;
                recompensa.NombreColumna = null;
            }
        }

        if (recompensaDto.NombreColumna != null)
        {
            recompensa.NombreColumna = TipoRecursoHelper.ParseTipoRecurso(recompensaDto.NombreColumna).ToString();
            recompensa.EstructuraId = null;
        }

        recompensa.Cantidad = recompensaDto.Cantidad;
        recompensa.EsRecompensa = true;

        await this._condicionRepositorio.Agregar(recompensa);
    }

    private void ValidarCondicionDTO(CondicionDTO condicionDto)
    {
        // Si no manda estructura ni recurso, rebotado
        if (!condicionDto.EstructuraId.HasValue && condicionDto.NombreColumna == null)
            throw new CondicionExcepcion("Debes seleccionar una estructura o un recurso");

        // Si manda las 2 estructura y recurso, rebotado
        if (condicionDto.EstructuraId.HasValue && condicionDto.NombreColumna != null)
            throw new CondicionExcepcion("Debes seleccionar una estructura o un recurso");

        // Si no mando estructura y el tipo de recurso enviado esta mal, rebotado
        if (!condicionDto.EstructuraId.HasValue && !TipoRecursoHelper.EsTipoRecursoValido(condicionDto.NombreColumna))
            throw new CondicionExcepcion("El nombre de columna proporcionado es inválido.");

        if (condicionDto.Cantidad < 0)
            throw new CondicionExcepcion("La cantidad no puede ser menor a 0");
    }

    private void ValidarRecompensaDTO(CondicionDTO recompensaDto)
    {
        // Si no manda estructura ni recurso,  -> Solo se permite 1 recompensa de 1 tipo
        if (!recompensaDto.EstructuraId.HasValue && recompensaDto.NombreColumna == null)
            throw new CondicionExcepcion("Debes seleccionar una estructura o un recurso");

        // Si manda las 2 estructura y recurso, rebotado -> Solo 1 de las 2 se permite
        if (recompensaDto.EstructuraId.HasValue && recompensaDto.NombreColumna != null)
            throw new CondicionExcepcion("Debes seleccionar una estructura o un recurso");

        // Si no mando estructura y el tipo de recurso enviado esta mal, rebotado
        if (!recompensaDto.EstructuraId.HasValue && !TipoRecursoHelper.EsTipoRecursoValido(recompensaDto.NombreColumna))
            throw new CondicionExcepcion("El nombre de columna proporcionado es inválido.");

        if (recompensaDto.Cantidad < 0)
            throw new CondicionExcepcion("La cantidad de la recompensa no puede ser menor a 0");
    }

    private CondicionDTO ToRecompensaDto(Condicion recompensa)
    {
        CondicionDTO c = new CondicionDTO
        {
            Cantidad = recompensa.Cantidad,
            Id = recompensa.Id,
            NombreColumna = recompensa.NombreColumna,
        };
        return c;
    }

    public CondicionDTO ToDto(Condicion entidad)
    {
        var dto = new CondicionDTO
        {
            Id = entidad.Id,
            Cantidad = entidad.Cantidad,
            EsRecompensa = entidad.EsRecompensa,
            NombreColumna = entidad.NombreColumna,
            EstructuraId = entidad.EstructuraId,
            Estructura = entidad.Estructura != null
                ? new EstructuraCondicionDTO { Id = entidad.Estructura.Id, Nombre = entidad.Estructura.Nombre }
                : null,
            Recompensa = entidad.Recompensa != null
                ? new CondicionDTO
                {
                    Id = entidad.Recompensa.Id,
                    Cantidad = entidad.Recompensa.Cantidad,
                    NombreColumna = entidad.Recompensa.NombreColumna,
                    EsRecompensa = entidad.Recompensa.EsRecompensa,
                    EstructuraId = entidad.Recompensa.EstructuraId,
                    Estructura = entidad.Recompensa.Estructura != null
                        ? new EstructuraCondicionDTO
                            { Id = entidad.Recompensa.Estructura.Id, Nombre = entidad.Recompensa.Estructura.Nombre }
                        : null
                }
                : null
        };

        return dto;
    }
}
