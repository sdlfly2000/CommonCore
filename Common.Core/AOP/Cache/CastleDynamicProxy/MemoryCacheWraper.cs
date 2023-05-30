using Microsoft.Extensions.Caching.Memory;
using System;

namespace Common.Core.AOP.Cache.CastleDynamicProxy
{
    public class MemoryCacheWraper : IMemoryCacheWraper
    {
        private static readonly Lazy<IMemoryCache> _memoryCacheLazy;

        static MemoryCacheWraper()
        {
            _memoryCacheLazy = new Lazy<IMemoryCache>(() => new MemoryCache(new MemoryCacheOptions()), isThreadSafe: true);
        }

        public IMemoryCache MemoryCache { get { return _memoryCacheLazy.Value; } }
    }
}
