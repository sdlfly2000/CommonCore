using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;

namespace Common.Core.DependencyInjection
{
    public static class DIModule
    {
        public static void RegisterDomain(IServiceCollection services, string domain)
        {
            var asm = Assembly.Load(domain);

            var impls = asm.GetExportedTypes().Where(t => !t.IsInterface).ToList();

            foreach (var impl in impls)
            {
                var serviceLocateObject = impl.GetCustomAttribute(typeof(ServiceLocateAttribute));
                if (serviceLocateObject != null)
                {
                    var iFace = (serviceLocateObject as ServiceLocateAttribute).IService;
                    services.AddTransient(iFace, impl);
                }
            }
        }
    }
}
