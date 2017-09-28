using Microsoft.AspNetCore.Mvc;

namespace sample.Health
{
    [Route("health")]
    public class HealthController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}
