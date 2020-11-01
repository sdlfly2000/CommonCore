using Common.Core.Cache.MemoryCache;
using Common.Core.Cache.Redis;
using Common.Core.DependencyInjection;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Common.Core.Cache
{
    [ServiceLocate(typeof(ICacheService))]
    public class CacheService : ICacheService
    {
        private string FeatureToggleIsMemoryCache = "FeatureToggles:IsMemoryCache";

        private readonly IMemoryCacheProcess _memoryCacheProcess;
        private readonly IRedisCacheProcess _redisCacheProcess;
        private readonly IConfiguration _configuration;

        private bool isMemoryCache;

        public CacheService(
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

        public T Get<T>(string Code) where T: class
        {
            return isMemoryCache
                    ? _memoryCacheProcess.Get<T>(Code)
                    : null;
        }

        public object Set(string Code, object value)
        {
            return isMemoryCache
                ? _memoryCacheProcess.Set(Code, value)
                : null;
        }

        public object Set(string Code, object value, MemoryCacheEntryOptions options)
        {
            return isMemoryCache
                ? _memoryCacheProcess.Set(Code, value, options)
                : null;
        }

        public T Set<T>(string Code, T value) where T:class
        {
            return isMemoryCache
                ? _memoryCacheProcess.Set<T>(Code, value)
                : null;
        }

        public T Set<T>(string Code, T value, MemoryCacheEntryOptions options) where T: class
        {
            return isMemoryCache
                ? _memoryCacheProcess.Set<T>(Code, value, options)
                : null;
        }

        public void Remove(string Code)
        {
            if (isMemoryCache) {
                _memoryCacheProcess.Remove(Code);
            }
        }

        public IList<string> LoadAllKeys()
        {
            return isMemoryCache
                ? _memoryCacheProcess.LoadAllKeys()
                : null;
        }    

        #region

        private bool IsMemoryCache()
        {
            return bool.Parse(_configuration[FeatureToggleIsMemoryCache]);
        }

        #endregion
    }
}
