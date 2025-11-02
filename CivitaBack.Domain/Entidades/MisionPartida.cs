namespace CivitaBack.Domain.Entidades;

public class MisionPartida
{
    public int Id { get; set; }

    public DateTime? FechaCompletado { get; set; }
    public DateTime FechaEntrega { get; set; }
    
    public bool Reclamado { get; set; }

    // Relaciones
    public int MisionId { get; set; }
    public Mision? Mision { get; set; }

    public int PartidaId { get; set; }
    public Partida? Partida { get; set; }
}