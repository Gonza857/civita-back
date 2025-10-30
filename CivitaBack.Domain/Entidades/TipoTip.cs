using CivitaBack.Domain.Common;

namespace CivitaBack.Domain.Entities;

public class TipoTip : Auditable
{
    public int Id { get; set; }
    public string Descripcion { get; set; } = string.Empty;
}
