using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CivitaBack.Data.DTO
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public string NombreUsuario { get; set; }
        public string Mail { get; set; }

        public int IdUsuario{ get; set; }

        public PartidaDTO? Partida { get; set; }
    }
}
