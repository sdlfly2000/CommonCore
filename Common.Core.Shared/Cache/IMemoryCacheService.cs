namespace Common.Core.Shared.Cache
{
    public interface IMemoryCacheService
    {
        Task<bool> InsertIfNotExist<T>(string cacheKeyUnique, T jsonValue, TimeSpan expire, CancellationToken? token);

        Task<bool> Upsert<T>(string cacheKeyUnique, T jsonValue, TimeSpan expire, CancellationToken? token);

        Task<(bool Success, T? CachedValue)> GetValue<T>(string cacheKeyUnique, CancellationToken? token);
    }
}
