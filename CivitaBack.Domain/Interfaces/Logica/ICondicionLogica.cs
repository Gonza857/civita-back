using CivitaBack.Domain.Entidades;

namespace CivitaBack.Domain.Interfaces.Logica
{
    public interface ICondicionLogica
    {
        /// <summary>
        /// Obtiene una Condicion específica por su ID.
        /// </summary>
        /// <param name="id">El ID de la Condicion a buscar.</param>
        /// <returns>La entidad Condicion.</returns>
        /// <exception cref="LogroExcepcion">Si no se encuentra la Condicion.</exception>
        Task<Condicion> ObtenerPorId(int id);

        /// <summary>
        /// Crea una nueva Condicion (no recompensa).
        /// </summary>
        /// <param name="entidad">La entidad Condicion con los datos para crear.</param>
        /// <exception cref="CondicionExcepcion">Si los datos de la condición son inválidos o la Estructura asociada no existe.</exception>
        Task Crear(Condicion entidad);

        /// <summary>
        /// Obtiene un listado de todas las Condiciones (que no son Recompensas).
        /// </summary>
        /// <returns>Una lista de entidades Condicion.</returns>
        Task<List<Condicion>> ObtenerListado();

        /// <summary>
        /// Elimina una Condicion de la base de datos por su ID.
        /// </summary>
        /// <param name="id">El ID de la Condicion a eliminar.</param>
        /// <exception cref="LogroExcepcion">Si el ID es inválido.</exception>
        Task Eliminar(int id);

        /// <summary>
        /// Actualiza una Condicion existente.
        /// </summary>
        /// <param name="condicion">La entidad Condicion con los datos actualizados.</param>
        /// <param name="id">El ID de la Condicion a actualizar.</param>
        /// <exception cref="CondicionExcepcion">Si los datos son inválidos o no se encuentra la Condicion o Recompensa asociada.</exception>
        Task Actualizar(Condicion condicion, int id);

        /// <summary>
        /// Obtiene un listado de todas las Condiciones que SÍ son Recompensas.
        /// </summary>
        /// <returns>Una lista de entidades Condicion marcadas como Recompensa.</returns>
        Task<List<Condicion>> ObtenerListadoRecompensas();

        /// <summary>
        /// Crea una nueva Recompensa (una Condicion marcada como EsRecompensa = true).
        /// </summary>
        /// <param name="recompensa">La entidad Condicion con los datos para crear la recompensa.</param>
        /// <exception cref="CondicionExcepcion">Si los datos de la recompensa son inválidos o la Estructura asociada no existe.</exception>
        Task CrearRecompensa(Condicion recompensa);

        List<Condicion> FiltrarCondicionSiCumple(Condicion condicion, Partida partida);
        public List<Condicion> FiltrarCondicionesSiCumplen(List<Condicion> condiciones, Partida partida);
    }
}