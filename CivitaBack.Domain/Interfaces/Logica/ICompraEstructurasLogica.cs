namespace CivitaBack.Domain.Interfaces.Logica
{
    public interface ICompraEstructurasLogica
    {
        Task<int> ComprarEstructuraAsync(int partidaId, int estructuraId);

    }
}
