using CivitaBack.Domain.Entities;

namespace CivitaBack.Domain.Interfaces.Repositorios
{
    public interface ILogroRepositorio : IRepositorioBase<Logro>
    {
        Task<bool> ExisteLogroEnCumplidos(int idLogro);
    }
}
