using CivitaBack.Domain.Common;

namespace CivitaBack.Domain.Entities;

public class TipoLogro : Auditable
{
    public int Id { get; set; }

    public string Nombre { get; set; } = string.Empty;
}
