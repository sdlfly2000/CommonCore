using Common.Core.AspNet.Test.Actions;
using Common.Core.AspNet.Test.Models;
using Common.Core.Cache;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

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

        public HomeController(
            ILogger<HomeController> logger, 
            ILogTestAction logTestAction, 
            ICacheTestAction cacheTestAction,
            ICacheService cacheService)
        {
            _logger = logger;
            _logTestAction = logTestAction;
            _cacheTestAction = cacheTestAction;
            _cacheService = cacheService;
        }

        [HttpGet("index")]
        public IActionResult Index()
        {
            _logger.LogInformation("Testing inside");
            return new JsonResult(_logTestAction.TestLog());
        }

        [HttpGet("cached")]
        public IActionResult Cached()
        {
            _logger.LogInformation("Testing Cached");
            var cacheObject = new CachedObject("1234", "CachedObject");

            _logger.LogInformation($"After Create: {cacheObject.GetHashCode()}");

            var cachedObject = _cacheTestAction.CreateObject(cacheObject);

            _logger.LogInformation($"After Create: {cachedObject.GetHashCode()}");

            return Ok();
        }

        [HttpGet("memoryCache")]
        public IActionResult MemoryCache()
        {
            _logger.LogInformation("Testing MemoryCache");
            var cached = _cacheService.Get("CachedObject1234");
            
            return Ok();
        }
    }
}
