using CivitaBack.Domain.Entities;

namespace CivitaBack.Domain.Interfaces.Repositorios
{
    public interface ILogroRepositorio
    {
        Task<bool> ExisteLogroEnCumplidos(int idLogro);
    }
}
