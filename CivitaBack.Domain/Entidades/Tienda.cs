using CivitaBack.Domain.Common;

namespace CivitaBack.Domain.Entidades;

public class Tienda : Auditable
{
    public int Id { get; set; }

    public int EstructuraId { get; set; }
    public Estructura? Estructura { get; set; }

    public int PartidaId { get; set; }
    public Partida? Partida { get; set; }
}
