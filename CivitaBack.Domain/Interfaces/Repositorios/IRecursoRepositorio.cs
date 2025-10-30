using CivitaBack.Domain.Entities;

namespace CivitaBack.Domain.Interfaces.Repositorios
{
    public interface IRecursoRepositorio : IRepositorioBase<Recurso>
    {
        Task GuardarRecurso(Recurso recurso);
        Task<Recurso> ObtenerRecursosPartida(int idPartida);
    }
}
