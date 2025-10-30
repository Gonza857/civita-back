using CivitaBack.Domain.Common;

namespace CivitaBack.Domain.Entities;

public class TipEnPartida : Auditable
{
    public int Id { get; set; }

    public int PartidaId { get; set; }
    public Partida? Partida { get; set; }

    public int TipId { get; set; }
    public Tip? Tip { get; set; }
}
