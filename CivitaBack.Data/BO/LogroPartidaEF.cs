using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CivitaBack.Data.BO;

public class LogroPartidaEF : AuditableEF
{
    public DateTime FechaCompletado {  get; set; }
    
    public int LogroId { get; set; }
    public LogroEF? Logro { get; set; }

    public int PartidaId { get; set; }
    public PartidaEF? Partida { get; set; }

}
