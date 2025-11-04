namespace CivitaBack.Data.BO;

public class MisionPartidaEF : AuditableEF
{
    public DateTime FechaCompletado {  get; set; }
    public DateTime FechaEntrega { get; set; }
    
    public bool Reclamado { get; set; }

    public int Id { get; set; }
    
    public int MisionId { get; set; }
    public MisionEF? Mision { get; set; }

    public int PartidaId { get; set; }
    public PartidaEF? Partida { get; set; }
}