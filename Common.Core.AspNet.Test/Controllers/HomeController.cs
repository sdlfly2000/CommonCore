using Common.Core.AspNet.Test.Actions;
using Microsoft.AspNetCore.Mvc;

namespace Common.Core.AspNet.Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ILogTestAction _logTestAction;

        public HomeController(ILogger<HomeController> logger, ILogTestAction logTestAction)
        {
            _logger = logger;
            _logTestAction = logTestAction;
        }

        [HttpGet("index")]
        public IActionResult Index()
        {
            _logger.LogInformation("Testing inside");
            return new JsonResult(_logTestAction.TestLog());
        }
    }
}
