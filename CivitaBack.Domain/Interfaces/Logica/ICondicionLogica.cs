using CivitaBack.Domain.Entidades;

namespace CivitaBack.Domain.Interfaces.Logica
{
    public interface ICondicionLogica
    {
        Task<Condicion> ObtenerPorId(int id);
        Task Crear(Condicion entidad);
        Task<List<Condicion>> ObtenerListado();
        Task Eliminar(int id);
        Task Actualizar(Condicion condicion, int id);

        Task<List<Condicion>> ObtenerListadoRecompensas();
        Task CrearRecompensa(Condicion recompensa);

    }
}
