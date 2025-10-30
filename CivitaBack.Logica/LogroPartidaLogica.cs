using System.Reflection;
using CivitaBack.Data.DTO;
using CivitaBack.Domain.Entities;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Logica.Excepciones;
using CivitaBack.Utils;
using Microsoft.Extensions.Logging;

namespace CivitaBack.Logica;

/// <summary>
/// Define la lógica de negocio para manejar los logros de un jugador dentro de una partida.
/// </summary>
public interface ILogroPartidaLogica
{
    /// <summary>
    /// Obtiene una lista de todos los logros que el jugador AÚN NO ha completado en la partida.
    /// </summary>
    /// <param name="partida">La partida actual, debe incluir Recursos y EstructuraMapa.</param>
    /// <returns>Una lista de DTOs de logros incompletos.</returns>
    Task<List<LogroDTO>> ObtenerLogrosIncompletos(Partida? partida);

    /// <summary>
    /// Obtiene una lista de todos los logros que el jugador YA ha completado y están registrados.
    /// </summary>
    /// <param name="partida">La partida actual, debe incluir Recursos y EstructuraMapa.</param>
    /// <returns>Una lista de DTOs de logros completados.</returns>
    Task<List<LogroDTO>> ObtenerLogrosCompletados(Partida? partida);

    /// <summary>
    /// Obtiene los logros incompletos cuyas condiciones se cumplen AHORA MISMO según el estado de la partida.
    /// </summary>
    /// <param name="partida">La partida actual, debe incluir Recursos y EstructuraMapa.</param>
    /// <returns>Una lista de DTOs de logros que están listos para ser reclamados.</returns>
    Task<List<LogroDTO>> ObtenerLogrosParaReclamar(Partida? partida);

    /// <summary>
    /// Procesa el reclamo de TODOS los logros cuyas condiciones se cumplan.
    /// Aplica las recompensas y guarda los logros como completados.
    /// </summary>
    /// <param name="partida">La partida actual, debe incluir Recursos y EstructuraMapa.</param>
    /// <returns>Una tarea que representa la operación de reclamo.</returns>
    Task ReclamarLogros(Partida? partida);

    /// <summary>
    /// Reinicia (borra) el progreso de todos los logros para una partida específica.
    /// </summary>
    /// <param name="partidaId">El ID de la partida a reiniciar.</param>
    /// <returns>Una tarea que representa la operación de reinicio.</returns>
    Task ReiniciarLogros(int partidaId);

}

/// <summary>
/// Implementación de la lógica de negocio para <see cref="ILogroPartidaLogica"/>.
/// </summary>
public class LogroPartidaLogica : ILogroPartidaLogica, IParser<Logro, LogroDTO>
{
    private readonly ILogroPartidaRepositorio _repositorioLogroPartida;
    private readonly IRecursoRepositorio _recursoRepositorio;
    private readonly IUnidadDeTrabajo _unidadDeTrabajo;
    private readonly ILogger<LogroPartidaLogica> _logger;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="LogroPartidaLogica"/>.
    /// </summary>
    /// <param name="rlp">Repositorio para la entidad de unión LogroPartida.</param>
    /// <param name="irr">Repositorio para la entidad Recurso.</param>
    /// <param name="iudt">Repositorio para guardar.</param>
    /// <param name="logger">Logger.</param>
    public LogroPartidaLogica(
            ILogroPartidaRepositorio rlp, 
            IRecursoRepositorio irr, 
            IUnidadDeTrabajo iudt,
            ILogger<LogroPartidaLogica> logger
        )
    {
        _repositorioLogroPartida = rlp;
        _recursoRepositorio = irr;
        _unidadDeTrabajo = iudt;
        _logger = logger;
    }
    
    /// <inheritdoc />
    public async Task<List<LogroDTO>> ObtenerLogrosIncompletos(Partida? partida)
    {
        this.ValidarPartida(partida);
        List<Logro> logrosNoCompletos = await _repositorioLogroPartida.ObtenerLogrosIncompletos(partida!.Id);
        return logrosNoCompletos.Select(l => this.ToDto(l)).ToList();
    }

    /// <inheritdoc />
    public async Task<List<LogroDTO>> ObtenerLogrosCompletados(Partida? partida)
    {
        this.ValidarPartida(partida);
        List<Logro> logrosCompletados = await _repositorioLogroPartida.ObtenerLogrosCompletos(partida!.Id);
        return logrosCompletados.Select(l => this.ToDto(l)).ToList();

    }
   
