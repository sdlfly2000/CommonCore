using Microsoft.Extensions.Caching.Memory;

namespace Common.Core.Cache
{
    public interface ICacheService
    {
        object Get(string Code);

        T Get<T>(string Code) where T: class;

        object Set(string Code, object value);

        object Set(string Code, object value, MemoryCacheEntryOptions options);

        void Remove(string Code);
    }
}
