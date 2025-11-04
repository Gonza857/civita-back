using CivitaBack.Domain.Entidades;

namespace CivitaBack.Domain.Interfaces.Logica;

public interface IInicialLogica
{
    Task IniciarPartida(Usuario usuario);
}