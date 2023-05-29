using Common.Core.AOP;
using Common.Core.AOP.CastleDynamicProxy;
using Common.Core.AspNet.Test.Models;
using Microsoft.Extensions.Caching.Memory;

namespace Common.Core.AspNet.Test.Actions
{
    [AOPInterception(typeof(ICacheTestAction), typeof(CacheTestAction))]
    public class CacheTestAction : ICacheTestAction
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<CacheTestAction> _logger;

        public CacheTestAction(
            IMemoryCache memoryCache,
            ILogger<CacheTestAction> logger)
        {
            _memoryCache = memoryCache;
            _logger = logger;
        }

        [CacheMethod]
        public CachedObject CreateObject(CachedObject cachedObject)
        {
            _logger.LogInformation($"MemoryCache: {_memoryCache.GetHashCode()}");
            return cachedObject;
        }
    }
}
