using CivitaBack.Domain.Common;

namespace CivitaBack.Domain.Entidades;

public class Tienda : Auditable
{
    public int Id { get; set; }

    public string? NombreArticulo { get; set; }
    public string? CodigoArticulo { get; set; }

    public int PartidaId { get; set; }
    public Partida? Partida { get; set; }
}
