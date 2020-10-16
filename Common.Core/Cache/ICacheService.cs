using Microsoft.Extensions.Caching.Memory;

namespace Common.Core.Cache
{
    public interface ICacheService
    {
        object Get(string Code);

        T Get<T>(string Code) where T: class;

        object Set(string Code, object value);

        T Set<T>(string Code, T value) where T : class;

        object Set(string Code, object value, MemoryCacheEntryOptions options);

        T Set<T>(string Code, T value, MemoryCacheEntryOptions options) where T : class;

        void Remove(string Code);
    }
}
