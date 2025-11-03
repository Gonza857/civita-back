using CivitaBack.Domain.Entidades;

namespace CivitaBack.Domain.Interfaces.Logica
{
    public interface IEstructuraLogica
    {
        Task<Estructura> ObtenerPorId(int idEstructura);
        Task<List<Estructura>> ObtenerListado();
        Task Crear(Estructura estructura);
        Task Eliminar(int idEstructura);
        Task Actualizar(Estructura estructura, int id);
    }
}
