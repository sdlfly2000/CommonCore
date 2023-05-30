using Microsoft.Extensions.Caching.Memory;

namespace Common.Core.AOP.Cache.CastleDynamicProxy
{
    public interface IMemoryCacheWraper
    {
        IMemoryCache MemoryCache { get; }
    }
}
