using Castle.DynamicProxy;
using Common.Core.AOP.Cache;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;

namespace Common.Core.AOP.CastleDynamicProxy
{
    public class CacheInterceptor : IInterceptor
    {
        private readonly ILogger<CacheInterceptor> _logger;
        private readonly IMemoryCache _memoryCache;

        public CacheInterceptor(
            ILogger<CacheInterceptor> logger, 
            IMemoryCache memoryCache)
        {
            _logger = logger;
            _memoryCache = memoryCache;
        }

        public void Intercept(IInvocation invocation)
        {
            using (_logger.BeginScope($"In Cache"))
            {
                try
                {
                    if (!invocation.MethodInvocationTarget.IsDefined(typeof(CacheMethodAttribute), false))
                    {
                        return;
                    }

                    var aspectReference = invocation.Arguments[0] as IReference;
                    var aspectInCache = TryGetObjectInCache(aspectReference);

                    if (aspectInCache != null)
                    { 
                        invocation.ReturnValue = aspectInCache;

                        _logger.LogInformation($"Loaded {aspectReference.CacheCode} in Cache");

                        return;
                    }

                    _logger.LogInformation($"{aspectReference.CacheCode} dese not exist in Cache");

                    invocation.Proceed();

                    TrySetObjectInCache(aspectReference, invocation.ReturnValue);
                }
                catch (Exception e)
                {
                    _logger.LogError(exception: e, message: e.Message);
                }
            }
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