    /// <inheritdoc />
    public async Task<List<LogroDTO>> ObtenerLogrosParaReclamar(Partida? partida)
    {
        this.ValidarPartida(partida);
        var logros = await this.ObtenerLogrosParaReclamables(partida!);
        return this.ListToDto(logros);
    }

    /// <inheritdoc />
    public async Task ReclamarLogros(Partida? partidaInput)
    {
        var partidaValidada =  this.ValidarPartida(partidaInput);
        
        // 1. Obtener los logros que cumplen la condición ahora mismo
        var logros = await this.ObtenerLogrosParaReclamables(partidaValidada);
        if (logros.Count == 0) return; // No hay nada que reclamar
        
        // 2. Obtener todas las recompensas de esos logros
        List<Condicion> recompensas = logros
            .Where(l => l.Condicion?.Recompensa != null)
            .Select(l => l.Condicion!.Recompensa!)
            .ToList();

        // 3. Aplicar las recompensas al objeto Recurso en memoria
        this.AplicarRecompensasRecurso(partidaValidada.Recursos!, recompensas);
        
        // 4. Guardar cada logro en la tabla LogroPartida para marcarlo como completado
        List<LogroPartida> logrosCumplidosParaAgregar = new List<LogroPartida>();
        foreach (Logro logro in logros)
        {
            logrosCumplidosParaAgregar.Add(
                new LogroPartida { PartidaId = partidaValidada.Id, LogroId = logro.Id }
                );
        }
        
        await this._repositorioLogroPartida.AgregarVarios(logrosCumplidosParaAgregar); // Agrego logros para guardar
        await this._recursoRepositorio.Actualizar(partidaValidada.Recursos!); // Actualizo recuros
        await this._unidadDeTrabajo.CommitAsync();
    }

    /// <inheritdoc />
    public async Task ReiniciarLogros(int partidaId)
    {
        await _repositorioLogroPartida.ReiniciarLogrosPartida(partidaId);
    }
    
    /// <summary>
    /// Modifica un objeto <see cref="Data.BO.Recurso"/> en memoria, sumando las cantidades de las recompensas.
    /// </summary>
    /// <param name="recursoPartida">El objeto Recurso a modificar.</param>
    /// <param name="recompensas">La lista de recompensas (Condicion) a aplicar.</param>
    /// <remarks>
    /// Utiliza reflexión de forma optimizada (con un diccionario) para actualizar las propiedades
    /// del objeto Recurso basándose en <c>NombreColumna</c>.
    /// </remarks>
    private void AplicarRecompensasRecurso(Recurso recursoPartida, List<Condicion> recompensas)
    {
        // Optimización: Cachear propiedades de Recurso en un diccionario
        var propiedadesIntRecurso = typeof(Recurso)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.PropertyType == typeof(int) && p.CanWrite) // Solo ints escribibles
            .ToDictionary(p => p.Name, p => p); // Key: "Energia", Value: PropertyInfo de Energia

