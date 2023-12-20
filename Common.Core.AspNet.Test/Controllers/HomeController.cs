using Common.Core.AspNet.Test.Actions;
using Common.Core.AspNet.Test.CQRS;
using Common.Core.AspNet.Test.Models;
using Common.Core.Cache;
using Common.Core.CQRS;
using Microsoft.AspNetCore.Mvc;

namespace Common.Core.AspNet.Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ILogTestAction _logTestAction;
        private readonly ICacheTestAction _cacheTestAction;
        private readonly ICacheService _cacheService;
        private readonly ImplementToServiceAction _implAction;
        private readonly IEventBus _eventBus;

        public HomeController(
            ILogger<HomeController> logger, 
            ILogTestAction logTestAction, 
            ICacheTestAction cacheTestAction,
            ICacheService cacheService,
            ImplementToServiceAction implAction,
            IEventBus eventBus)
        {
            _logger = logger;
            _logTestAction = logTestAction;
            _cacheTestAction = cacheTestAction;
            _cacheService = cacheService;
            _implAction = implAction;
            _eventBus = eventBus;
        }

        [HttpGet("index")]
        public IActionResult Index()
        {
            _logger.LogInformation("Testing inside");
            return Ok(_implAction.WhoAmI());
        }

        [HttpGet("cached")]
        public IActionResult Cached()
        {
            _logger.Log(LogLevel.Trace,"Testing Cached");
            var cacheObject = new CachedObject("1234", "CachedObject");

            _logger.LogTrace($"After Create: {cacheObject.GetHashCode()}");

            var cachedObject = _cacheTestAction.CreateObject(cacheObject);

            _logger.LogTrace($"After Create: {cachedObject.GetHashCode()}");

            return Ok();
        }

        [HttpGet("memoryCache")]
        public IActionResult MemoryCache()
        {
            _logger.LogInformation("Testing MemoryCache");
            var cached = _cacheService.Get("CachedObject1234");
            
            return Ok();
        }

        [HttpGet("Log")]
        public IActionResult Log()
        {
            var logRequest = new LogRequest("Test Log");
            var response = _eventBus.Send<LogRequest, LogResponse>(logRequest);
            return Ok();
        }
    }
}
