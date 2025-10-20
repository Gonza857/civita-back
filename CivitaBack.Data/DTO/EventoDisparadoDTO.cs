using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CivitaBack.Data.DTO
{
    public class EventoDisparadoDTO
    {
        public int Id { get; set; }
        public string TextoDescripcion { get; set; } = string.Empty;
        public int EcoCoinsAceptar { get; set; }
        public int FelicidadAceptar { get; set; }
        public int ContaminacionAceptar { get; set; }
        public int FelicidadRechazar { get; set; }
        public int ContaminacionRechazar { get; set; }
        
    }
}
