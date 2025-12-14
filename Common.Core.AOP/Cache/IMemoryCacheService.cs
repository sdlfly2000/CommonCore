using System;
using System.Threading.Tasks;

namespace Common.Core.AOP.Cache
{
    public interface IMemoryCacheService
    {
        Task Set<T>(string cacheKeyUnique, T jsonValue, TimeSpan expire);

        Task<bool> TryGetValue<T>(string cacheKeyUnique, out T cachedValue);
    }
}
