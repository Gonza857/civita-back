using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CivitaBack.Data.DTO
{
    public class LoginDTO
    {
        public string Token { get; set; }
        public string NombreUsuario { get; set; }
        public string Mail { get; set; }

        public int IdUsuario{ get; set; }
        public int IdPartida{ get; set; }

    }
}
