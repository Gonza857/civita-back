using CivitaBack.Domain.Entidades;

namespace CivitaBack.Domain.Interfaces.Repositorios;

public interface IMisionRepositorio : IRepositorioBase<Mision>
{
    Task<List<Mision>> Listado();
    Task<List<Mision>> ListadoActivo();
    
}