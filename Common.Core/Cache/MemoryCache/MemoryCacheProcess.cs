using Microsoft.Extensions.Caching.Memory;
using Common.Core.DependencyInjection;

namespace Common.Core.Cache.MemoryCache
{
    [ServiceLocate(typeof(IMemoryCacheProcess))]
    public class MemoryCacheProcess : IMemoryCacheProcess
    {
        private readonly IMemoryCache _memoryCache;

        public MemoryCacheProcess(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public object Get(string Code)
        {
            return _memoryCache.Get(Code);
        }

        public object Set(string Code, object value)
        {
            return _memoryCache.Set(Code, value);
        }
    }
}
