using Common.Core.DependencyInjection;
using System.Collections.Generic;
using System.Reflection;
using System.Collections;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace Common.Core.Cache.LocalMemoryCache
{
    [ServiceLocate(typeof(IMemoryCacheProcess))]
    public class MemoryCacheProcess : IMemoryCacheProcess
    {
        private static readonly Lazy<IMemoryCache> _memoryCacheLazy;

        static MemoryCacheProcess()
        {
            _memoryCacheLazy = new Lazy<IMemoryCache>(() => new MemoryCache(new MemoryCacheOptions()), isThreadSafe: true);
        }

        public object Get(string Code)
        {
            return _memoryCacheLazy.Value.Get(Code);
        }

        public T Get<T>(string Code) where T : class
        {
            return _memoryCacheLazy.Value.Get<T>(Code);
        }

        public object Set(string Code, object value)
        {
            return _memoryCacheLazy.Value.Set(Code, value);
        }

        public object Set(string Code, object value, MemoryCacheEntryOptions options)
        {
            return _memoryCacheLazy.Value.Set(Code, value, options);
        }

        public T Set<T>(string Code, T value, MemoryCacheEntryOptions options) where T : class
        {
            return _memoryCacheLazy.Value.Set(Code, value, options);
        }

        public T Set<T>(string Code, T value) where T : class
        {
            return _memoryCacheLazy.Value.Set(Code, value);
        }

        public void Remove(string Code)
        {
            _memoryCacheLazy.Value.Remove(Code);
        }

        public IList<string> LoadAllKeys()
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            var entries = _memoryCacheLazy.Value.GetType().GetField("_entries", flags).GetValue(_memoryCacheLazy.Value);
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
