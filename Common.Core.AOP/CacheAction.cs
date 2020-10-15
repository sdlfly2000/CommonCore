using System.Reflection;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;
using System.Collections.Generic;

namespace Common.Core.AOP
{
    public class CacheAction<T>: ICacheAction<T> where T : ICacheAspect
    {
        private readonly IMemoryCache _memoryCache;

        public CacheAction(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public object BeforeAction(MethodInfo targetMethod, object[] args)
        {
            return _memoryCache.Get(args[0]);
        }

        public object AfterAction(MethodInfo targetMethod, object[] args)
        {
            var obj = args.Last();

            if(obj is List<T>)
            {
                ((List<T>)obj).Select(o => _memoryCache.Set(o.Code, o));                
            }

            if (obj is T)
            {
                _memoryCache.Set(((T)obj).Code, obj);
            }

            return obj;
        }
    }
}