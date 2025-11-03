using CivitaBack.Domain.Entidades;

namespace CivitaBack.Domain.Interfaces.Logica;

public interface IRecompensaLogica
{
    Task ReclamarRecompensas(List<Condicion> recompensas, Partida partida);
    Task ReclamarRecompensa (Condicion recompensa, Partida partida);
}