using Microsoft.Extensions.Caching.Memory;

namespace Common.Core.AspNet.Test.Common
{
    public interface IMemoryCacheWraper
    {
        IMemoryCache MemoryCache { get; }
    }
}
