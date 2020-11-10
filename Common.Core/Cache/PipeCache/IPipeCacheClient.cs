using System.Collections.Generic;

namespace Common.Core.Cache.PipeCache
{
    public interface IPipeCacheClient<T> where T: class
    {
        T Get(string key);

        T Set(string key, T value);

        void Remove(string key);

        IList<string> GetAllKeys();
    }
}
