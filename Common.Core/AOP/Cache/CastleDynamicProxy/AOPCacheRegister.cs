using Castle.DynamicProxy;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Reflection;

namespace Common.Core.AOP.CastleDynamicProxy
{
    public static class AOPCacheRegister
    {
        private readonly static ProxyGenerator _proxyGenerator = new ProxyGenerator();

        public static IServiceCollection RegisterCache(this IServiceCollection services, params string[] domains)
        {
            var asms = domains.Select(domain => Assembly.Load(domain)).ToList();
            var exportedTypes = asms.SelectMany(asm => asm.GetExportedTypes().Where(t => !t.IsInterface).ToList()).ToList();
            
            var caches = exportedTypes.SelectMany(exportedType => exportedType.GetCustomAttributes(typeof(CacheAttribute))).ToList();

            caches.ForEach(cache =>
            {
                var serviceInterface = (cache as CacheAttribute).InterFace;
                var serviceImplement = (cache as CacheAttribute).Implement;

                var IsExistImplementInstance = services.IsExistService(serviceInterface, out var serviceDescriptor);

                if (!IsExistImplementInstance) services.AddTransient(serviceImplement);

                var serviceProvider = services.BuildServiceProvider();

                var serviceImplementInstance = IsExistImplementInstance
                ? serviceProvider.GetRequiredService(serviceInterface)
                : serviceProvider.GetRequiredService(serviceImplement);

                if (IsExistImplementInstance) services.Remove(serviceDescriptor);

                AddProxyTransient(services, serviceInterface, serviceImplementInstance);
            });

            return services;
        }

        #region Private Methods

        private static bool IsExistService(this IServiceCollection services, Type serviceType, out ServiceDescriptor serviceDescriptor)
        {
            serviceDescriptor = services.FirstOrDefault(service => service.ServiceType.Equals(serviceType));
            return serviceDescriptor != null;  
        }

        private static IServiceCollection AddProxyTransient(this IServiceCollection services, Type serviceType, object serviceImplementInstance)
        {
            return services.AddTransient(serviceType, (serviceProvider) =>
            {
                var logger = (ILogger<CacheInterceptor>)serviceProvider.GetRequiredService(typeof(ILogger<CacheInterceptor>));
                var memoryCache = (IMemoryCache)serviceProvider.GetRequiredService(typeof(IMemoryCache));

                var cacheProxy = _proxyGenerator.CreateInterfaceProxyWithTarget(
                    serviceType,
                    serviceImplementInstance,
                    new CacheInterceptor(logger, memoryCache));

                return cacheProxy;
            });
        }

        #endregion
    }
}
