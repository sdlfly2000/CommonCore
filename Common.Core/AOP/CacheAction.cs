using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;

namespace Common.Core.AOP
{
    public class CacheAction<TAspect, TReference>: ICacheAction<TAspect, TReference> where TReference : IReference
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

            if (obj is TAspect)
            {
                _memoryCache.Set(reference.CacheCode, obj);
            }

            return obj;
        }
    }
}