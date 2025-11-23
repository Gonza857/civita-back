using CivitaBack.Domain.Common;

namespace CivitaBack.Domain.Entidades;

public class Logro : Auditable
{
    public int Id { get; set; }

    public string Titulo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;

    public int TipoLogroId { get; set; }
    public TipoLogro TipoLogro { get; set; }

    public Condicion Condicion { get; set; }
    
    public int CondicionId { get; set; }


    public ICollection<LogroPartida> LogroPartidas { get; set; } = new List<LogroPartida>();
}
