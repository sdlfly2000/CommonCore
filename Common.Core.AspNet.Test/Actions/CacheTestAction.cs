using Common.Core.AOP;
using Common.Core.AOP.CastleDynamicProxy;
using Common.Core.AOP.Log;
using Common.Core.AspNet.Test.Models;

namespace Common.Core.AspNet.Test.Actions
{
    [AOPInterception(typeof(ICacheTestAction), typeof(CacheTestAction))]
    public class CacheTestAction : ICacheTestAction
    {
        private readonly ILogger<CacheTestAction> _logger;

        public CacheTestAction(
            ILogger<CacheTestAction> logger)
        {
            _logger = logger;
        }

        [CacheMethod]
        [LogTrace(activityName:"CreateObjectActivity")]
        public CachedObject CreateObject(CachedObject cachedObject)
        {
            return cachedObject;
        }
    }
}
