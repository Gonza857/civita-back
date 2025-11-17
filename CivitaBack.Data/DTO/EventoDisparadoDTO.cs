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
        public string TipoEvento { get; set; } = "PREGUNTA"; 
        public string Titulo { get; set; } = string.Empty;
        public string PreguntaTexto { get; set; } = string.Empty; 
        public string OpcionA_Texto { get; set; } = string.Empty;
        public string OpcionB_Texto { get; set; } = string.Empty;
        public string EfectoAciertoResumen { get; set; } = string.Empty;

    }
}
