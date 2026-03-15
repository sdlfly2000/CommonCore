using Common.Core.Shared.Cache;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Common.Core.ConsoleApp.Test
{
    public class MemoryCacheService : IMemoryCacheService
    {
        private readonly IDistributedCache _redisCache;

        public MemoryCacheService(IDistributedCache redisCache)
        {
            _redisCache = redisCache;
        }

        public async Task<(bool Success, T? CachedValue)> GetValue<T>(string cacheKeyUnique, CancellationToken? token)
        {
            var value = await _redisCache.GetStringAsync(cacheKeyUnique, token ?? CancellationToken.None).ConfigureAwait(false);

            if (value == null) return (false, default);

            var cachedValue = JsonSerializer.Deserialize<T>(value);

            if (cachedValue == null) return (false, default);

            return (true, cachedValue);
        }

        public Task<bool> InsertIfNotExist<T>(string cacheKeyUnique, T jsonValue, TimeSpan expire, CancellationToken? token)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Upsert<T>(string cacheKeyUnique, T jsonValue, TimeSpan expire, CancellationToken? token)
        {
            await _redisCache.SetStringAsync(cacheKeyUnique, JsonSerializer.Serialize(jsonValue), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expire
            }, token ?? CancellationToken.None).ConfigureAwait(false);

            return true;
        }
    }
}
