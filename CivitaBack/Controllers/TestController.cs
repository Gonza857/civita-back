using Microsoft.AspNetCore.Mvc;

namespace CivitaBack.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { mensaje = "Todo OK" });
        }
    }
}
