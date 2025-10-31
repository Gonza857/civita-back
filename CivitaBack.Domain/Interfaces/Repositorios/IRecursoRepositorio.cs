using CivitaBack.Domain.Entidades;

namespace CivitaBack.Domain.Interfaces.Repositorios
{
    public interface IRecursoRepositorio : IRepositorioBase<Recurso>
    {
        Task<Recurso> ObtenerRecursosPartida(int idPartida);
    }
}
