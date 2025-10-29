using CivitaBack.Domain.Common;

namespace CivitaBack.Domain.Entities;

public class Partida : Auditable
{
    public int Id { get; set; }

    public DateTime? UltimaVez { get; set; }
    public string? JsonMapa { get; set; }

    public int UsuarioId { get; set; }
    public Usuario? Usuario { get; set; }

    public Recurso? Recursos { get; set; }
    public List<Evento>? Evento { get; set; }
    public List<Tienda>? Tienda { get; set; }
    public List<EstructuraMapa>? EstructuraMapa { get; set; }
    public List<TipEnPartida>? TipEnPartida { get; set; }

    public List<LogroPartida> LogroPartidas { get; set; } = new();
}
