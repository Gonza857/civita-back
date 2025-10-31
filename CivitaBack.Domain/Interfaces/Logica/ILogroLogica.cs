using CivitaBack.Domain.Entidades;

namespace CivitaBack.Domain.Interfaces.Logica
{
    public interface ILogroLogica
    {
        Task<Logro> ObtenerPorId(int Id);
        Task Crear(Logro entidad);
        Task<List<Logro>> ObtenerListado();
        Task<List<Logro>> ObtenerListadoInterno();
        Task Eliminar(int Id);
        Task Actualizar(Logro logro, int id);
        Task<List<Logro>> ObtenerLogrosCumplidos(Partida partida);
        Task<List<Logro>> ComprobarSiCumpleAlgunLogro(Partida? partida, List<Logro> logrosDB);
        Task MarcarLogrosComoCompletados(Partida? partida, List<Logro> logros);

        Task<List<Logro>> ObtenerLogrosParaObtenerRecompensa(int partidaId);
    }
}
