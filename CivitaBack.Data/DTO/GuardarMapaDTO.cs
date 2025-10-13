namespace CivitaBack.Data.DTO
{
    public class GuardarMapaDTO
    {
        public int PartidaId { get; set; }
        public string JsonMapa { get; set; } = string.Empty;

        public List<EstructuraEnMapaDTO>? Estructuras { get; set; }
    }

    
}
