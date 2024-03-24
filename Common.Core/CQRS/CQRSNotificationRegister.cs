using Common.Core.CQRS.Shared;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Common.Core.CQRS
{
    public static class CQRSNotificationRegister
    {
        public static Dictionary<string, List<Type>> NotificationHandlers { get; set; } = new Dictionary<string, List<Type>>();

        public static IServiceCollection RegisterNotifications(this IServiceCollection services, params string[] domains)
        {
            var asms = domains.Select(domain => Assembly.Load(domain)).ToList();

            var impls = asms.SelectMany(asm => asm.GetExportedTypes().Where(t => !t.IsInterface).ToList()).ToList();

            foreach (var impl in impls)
            {
                var cqrs = impl.GetCustomAttribute(typeof(CQRSNotificationHandlerAttribute));
                if (cqrs != null )
                {
                    var notification = (cqrs as CQRSNotificationHandlerAttribute).Notification;
                    var type = (cqrs as CQRSNotificationHandlerAttribute).Type;

                    if (NotificationHandlers.ContainsKey(notification))
                    {
                        NotificationHandlers.GetValueOrDefault(notification)?.Add(type);
                    }
                    else
                    {
                        NotificationHandlers.Add(notification, new List<Type> { type });
                    }
                }
            }

            return services;
        }
    }
}
