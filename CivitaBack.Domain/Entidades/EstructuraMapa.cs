using CivitaBack.Domain.Common;

namespace CivitaBack.Domain.Entities;

public class EstructuraMapa : Auditable
{
    public int Id { get; set; }

    public int PartidaId { get; set; }
    public Partida? Partida { get; set; }

    public int EstructuraId { get; set; }
    public Estructura? Estructura { get; set; }

    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
}
