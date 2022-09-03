using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;

namespace Common.Core.DependencyInjection
{
    public static class DIModule
    {
        public static IServiceCollection RegisterDomain(this IServiceCollection services, params string[] domains)
        {
            var domainList = domains.Append("Common.Core").ToList();

            var asms = domainList.Select(domain => Assembly.Load(domain)).ToList();

            var impls = asms.SelectMany(asm => asm.GetExportedTypes().Where(t => !t.IsInterface).ToList()).ToList();

            foreach (var impl in impls)
            {
                var serviceLocateObject = impl.GetCustomAttribute(typeof(ServiceLocateAttribute));
                if (serviceLocateObject != null)
                {
                    var iFace = (serviceLocateObject as ServiceLocateAttribute).IService;
                    var serviceType = (serviceLocateObject as ServiceLocateAttribute).ServiceType;
                    switch (serviceType)
                    {
                        case ServiceType.Scoped:
                            services.AddScoped(iFace, impl);
                            break;
                        case ServiceType.Singleton:
                            services.AddSingleton(iFace, impl);
                            break;
                        default:
                            services.AddTransient(iFace, impl);
                            break;
                    }                    
                }
            }

            return services;
        }
    }
}
