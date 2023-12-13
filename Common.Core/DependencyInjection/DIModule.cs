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
                            if (iFace == null) { services.AddScoped(impl);  } else { services.AddScoped(iFace, impl);}
                            break;
                        case ServiceType.Singleton:
                            if (iFace == null) { services.AddSingleton(impl); } else { services.AddSingleton(iFace, impl); }
                            break;
                        default:
                            if (iFace == null) { services.AddTransient(impl); } else { services.AddTransient(iFace, impl); }
                            break;
                    }                    
                }
            }

            return services;
        }
    }
}
