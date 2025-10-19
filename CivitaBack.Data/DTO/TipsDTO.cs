using CivitaBack.Data.BO;

namespace CivitaBack.Data.DTO
{
    public class TipDTO
    {
        public int Id { get; set; }
        public string Mensaje { get; set; }
        public string? Expresion { get; set; }
        public string? ElementoAdicional { get; set; }
        public bool EfectoFiltro { get; set; }
        public int TipoId {  get; set; }
        public string? TipoTipDescripcion { get; set; }
        public List<TipEnPartida>? TipEnPartida { get; set; }
    }
}
