
using CivitaBack.Domain.Enum;

namespace CivitaBack.Domain.Entidades
{
    public class EfectoEvento
    {
        public int Id { get; set; }
        public int EventoMaestroId { get; set; }
        public EventoMaestro? EventoMaestro { get; set; }
        public TipoResultado TipoResultado { get; set; }
        public int EcoCoins { get; set; }
        public int Felicidad { get; set; }
        public int Contaminacion { get; set; }
        public int Energia { get; set; }
        public int Experiencia { get; set; }
    }
}
