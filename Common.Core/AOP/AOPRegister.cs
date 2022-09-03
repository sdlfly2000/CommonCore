using Castle.DynamicProxy;
using Common.Core.AOP.Cache.CastleDynamicProxy;
using Common.Core.AOP.Log;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Common.Core.AOP
{
    public static class AOPRegister
    {
        private readonly static ProxyGenerator _proxyGenerator = new ProxyGenerator();

        public static IServiceCollection RegisterInterceptor(this IServiceCollection services, bool enableLog, bool enableCache, Type[]? interceptors, params string[] domains)
        {
            var interceptorList = interceptors == null ? new List<Type>(): interceptors.ToList();

            if (enableCache)
            {
                interceptorList.Add(typeof(ICacheIntercptor));
            }

            if (enableLog)
            {
                interceptorList.Add(typeof(ILogInterceptor));
            }

            var asms = domains.Select(domain => Assembly.Load(domain)).ToList();
            var exportedTypes = asms.SelectMany(asm => asm.GetExportedTypes().Where(t => !t.IsInterface).ToList()).ToList();

            var interceptions = exportedTypes.SelectMany(exportedType => exportedType.GetCustomAttributes(typeof(AOPInterceptionAttribute))).ToList();

            interceptions.ForEach(cache =>
            {
                var serviceInterface = (cache as AOPInterceptionAttribute).InterFace;
                var serviceImplement = (cache as AOPInterceptionAttribute).Implement;

                var IsExistImplementInstance = services.IsExistService(serviceInterface, out var serviceDescriptor);

                if (!IsExistImplementInstance) services.AddTransient(serviceImplement);

                var serviceProvider = services.BuildServiceProvider();

                var serviceImplementInstance = IsExistImplementInstance
                ? serviceProvider.GetRequiredService(serviceInterface)
                : serviceProvider.GetRequiredService(serviceImplement);

                if (IsExistImplementInstance) services.Remove(serviceDescriptor);

                var interceptorInstances = interceptorList.Select(interceptor => serviceProvider.GetRequiredService(interceptor) as IInterceptor).ToArray();

                AddProxyTransient(services, serviceInterface, serviceImplementInstance, interceptorInstances);
            });

            return services;
        }

        #region Private Methods

        private static bool IsExistService(this IServiceCollection services, Type serviceType, out ServiceDescriptor serviceDescriptor)
        {
            serviceDescriptor = services.FirstOrDefault(service => service.ServiceType.Equals(serviceType));
            return serviceDescriptor != null;
        }

        private static IServiceCollection AddProxyTransient(this IServiceCollection services, Type serviceType, object serviceImplementInstance, IInterceptor[] interceptors)
        {
            return services.AddTransient(serviceType, (serviceProvider) =>
            {
                    return _proxyGenerator.CreateInterfaceProxyWithTarget(
                    serviceType,
                    serviceImplementInstance,
                    interceptors);
            });
        }

        #endregion
    }
}
