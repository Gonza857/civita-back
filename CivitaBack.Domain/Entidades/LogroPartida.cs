using CivitaBack.Domain.Common;

namespace CivitaBack.Domain.Entidades;

public class LogroPartida : Auditable
{
    public DateTime FechaCompletado { get; private set; }

    // Relaciones
    public int LogroId { get; set; }
    public Logro? Logro { get; set; }

    public int PartidaId { get; set; }
    public Partida? Partida { get; set; }
}

