using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Baithuchanh2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HelloWorldController : ControllerBase
    {
        [HttpGet("auth")]
        public IActionResult Get()
        {
            return Ok("Hello World");
        }

    }
}
