using CivitaBack.Domain.Entidades;

namespace CivitaBack.Domain.Interfaces.Repositorios;

public interface IMisionRepositorio : IRepositorioBase<Mision>
{
    Task<List<Mision>> ObtenerMisionesDia(int idUsuario);
    Task<List<Mision>> ObtenerMisionesSemana(int idUsuario);
    Task<List<Mision>> ObtenerMisionesMes(int idUsuario);
    Task<List<Mision>> Listado();
    Task<List<Mision>> ListadoActivo();
}