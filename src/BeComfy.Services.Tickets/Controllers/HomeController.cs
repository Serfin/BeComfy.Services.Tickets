using Microsoft.AspNetCore.Mvc;

namespace BeComfy.Services.Tickets.Controllers
{
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
            => Ok("BeComfy Tickets Microservice");
    }
}