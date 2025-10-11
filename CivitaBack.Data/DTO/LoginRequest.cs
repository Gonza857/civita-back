using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CivitaBack.Data.DTO
{
    public class LoginRequest
    {
        public string Mail { get; set; }
        public string Password { get; set; }
    }
}
