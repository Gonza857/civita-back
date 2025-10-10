using CivitaBack.Logica;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CivitaBack.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecursosController : ControllerBase
    {

        private readonly IRecursoLogica _recursoLogica;
        public RecursosController(IRecursoLogica rl)
        {
            this._recursoLogica = rl;
        }




    }
}
