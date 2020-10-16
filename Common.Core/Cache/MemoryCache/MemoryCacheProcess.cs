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

        public T Get<T>(string Code) where T : class
        {
            return _memoryCache.Get<T>(Code);
        }

        public object Set(string Code, object value)
        {
            return _memoryCache.Set(Code, value);
        }

        public object Set(string Code, object value, MemoryCacheEntryOptions options)
        {
            return _memoryCache.Set(Code, value, options);
        }

        public void Remove(string Code)
        {
            _memoryCache.Remove(Code);
        }
    }
}
