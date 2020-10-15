using Common.Core.Cache.MemoryCache;
using Common.Core.Cache.Redis;
using Common.Core.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Common.Core.Cache
{
    [ServiceLocate(typeof(ICacheServiceFactory))]
    public class CacheServiceFactory : ICacheServiceFactory
    {
        private string FeatureToggleIsMemoryCache = "FeatureToggles:IsMemoryCache";

        private readonly IMemoryCacheProcess _memoryCacheProcess;
        private readonly IRedisCacheProcess _redisCacheProcess;
        private readonly IConfiguration _configuration;

        private bool isMemoryCache;

        public CacheServiceFactory(
            IConfiguration configuration,
            IMemoryCacheProcess memoryCacheProcess,
            IRedisCacheProcess redisCacheProcess)
        {
            _memoryCacheProcess = memoryCacheProcess;
            _redisCacheProcess = redisCacheProcess;
            _configuration = configuration;

            isMemoryCache = IsMemoryCache();
        }

        public object Get(string Code)
        {
            return isMemoryCache
                    ? _memoryCacheProcess.Get(Code)
                    : null;
        }

        public object Set(string Code, object value)
        {
            return _memoryCacheProcess.Set(Code, value);
        }

        private bool IsMemoryCache()
        {
            return _configuration.GetValue<bool>(FeatureToggleIsMemoryCache);
        }
    }
}
