using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Common.Core.LogService
{
    public static class LogService
    {
        private static IServiceProvider _serviceProvider;

        public static void SetProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public static void Info<T>(string message) where T : class
        {
            var logger = _serviceProvider.GetService<ILogger<T>>();

            logger.LogInformation(message);
        }
    }
}
