using Common.Core.AspNet.Test.Actions;
using Common.Core.AspNet.Test.Models;
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
        private readonly IMemoryCache _memoryCache;
        private readonly IPocAction _pocAction;

        public HomeController(
            ILogger<HomeController> logger, 
            ILogTestAction logTestAction, 
            ICacheTestAction cacheTestAction,
            IMemoryCache memoryCache,
            IPocAction pocAction)
        {
            _logger = logger;
            _logTestAction = logTestAction;
            _cacheTestAction = cacheTestAction;
            _memoryCache = memoryCache;
            _pocAction = pocAction;
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
            _logger.LogInformation($"MemoryCache: {_memoryCache.GetHashCode()}");
            _logger.LogInformation("Testing Cached");
            var cacheObject = new CachedObject("1234", "CachedObject");
            _logger.LogInformation($"Before Create: {cacheObject.GetHashCode()}");
            var cachedObject = _cacheTestAction.CreateObject(cacheObject);
            _logger.LogInformation($"After Create: {cachedObject.GetHashCode()}");

            _logger.LogInformation($"MemoryCache: {_memoryCache.GetHashCode()}");

            return Ok();
        }

        [HttpGet("memoryCache")]
        public IActionResult MemoryCache()
        {
            _logger.LogInformation("Testing MemoryCache");
            var cached = _memoryCache.Get("Key");
            
            return Ok();
        }


        [HttpGet("pocAction")]
        public IActionResult PocAction()
        {
            _logger.LogInformation("Testing PocAction");
            _ = _pocAction.GetKey1();
            _ = _pocAction.GetKey2();
            return Ok();
        }

    }
}
