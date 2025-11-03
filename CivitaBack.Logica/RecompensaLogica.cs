using System.Reflection;
using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Utils;

namespace CivitaBack.Logica;

public class RecompensaLogica : IRecompensaLogica
{
    
    private readonly IPartidaRepositorio _partidaRepositorio;
    private readonly IUnidadDeTrabajo _unidadDeTrabajo;
    
    public RecompensaLogica(IPartidaRepositorio partidaRepositorio, IUnidadDeTrabajo unidadDeTrabajo)
    {
        _partidaRepositorio = partidaRepositorio;
        _unidadDeTrabajo = unidadDeTrabajo;
    }

    public async Task ReclamarRecompensas(List<Condicion> recompensas, Partida partida)
    {
        this.ValidarPartida(partida);
        this.ValidarRecompensas(recompensas);
        this.AplicarRecompensasRecurso(partida.Recursos!, recompensas);
        await this._partidaRepositorio.Actualizar(partida);
        await this._unidadDeTrabajo.CommitAsync();
    }

    public async Task ReclamarRecompensa(Condicion recompensa, Partida partida)
    {
        this.ValidarPartida(partida);
        this.ValidarRecompensa(recompensa);
        List<Condicion> recompensas = new List<Condicion>{recompensa};
        this.AplicarRecompensasRecurso(partida.Recursos!, recompensas);
        await this._partidaRepositorio.Actualizar(partida);
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
    
    private void ValidarPartida(Partida? partida)
    {
        if (partida == null)
            throw new Exception("Partida invalida");
        
        if (partida.Recursos == null)
            throw new Exception("Recursos invalidos");
    }

    private void ValidarRecompensas(List<Condicion> recompensas)
    {
        foreach (var r in recompensas) this.ValidarRecompensa(r);
    }

    private void ValidarRecompensa(Condicion recompensa)
    {
        if (!recompensa.EsRecompensa) 
            throw new Exception("Recompensa invalida");
    }
}