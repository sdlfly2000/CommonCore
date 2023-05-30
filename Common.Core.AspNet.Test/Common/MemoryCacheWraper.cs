using Microsoft.Extensions.Caching.Memory;

namespace Common.Core.AspNet.Test.Common
{
    public class MemoryCacheWraper : IMemoryCacheWraper
    {
        private static readonly IMemoryCache _memoryCache;

        static MemoryCacheWraper()
        {
            _memoryCache ??= new MemoryCache(new MemoryCacheOptions());
        }

        public IMemoryCache MemoryCache { get { return _memoryCache; } }
    }
}
