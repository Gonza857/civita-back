using System.Reflection;
using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Enum;
using CivitaBack.Domain.Excepciones;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Utils;

namespace CivitaBack.Logica;

public class RecompensaLogica : IRecompensaLogica
{
    
    private readonly IPartidaRepositorio _partidaRepositorio;
    private readonly IUnidadDeTrabajo _unidadDeTrabajo;
    private readonly IRecompensaRepositorio _recompensaRepositorio;
    private readonly IEstructuraRepositorio _estructuraRepositorio;
    
    public RecompensaLogica(
        IPartidaRepositorio partidaRepositorio, 
        IUnidadDeTrabajo unidadDeTrabajo, 
        IRecompensaRepositorio recompensaRepositorio,
        IEstructuraRepositorio estructuraRepositorio)
    {
        _partidaRepositorio = partidaRepositorio;
        _unidadDeTrabajo = unidadDeTrabajo;
        _recompensaRepositorio = recompensaRepositorio;
        _estructuraRepositorio = estructuraRepositorio;
    }
    
    /// <inheritdoc />
    public async Task CrearRecompensa(Recompensa recompensa)
    {
        // 1. Valida y procesa la entidad
        await ProcesarCamposExcluyentes(recompensa);

        // 2. Guarda
        await _recompensaRepositorio.Agregar(recompensa);
        await _unidadDeTrabajo.CommitAsync();
    }

    public async Task<List<Recompensa>> Listado()
    {
        return await this._recompensaRepositorio.ObtenerTodos();
    }

    public async Task ReclamarRecompensas(List<Recompensa> recompensas, Partida partida)
    {
        this.ValidarPartida(partida);

        var listaRecompensaRecursos = recompensas.Where((r) => r.NombreColumna != "Experiencia").ToList();
        var listaRecompensaExperiencia = recompensas.Where((r) => r.NombreColumna == "Experiencia").ToList();
        
        this.AplicarRecompensasRecurso(partida.Recursos!, listaRecompensaRecursos);
        this.AplicarExperienciaPartida(partida, listaRecompensaExperiencia);
        
        // await this._partidaRepositorio.Actualizar(partida);
        // await this._unidadDeTrabajo.CommitAsync();
    }
    
    
    /// <summary>
    /// Modifica un objeto <see cref="Recurso"/> en memoria, sumando las cantidades de las recompensas.
    /// </summary>
    /// <param name="recursoPartida">El objeto Recurso a modificar.</param>
    /// <param name="recompensas">La lista de recompensas (Condicion) a aplicar.</param>
    /// <remarks>
    /// Utiliza reflexión de forma optimizada (con un diccionario) para actualizar las propiedades
    /// del objeto Recurso basándose en <c>NombreColumna</c>.
    /// </remarks>
    private void AplicarRecompensasRecurso(Recurso recursoPartida, List<Recompensa> recompensas)
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

    private void AplicarExperienciaPartida(Partida partida, List<Recompensa> recompensas)
    {
        
        recompensas.ForEach((r) =>
        {
            partida.Experiencia += r.Cantidad;
        });
    }
    
    private void ValidarPartida(Partida? partida)
    {
        if (partida == null)
            throw new Exception("Partida invalida");
        
        if (partida.Recursos == null)
            throw new Exception("Recursos invalidos");
    }
    
    /// <summary>
    /// Valida y normaliza los campos EstructuraId y NombreColumna.
    /// Modifica el objeto 'condicion' pasado por referencia.
    /// </summary>
    private async Task ProcesarCamposExcluyentes(Recompensa recompensa)
    {
        ValidarDatosBase(recompensa);
        
        if (recompensa.EstructuraId.HasValue)
        {
            Estructura? e = await _estructuraRepositorio.ObtenerPorId(recompensa.EstructuraId.Value);
            if (e == null)
            {
                throw new CondicionExcepcion($"La estructura con Id {recompensa.EstructuraId.Value} no existe.");
            }

            recompensa.EstructuraId = e.Id;
            recompensa.NombreColumna = null; 
            recompensa.Estructura = null; 
        }
        else if (!string.IsNullOrWhiteSpace(recompensa.NombreColumna))
        {
            recompensa.NombreColumna = TipoRecursoHelper.ParseTipoRecurso(recompensa.NombreColumna).ToString();
            recompensa.EstructuraId = null; 
            recompensa.Estructura = null;
        }
    }
    
    /// <summary>
    /// Valida las reglas de negocio base para una Condicion o Recompensa.
    /// </summary>
    private void ValidarDatosBase(Recompensa recompensa)
    {
        if (!recompensa.EstructuraId.HasValue && string.IsNullOrWhiteSpace(recompensa.NombreColumna))
            throw new DominioException("Debes seleccionar una estructura o un recurso");

        if (recompensa.EstructuraId.HasValue && !string.IsNullOrWhiteSpace(recompensa.NombreColumna))
            throw new DominioException("Debes seleccionar una estructura o un recurso, no ambos");

        if (!recompensa.EstructuraId.HasValue && !TipoRecursoHelper.EsTipoRecursoValido(recompensa.NombreColumna))
            throw new DominioException("El nombre de columna proporcionado es inválido.");

        if (recompensa.Cantidad < 0)
            throw new DominioException("La cantidad no puede ser menor a 0");
    }
    
}