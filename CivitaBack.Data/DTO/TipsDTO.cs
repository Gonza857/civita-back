using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CivitaBack.Data.BO;

namespace CivitaBack.Data.DTO
{
    public class TipsDTO
    {
        public int Id { get; set; }

        public string Mensaje { get; set; }

        public string? Expresion { get; set; }

        public string? ElementoAdicional { get; set; }
        public bool EfectoFiltro { get; set; }
        public int TipoId {  get; set; }
        public TipoTipDTO? TipoTip { get; set; }

        public List<TipEnPartida>? TipEnPartida { get; set; }
    }
}
