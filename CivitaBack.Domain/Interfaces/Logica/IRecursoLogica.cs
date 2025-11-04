using CivitaBack.Domain.Entidades;

namespace CivitaBack.Domain.Interfaces.Logica;

public interface IRecursoLogica
{
    Task ConfigurarInicial(Partida partida);

    Task<Recurso> ObtenerRecursos(int idPartida);
    Task ModificarEnergia(int idPartida, int cantidad);
    
}