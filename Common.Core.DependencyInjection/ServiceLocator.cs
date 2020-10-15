using System;

namespace Common.Core.DependencyInjection
{
    public class ServiceLocator
    {
        public static IServiceProvider ServiceProvider { get; set; }

        public static void SetServices(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }
    }
}
