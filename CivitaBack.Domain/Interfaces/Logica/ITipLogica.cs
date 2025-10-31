using CivitaBack.Domain.Entidades;

namespace CivitaBack.Domain.Interfaces.Logica;

public interface ITipLogica
{
    Task<List<Tip>> ObtenerMsjPorIdTipo(int id);
    Task<List<Tip>> Listado();
    Task Crear(Tip tip);
    Task Actualizar(Tip tip, int id);

    Task<Tip?> ObtenerPorIdTipo(int id);
}