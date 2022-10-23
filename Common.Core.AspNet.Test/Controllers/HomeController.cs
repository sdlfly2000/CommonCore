using Common.Core.AOP;
using Common.Core.AOP.Log;
using Microsoft.AspNetCore.Mvc;

namespace Common.Core.AspNet.Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AOPInterception(typeof(IHomeController), typeof(HomeController))]
    public class HomeController : ControllerBase, IHomeController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet("index")]
        [LogTrace]
        public IActionResult Index()
        {
            _logger.LogInformation("Testing");
            return new JsonResult("Hello World");
        }
    }
}
