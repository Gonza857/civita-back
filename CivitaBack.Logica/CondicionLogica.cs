using System.Data;
using System.Reflection;
using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Enum;
using CivitaBack.Domain.Excepciones;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Utils;

namespace CivitaBack.Logica;

public class CondicionLogica : ICondicionLogica
{
    private readonly ICondicionRepositorio _condicionRepositorio;
    private readonly IEstructuraRepositorio _estructuraRepositorio;
    private readonly IUnidadDeTrabajo _uow;
    private readonly IRecompensaRepositorio _recompensaRepositorio;

    public CondicionLogica(
        ICondicionRepositorio icr, 
        IEstructuraRepositorio ier, 
        IUnidadDeTrabajo uow,
        IRecompensaRepositorio irr)
    {
        _condicionRepositorio = icr;
        _estructuraRepositorio = ier;
        _uow = uow;
        _recompensaRepositorio = irr;
    }

    /// <inheritdoc />
    public async Task<Condicion> ObtenerPorId(int id)
    {
        var condicion = await _condicionRepositorio.ObtenerPorId(id);
        if (condicion == null)
            throw new CondicionExcepcion($"No se pudo obtener la Condicion con Id {id}");
        return condicion;
    }

    /// <inheritdoc />
    public async Task Crear(Condicion condicion, List<int> recompensasIds)
    {
        List<Recompensa> recompensasDb = await this._recompensaRepositorio.ObtenerVariosPorIds(recompensasIds);
        
        if (recompensasDb.Count != recompensasIds.Count)
            throw new DominioException("No se encontrarón las recompensas");
        
        condicion.Recompensas = recompensasDb;

        await ProcesarCamposExcluyentes(condicion);
        await _condicionRepositorio.Agregar(condicion);
        await _uow.CommitAsync();
    }

    /// <inheritdoc />
    public async Task<List<Condicion>> ObtenerListado()
    {
        return await _condicionRepositorio.ObtenerTodos();
    }

    /// <inheritdoc />
    public async Task Eliminar(int id)
    {
        if (id <= 0)
            throw new Domain.Excepciones.CondicionExcepcion("No se pudo borrar la Condicion. Id inválido.");

        // (Tu Repositorio Genérico ya maneja la lógica de buscar antes de borrar)
        await _condicionRepositorio.Eliminar(id);
        await _uow.CommitAsync();
    }

    /// <inheritdoc />
    public async Task Actualizar(Condicion condicionNuevosDatos, int id, List<int> recompensasIds)
    {
        await this.ProcesarCamposExcluyentes(condicionNuevosDatos);
        this.ValidarIds(recompensasIds);
        
        Condicion? condicionDb = await _condicionRepositorio.ObtenerPorId(id);
        if (condicionDb == null)
            throw new DataException($"No se encontró la Condicion con Id {id} para actualizar.");
        
        List<Recompensa> recompensasDb = await this._recompensaRepositorio.ObtenerVariosPorIds(recompensasIds);

        if (recompensasDb.Count != recompensasIds.Count)
            throw new DominioException("No se encontrarón las recompensas");
        
        condicionDb.Cantidad = condicionNuevosDatos.Cantidad;
        condicionDb.NombreColumna = condicionNuevosDatos.NombreColumna;
        condicionDb.EstructuraId = condicionNuevosDatos.EstructuraId;
        condicionDb.Recompensas = recompensasDb;
        
        await _condicionRepositorio.Actualizar(condicionDb);
        await _uow.CommitAsync();
    }

    public List<Condicion> FiltrarCondicionSiCumple(Condicion condicion, Partida partida)
    {
        List<Condicion> cs = new List<Condicion>{condicion};
        return this.FiltrarCondicionesSiCumplen(cs, partida);
    }
    
    /// <summary>
    /// Filtra una lista de condiciones para devolver solo las que se cumplen.
    /// </summary>
    public List<Condicion> FiltrarCondicionesSiCumplen(List<Condicion> condiciones, Partida partida)
    {
        List<Condicion> condicionesOk = new List<Condicion>();
        foreach (Condicion condicion in condiciones)
        {
            if (SeCumpleCondicion(condicion, partida))
                condicionesOk.Add(condicion);
        }
        return condicionesOk;
    }
    
    /// <summary>
    /// Filtra una lista de condiciones para devolver solo las que se cumplen.
    /// </summary>
    public List<MisionPartida> FiltrarMisionesQueNoCumplen(List<MisionPartida> misionesPartida, Partida partida)
    {
        List<MisionPartida> mpsOk = new List<MisionPartida>();
        
        foreach (MisionPartida mp in misionesPartida)
        {
            if (!SeCumpleCondicion(mp.Mision.Condicion, partida))
                mpsOk.Add(mp);
        }
        return mpsOk;
    }
    
    // --- MÉTODOS PRIVADOS REUTILIZABLES ---
    