        // Iteración para aplicar recompensas
        foreach (var recompensa in recompensas)
        {
            if (string.IsNullOrEmpty(recompensa.NombreColumna))
                continue; 

            // Buscar propiedad en diccionario (rápido)
            if (propiedadesIntRecurso.TryGetValue(recompensa.NombreColumna, out var propiedad))
            {
                // Sumar recompensa
                int valorActual = (int)propiedad.GetValue(recursoPartida)!;
                propiedad.SetValue(recursoPartida, valorActual + recompensa.Cantidad);
            }
            else
            {
                // ⚠️ Loggear si la propiedad no se encuentra
                Console.WriteLine($"⚠️ Propiedad {recompensa.NombreColumna} no encontrada o no es int/escribible en Recurso");
            }
        }
    }

    /// <summary>
    /// Método orquestador privado que obtiene y filtra los logros listos para reclamar.
    /// </summary>
    /// <param name="partida">La partida validada.</param>
    /// <returns>Una lista de entidades <see cref="LogroEF"/> que cumplen las condiciones.</returns>
    private async Task<List<Logro>> ObtenerLogrosParaReclamables(Partida partida)
    {
        // 1. Obtener los que AÚN NO están en la tabla LogroPartida
        List<Logro> logrosNoCompletos = await _repositorioLogroPartida.ObtenerLogrosIncompletos(partida!.Id);
        // 2. Filtrar esa lista contra el estado actual de la partida
        return this.FiltrarLogrosQueCumplenCondicion(logrosNoCompletos, partida);
    }
    
    /// <summary>
    /// Filtra una lista de logros contra el estado actual de la partida (Recursos y Estructuras).
    /// </summary>
    /// <param name="logrosNoCompletos">La lista de logros a verificar.</param>
    /// <param name="partida">La partida actual, con Recursos y EstructuraMapa.</param>
    /// <returns>Una sub-lista de logros cuyas condiciones se cumplen.</returns>
    /// <exception cref="LogroExcepcion">Se lanza si una condición no es válida (ni columna ni estructura).</exception>
    private List<Logro> FiltrarLogrosQueCumplenCondicion(List<Logro> logrosNoCompletos, Partida partida)
    {
        List<Logro> logrosParaReclamar = new List<Logro>();
        Recurso recursoPartida = partida.Recursos!; 
        
        // --- MEJORA ---
        // Cachea las propiedades UNA SOLA VEZ, antes del loop
        var propiedadesIntRecurso = typeof(Recurso)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.PropertyType == typeof(int))
            .ToDictionary(p => p.Name, p => p);
        // --- FIN MEJORA ---
    
        foreach (Logro logro in logrosNoCompletos)
        {
            Condicion condicion = logro.Condicion;

            // Condición por Recurso (ej: "Energia" >= 500)
            if (!string.IsNullOrEmpty(condicion.NombreColumna))
            {
                if (propiedadesIntRecurso.TryGetValue(condicion.NombreColumna, out var propiedad))
                {
                    int valorColumna = (int)propiedad.GetValue(recursoPartida)!;
                    if (valorColumna >= condicion.Cantidad)
                        logrosParaReclamar.Add(logro);
                }
                else
                {
                    _logger.LogWarning("Condición de logro {LogroId} apunta a columna no válida: {NombreColumna}", logro.Id, condicion.NombreColumna);
                }
            }
            // Condición por Estructura (ej: Count(EstructuraId == 5) >= 2)
            else if (condicion.EstructuraId != null)
            {
                int cantidadTotal = partida.EstructuraMapa!
                    .Count(em => em.EstructuraId == condicion.EstructuraId);

                if (cantidadTotal >= condicion.Cantidad)
                    logrosParaReclamar.Add(logro);
            }
            else throw new LogroExcepcion("Ocurrió un error al procesar los logros para reclamar.");

        }

        return logrosParaReclamar;
    }
    

    /// <summary>
    /// Convierte una entidad <see cref="LogroEF"/> a su <see cref="LogroDTO"/> correspondiente.
    /// </summary>
    /// <param name="entidad">La entidad Logro a convertir.</param>
    /// <returns>El DTO.</returns>
    public LogroDTO ToDto(Logro entidad)
    {
        return new LogroDTO
        {
            Id = entidad.Id,
            Descripcion = entidad.Descripcion,
            Tipo = entidad.TipoLogro.Nombre,
            Titulo = entidad.Titulo,
            TipoId = entidad.TipoLogro.Id
        };
    }
    
    /// <summary>
    /// Validador privado para asegurar que la partida y sus navegaciones esenciales no sean nulas.
    /// </summary>
    /// <param name="partida">La partida a validar.</param>
    /// <returns>La misma instancia de partida, garantizada de no ser nula.</returns>
    /// <exception cref="LogroPartidaExcepcion">Si la partida, Recursos o EstructuraMapa son nulos.</exception>
    private Partida ValidarPartida(Partida? partida)
    {
        if (partida == null)
            throw new LogroPartidaExcepcion("Ocurrió un error al obtener los logros del usuario.");
        if (partida.Recursos == null || partida.EstructuraMapa == null)
            throw new LogroPartidaExcepcion("Ocurrió un error al obtener los logros del usuario.");
        return partida;
    }

    /// <summary>
    /// Helper para convertir una lista de entidades <see cref="LogroEF"/> a DTOs.
    /// </summary>
    /// <param name="logros">La lista de entidades.</param>
    /// <returns>La lista de DTOs.</returns>
    private List<LogroDTO> ListToDto(List<Logro> logros)
    {
        return logros.Select(l => this.ToDto(l)).ToList();
    }
    
}