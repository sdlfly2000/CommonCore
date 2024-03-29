using Common.Core.CQRS.Notification;
using Common.Core.CQRS.Request;
using Common.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Core.CQRS
{
    [ServiceLocate(typeof(IEventBus), ServiceType.Scoped)]
    public class EventBus : IEventBus
    {
        private readonly IServiceProvider _serviceProvider;

        public EventBus(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<List<IResponse>> Publish<TNotification, TResponse>(TNotification request)
            where TNotification : INotification
            where TResponse : IResponse
        {
            var responses = new ConcurrentBag<IResponse>();

            var handlers = CQRSNotificationRegister.NotificationHandlers.GetValueOrDefault(request.GetType().Name);

            var services = handlers?.Select(_serviceProvider.GetRequiredService);

            foreach (var handler in handlers)
            {
                var service = _serviceProvider.GetRequiredService(handler) as dynamic;
;
                responses.Add(await service?.Handle(request));
            }

            return responses.ToList();
        }

        public async Task<TResponse> Send<TRequest, TResponse>(TRequest request)
            where TResponse : IResponse
            where TRequest : IRequest
        {
            var requestHandler = _serviceProvider.GetRequiredService<IRequestHandler<TRequest, TResponse>>();

            return await requestHandler.Handle(request);
        }
    }
}
