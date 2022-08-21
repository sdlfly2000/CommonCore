using System.Reflection;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using Common.Core.DependencyInjection;

namespace Common.Core.AOP
{
    [ServiceLocate(typeof(ICacheAction))]
    public class CacheAction: ICacheAction
    {
        private readonly IMemoryCache _memoryCache;

        public CacheAction(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public object BeforeAction(MethodInfo targetMethod, object[] args)
        {
            var reference = args[0] as IReference;
            return _memoryCache.Get(reference.CacheCode);
        }

        public object AfterAction(MethodInfo targetMethod, object[] args)
        {
            var reference = args[0] as IReference;
            var obj = args.Last(); 
            _memoryCache.Set(reference.CacheCode, obj);
            
            return obj;
        }
    }
}