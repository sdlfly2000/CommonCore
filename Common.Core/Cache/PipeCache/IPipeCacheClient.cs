using System.Collections.Generic;

namespace Common.Core.Cache.PipeCache
{
    public interface IPipeCacheClient
    {
        T Get<T>(string key) where T: class;

        void Set<T>(string key, T value) where T : class;

        void Remove(string key);

        IList<string> GetAllKeys();
    }
}
