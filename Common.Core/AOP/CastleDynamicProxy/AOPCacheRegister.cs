using Castle.DynamicProxy;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Reflection;

namespace Common.Core.AOP.CastleDynamicProxy
{
    public static class AOPCacheRegister
    {
        public static IServiceCollection RegisterCache(this IServiceCollection services, params string[] domains)
        {
            var asms = domains.Select(domain => Assembly.Load(domain)).ToList();
            var exportedTypes = asms.SelectMany(asm => asm.GetExportedTypes().Where(t => !t.IsInterface).ToList()).ToList();
            
            var caches = exportedTypes.SelectMany(exportedType => exportedType.GetCustomAttributes(typeof(CacheAttribute))).ToList();

            var proxyGenerator = new ProxyGenerator();

            caches.ForEach(cache =>
            {
                var serviceInterface = (cache as CacheAttribute).InterFace;
                var serviceImplement = (cache as CacheAttribute).Implement;

                services.AddTransient(serviceImplement);

                services.AddTransient(serviceInterface, (serviceProvider) =>
                {
                    //var logger = (ILogger)serviceProvider.GetRequiredService(typeof(ILogger<>));
                    var memoryCache = (IMemoryCache)serviceProvider.GetRequiredService(typeof(IMemoryCache));
                    var cacheInvocation = serviceProvider.GetRequiredService(serviceImplement);

                    var cacheProxy = proxyGenerator.CreateInterfaceProxyWithTarget(
                        serviceInterface, 
                        cacheInvocation, 
                        new CacheInterceptor(memoryCache));

                    return cacheProxy;
                });
            });

            return services;
        }
    }
}
