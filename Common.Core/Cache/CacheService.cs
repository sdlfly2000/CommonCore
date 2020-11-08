using Common.Core.Cache.MemoryCache;
using Common.Core.DependencyInjection;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;

namespace Common.Core.Cache
{
    [ServiceLocate(typeof(ICacheService))]
    public class CacheService : ICacheService
    {
        private readonly IMemoryCacheProcess _memoryCacheProcess;

        public CacheService(
            IMemoryCacheProcess memoryCacheProcess)
        {
            _memoryCacheProcess = memoryCacheProcess;
        }

        public object Get(string Code)
        {
            return _memoryCacheProcess.Get(Code);
        }

        public T Get<T>(string Code) where T: class
        {
            return _memoryCacheProcess.Get<T>(Code);
        }

        public object Set(string Code, object value)
        {
            return _memoryCacheProcess.Set(Code, value);
        }

        public object Set(string Code, object value, MemoryCacheEntryOptions options)
        {
            return _memoryCacheProcess.Set(Code, value, options);
        }

        public T Set<T>(string Code, T value) where T:class
        {
            return _memoryCacheProcess.Set<T>(Code, value);
        }

        public T Set<T>(string Code, T value, MemoryCacheEntryOptions options) where T: class
        {
            return _memoryCacheProcess.Set<T>(Code, value, options);
        }

        public void Remove(string Code)
        {
            _memoryCacheProcess.Remove(Code);            
        }

        public IList<string> LoadAllKeys()
        {
            return _memoryCacheProcess.LoadAllKeys();
        }    
    }
}
