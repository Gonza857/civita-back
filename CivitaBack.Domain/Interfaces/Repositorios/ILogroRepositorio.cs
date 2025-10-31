using CivitaBack.Domain.Entidades;

namespace CivitaBack.Domain.Interfaces.Repositorios
{
    public interface ILogroRepositorio : IRepositorioBase<Logro>
    {
        Task<bool> ExisteLogroEnCumplidos(int idLogro);
        Task<Logro?> ObtenerPorId(int id);
    }
}
