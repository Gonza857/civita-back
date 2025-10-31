using CivitaBack.Domain.Common;

namespace CivitaBack.Domain.Entidades;

public class Tip : Auditable
{
    public int Id { get; set; }
    public string Mensaje { get; set; } = string.Empty;
    public string? Expresion { get; set; }
    public string? ElementoAdicional { get; set; }
    public bool EfectoFiltro { get; set; }

    public int TipoId { get; set; }
    public TipoTip TipoTip { get; set; }

    public List<TipEnPartida>? TipEnPartida { get; set; }
}