    /// <summary>
    /// Método orquestador que determina si una condición se cumple,
    /// llamando al helper correcto (Recurso o Estructura).
    /// </summary>
    private bool SeCumpleCondicion(Condicion condicion, Partida partida)
    {
        // 1. Condición por Recurso
        if (!string.IsNullOrEmpty(condicion.NombreColumna))
        {
            if (partida.Recursos == null)
                throw new CondicionExcepcion("La partida no tiene Recursos cargados.");

            return SeCumplePorRecurso(condicion, partida.Recursos);
        }

        // 2. Condición por Estructura
        if (condicion.EstructuraId != null)
        {
            if (partida.EstructuraMapa == null)
                throw new CondicionExcepcion("La partida no tiene EstructuraMapa cargada.");

            return SeCumplePorEstructura(condicion, partida.EstructuraMapa);
        }

        // 3. Condición inválida
        throw new CondicionExcepcion($"La Condición {condicion.Id} no tiene NombreColumna ni EstructuraId.");
    }

    /// <summary>
    /// Diccionario cacheado de las propiedades 'int' de la clase Recurso.
    /// Es estático para un rendimiento óptimo (solo se crea una vez).
    /// </summary>
    private static readonly Dictionary<string, PropertyInfo> PropiedadesRecurso =
        typeof(Recurso)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.PropertyType == typeof(int))
            .ToDictionary(p => p.Name, p => p, StringComparer.OrdinalIgnoreCase); // Case-insensitive

    /// <summary>
    /// Verifica si una condición de tipo RECURSO se cumple.
    /// </summary>
    private bool SeCumplePorRecurso(Condicion condicion, Recurso recursoPartida)
    {
        // Usa el diccionario estático _propiedadesRecurso
        if (PropiedadesRecurso.TryGetValue(condicion.NombreColumna!, out var propiedad))
        {
            int valorActual = (int)propiedad.GetValue(recursoPartida)!;
            return valorActual >= condicion.Cantidad;
        }

        // Si la columna no existe en el diccionario, es un error de configuración
        throw new CondicionExcepcion(
            $"Nombre de columna '{condicion.NombreColumna}' no es válido para verificar condición.");
    }

    /// <summary>
    /// Verifica si una condición de tipo ESTRUCTURA se cumple.
    /// </summary>
    private bool SeCumplePorEstructura(Condicion condicion, List<EstructuraMapa> estructuraMapa)
    {
        int cantidadTotal = estructuraMapa
            .Count(em => em.EstructuraId == condicion.EstructuraId!.Value);

        return cantidadTotal >= condicion.Cantidad;
    }

    /// <summary>
    /// Valida las reglas de negocio base para una Condicion o Recompensa.
    /// </summary>
    private void ValidarDatosBase(Condicion condicion)
    {
        if (!condicion.EstructuraId.HasValue && string.IsNullOrWhiteSpace(condicion.NombreColumna))
            throw new CondicionExcepcion("Debes seleccionar una estructura o un recurso");

        if (condicion.EstructuraId.HasValue && !string.IsNullOrWhiteSpace(condicion.NombreColumna))
            throw new CondicionExcepcion("Debes seleccionar una estructura o un recurso, no ambos");

        if (!condicion.EstructuraId.HasValue && !TipoRecursoHelper.EsTipoRecursoValido(condicion.NombreColumna))
            throw new CondicionExcepcion("El nombre de columna proporcionado es inválido.");

        if (condicion.Cantidad < 0)
            throw new CondicionExcepcion("La cantidad no puede ser menor a 0");
    }

    private void ValidarIds(List<int> ids)
    {
        ids.ForEach((id) =>
        {
            if (id <= 0) throw new DominioException("Las recompensas recibidas son inválidas");
        });
    }

    /// <summary>
    /// Valida y normaliza los campos EstructuraId y NombreColumna.
    /// Modifica el objeto 'condicion' pasado por referencia.
    /// </summary>
    private async Task ProcesarCamposExcluyentes(Condicion condicion)
    {
        // 1. Validar la lógica
        ValidarDatosBase(condicion);

        // 2. Procesar (Normalizar)
        if (condicion.EstructuraId.HasValue)
        {
            Estructura? e = await _estructuraRepositorio.ObtenerPorId(condicion.EstructuraId.Value);
            if (e == null)
            {
                throw new CondicionExcepcion($"La estructura con Id {condicion.EstructuraId.Value} no existe.");
            }

            condicion.EstructuraId = e.Id;
            condicion.NombreColumna = null; // Asegura exclusividad
            condicion.Estructura = null; // No guardamos el objeto de navegación
        }
        else if (!string.IsNullOrWhiteSpace(condicion.NombreColumna))
        {
            condicion.NombreColumna = TipoRecursoHelper.ParseTipoRecurso(condicion.NombreColumna).ToString();
            condicion.EstructuraId = null; // Asegura exclusividad
            condicion.Estructura = null;
        }
    }
}