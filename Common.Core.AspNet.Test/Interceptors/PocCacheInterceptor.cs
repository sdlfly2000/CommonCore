using Castle.DynamicProxy;
using Common.Core.AspNet.Test.Common;
using Common.Core.DependencyInjection;

namespace Common.Core.AspNet.Test.Interceptors
{
    [ServiceLocate(typeof(IPocCacheInterceptor))]
    public class PocCacheInterceptor : IPocCacheInterceptor
    {
        public static readonly IMemoryCacheWraper _memoryCacheWraper;

        static PocCacheInterceptor()
        {
            _memoryCacheWraper ??= new MemoryCacheWraper();
            Console.WriteLine($"MemoryCache in Ctor: {_memoryCacheWraper.MemoryCache.GetHashCode()}");
        }

        public void Intercept(IInvocation invocation)
        {
            Console.WriteLine($"MemoryCache: {_memoryCacheWraper.MemoryCache.GetHashCode()}");
        }
    }
}
