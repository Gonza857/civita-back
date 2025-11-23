using CivitaBack.Domain.Entidades;

namespace CivitaBack.Domain.Interfaces.Logica;

public interface IRecompensaLogica
{
    Task ReclamarRecompensas(List<Recompensa> recompensas, Partida partida);
    
    /// <summary>
    /// Crea una nueva Recompensa (una Condicion marcada como EsRecompensa = true).
    /// </summary>
    /// <param name="recompensa">La entidad Condicion con los datos para crear la recompensa.</param>
    /// <exception cref="CondicionExcepcion">Si los datos de la recompensa son inválidos o la Estructura asociada no existe.</exception>
    Task CrearRecompensa(Recompensa recompensa);

    /// <summary>
    /// Obtiene un listado de todas las recompensas
    /// </summary>
    /// <returns>Una lista de recompensas.</returns>
    Task<List<Recompensa>> Listado();
}