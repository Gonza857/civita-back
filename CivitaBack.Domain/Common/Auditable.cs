namespace CivitaBack.Domain.Common;

public class Auditable
{
    public DateTime Creado { get; set; } = DateTime.UtcNow;
    public DateTime? Editado { get; set; }
}