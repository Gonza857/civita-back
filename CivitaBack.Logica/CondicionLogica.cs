using CivitaBack.Data.BO;
using CivitaBack.Data.DTO;
using CivitaBack.Data.Enum;
using CivitaBack.Data.Repositorio;
using CivitaBack.Logica.Excepciones;
using CivitaBack.Tests;
using CivitaBack.Utils;

namespace CivitaBack.Logica;

public interface ICondicionLogica
{
    Task<CondicionDTO> ObtenerPorId(int id);
    Task Crear(CondicionDTO entidad);
    Task<List<CondicionDTO>> ObtenerListado();
    Task Eliminar(int id);
    Task Actualizar(CondicionDTO condicionDto, int id);
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
            Estructura e = await this.estructuraRepositorio.ObtenerPorId(condicionDto.EstructuraId.Value);
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
        
        await this._condicionRepositorio.Guardar(condicion);
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
        Condicion? condicionDb = await this._condicionRepositorio.ObtenerPorId(id);
        
        if (condicionDb == null) 
            throw new CondicionExcepcion("Ocurrió un error al actualizar la Condicion");

        this.ValidarCondicionDTO(condicionDto);

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


        condicionDb.Cantidad = condicionDto.Cantidad;

        await this._condicionRepositorio.Actualizar(condicionDb);
    }

    private void ValidarCondicionDTO (CondicionDTO condicionDto)
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


    public CondicionDTO ToDto(Condicion entidad)
    {
        CondicionDTO c = new CondicionDTO
        {
            Cantidad = entidad.Cantidad,
            Id = entidad.Id,
        };

        if (entidad.NombreColumna == null)
        {
            c.Estructura = new EstructuraCondicionDTO
            {
                Id = entidad.Estructura.Id,
                Nombre = entidad.Estructura.Nombre,
            };
        } else
        {
            c.NombreColumna = entidad.NombreColumna;
        }
        return c;
    }
}