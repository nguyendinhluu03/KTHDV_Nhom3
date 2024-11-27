using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Baithuchanh1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelloWorldController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Hello World");
        }

    }
}
