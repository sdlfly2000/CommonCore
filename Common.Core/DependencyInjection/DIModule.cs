using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Common.Core.DependencyInjection
{
    public static class DIModule
    {
        public static void RegisterDomain(this IServiceCollection services, IList<string> domains)
        {
            domains.Add("Common.Core");

            var asms = domains.Select(domain => Assembly.Load(domain)).ToList();

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
        }
    }
}
