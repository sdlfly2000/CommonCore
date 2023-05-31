using Castle.DynamicProxy;
using Common.Core.AOP.Cache;
using Common.Core.AOP.Cache.CastleDynamicProxy;
using Common.Core.Cache;
using Common.Core.DependencyInjection;
using System;

namespace Common.Core.AOP.CastleDynamicProxy
{
    [ServiceLocate(typeof(ICacheInterceptor))]
    public class CacheInterceptor : ICacheInterceptor
    {

        private readonly ICacheService _cacheService;

        public CacheInterceptor(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        public void Intercept(IInvocation invocation)
        {
            try
            {
                if (!invocation.MethodInvocationTarget.IsDefined(typeof(CacheMethodAttribute), false))
                {
                    invocation.Proceed();
                    return;
                }

                var aspectReference = invocation.Arguments[0] as IReference;
                var aspectInCache = TryGetObjectInCache(aspectReference);

                if (aspectInCache != null)
                {
                    invocation.ReturnValue = aspectInCache;

                    //_logger.LogInformation($"Loaded {aspectReference.CacheCode} in Cache");

                    return;
                }

                //_logger.LogInformation($"{aspectReference.CacheCode} dese not exist in Cache");

                invocation.Proceed();

                TrySetObjectInCache(aspectReference, invocation.ReturnValue);
            }
            catch (Exception e)
            {
                //_logger.LogError(exception: e, message: e.Message);
            }
            
        }

        #region Private Methods

        private object? TryGetObjectInCache(IReference reference)
        {
            //_logger.LogInformation($"MemoryCache: {_memoryCache.GetHashCode()}");
            return _cacheService.Get(reference.CacheCode);
        }

        private void TrySetObjectInCache(IReference reference, object aspectToCache)
        {
            //_logger.LogInformation($"MemoryCache: {_memoryCache.GetHashCode()}");
            if (aspectToCache != null) _cacheService.Set(reference.CacheCode, aspectToCache);
        }

        #endregion
    }
}
