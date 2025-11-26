using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CivitaBack.Data.DTO
{
    public class EventoResueltoDTO
    {
        public int Id { get; set; }
        public string TextoRespuesta { get; set; } = string.Empty;
        public string RecursosAplicados { get; set; } = string.Empty;
        public int PartidaId { get; set; }
        public RecursoDTO? RecursosActualizados { get; set; }
    }
}
