using Microsoft.Extensions.Caching.Memory;
using Common.Core.DependencyInjection;
using System.Collections.Generic;
using System.Reflection;
using System.Collections;

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

        public T Set<T>(string Code, T value, MemoryCacheEntryOptions options) where T:class
        {
            return _memoryCache.Set<T>(Code, value, options);
        }

        public T Set<T>(string Code, T value) where T : class
        {
            return _memoryCache.Set<T>(Code, value);
        }

        public void Remove(string Code)
        {
            _memoryCache.Remove(Code);
        }

        public IList<string> LoadAllKeys()
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            var entries = _memoryCache.GetType().GetField("_entries", flags).GetValue(_memoryCache);
            var cacheItems = entries as IDictionary;
            var keys = new List<string>();
            if (cacheItems == null) return keys;
            foreach (DictionaryEntry cacheItem in cacheItems)
            {
                keys.Add(cacheItem.Key.ToString());
            }
            return keys;
        }
    }
}
