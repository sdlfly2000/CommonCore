﻿using Microsoft.Extensions.Caching.Memory;

namespace Common.Core.Cache.MemoryCache
{
    public interface IMemoryCacheProcess
    {
        object Get(string Code);

        T Get<T>(string Code) where T: class;

        object Set(string Code, object value);

        void Remove(string Code);

        object Set(string Code, object value, MemoryCacheEntryOptions options);
    }
}
