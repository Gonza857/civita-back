using CivitaBack.Domain.Common;

namespace CivitaBack.Domain.Entities;

public class Logro : Auditable
{
    public int Id { get; set; }

    public string Titulo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;

    public TipoLogro TipoLogro { get; set; }

    public Condicion Condicion { get; set; }

    public ICollection<LogroPartida> LogroPartidas { get; set; } = new List<LogroPartida>();
}
