using CivitaBack.Domain.Common;

namespace CivitaBack.Domain.Entidades;

public class Usuario : Auditable
{
    public int Id { get; set; }

    public string? NombreUsuario { get; set; }
    public string? Mail { get; set; }
    public string? HashDeContrasena { get; set; }

    public bool EsDios { get; set; } = false;

    // Relación con Partidas
    public Partida Partida { get; set; }
}
