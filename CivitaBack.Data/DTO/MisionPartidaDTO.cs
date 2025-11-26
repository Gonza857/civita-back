namespace CivitaBack.Data.DTO;

public class MisionPartidaDTO
{
    public int Id { get; set; }
    public string Titulo { get; set; }
    public string Descripcion { get; set; }
    public bool Reclamada { get; set; }
    public bool PuedeReclamar { get; set; }
    
    public string Tipo { get; set; }
    
}