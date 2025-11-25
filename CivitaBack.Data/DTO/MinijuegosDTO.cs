using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using CivitaBack.Domain.Entidades;

namespace CivitaBack.Data.DTO
{
    public class MinijuegosDTO
    {
        public MinijuegosDTO()
        {
            
        }
        
        public int PartidaId { get; set; }
        public string MiniJuegoKey { get; set; }
        public Recurso Recurso { get; set; }
    }
}
