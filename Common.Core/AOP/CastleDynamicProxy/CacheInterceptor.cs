using Castle.DynamicProxy;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;

namespace Common.Core.AOP.CastleDynamicProxy
{
    public class CacheInterceptor : IInterceptor
    {
        //private readonly ILogger _logger;
        private readonly IMemoryCache _memoryCache;

        public CacheInterceptor(
            //ILogger logger, 
            IMemoryCache memoryCache)
        {
            //_logger = logger;
            _memoryCache = memoryCache;
        }

        public void Intercept(IInvocation invocation)
        {
            Console.WriteLine("In Intercept");
            if(!invocation.MethodInvocationTarget.IsDefined(typeof(CacheMethodAttribute), false))
            {
                return;
            }

            var aspectReference = invocation.Arguments[0] as IReference;
            var aspectInCache = TryGetObjectInCache(aspectReference);

            if (aspectInCache != null)
            {
                invocation.ReturnValue = aspectInCache;
                return;
            }

            invocation.Proceed();

            TrySetObjectInCache(aspectReference, invocation.ReturnValue);
        }

        #region Private Methods

        private object TryGetObjectInCache(IReference reference)
        {
            return _memoryCache.Get(reference.CacheCode);
        }

        private void TrySetObjectInCache(IReference reference, object aspectToCache)
        {
            if (aspectToCache != null) _memoryCache.Set(reference.CacheCode, aspectToCache);
        }

        #endregion
    }
}
