using Common.Core.LogService;
using Microsoft.AspNetCore.Mvc;

namespace Common.Core.AspNet.Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ILogger _logger;

        public HomeController(ILogger logger)
        {
            _logger = logger;
        }

        [HttpGet("index")]
        public IActionResult Index()
        {
            _logger.LogInformation("Testing");
            return new JsonResult("Hello World");
        }
    }
}
