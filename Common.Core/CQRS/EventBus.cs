using Common.Core.CQRS.Notification;
using Common.Core.CQRS.Request;
using Common.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Common.Core.CQRS
{
    [ServiceLocate(typeof(IEventBus), ServiceType.Scoped)]
    public class EventBus : IEventBus
    {
        private readonly IServiceProvider _services;

        public EventBus(IServiceProvider services)
        {
            _services = services;
        }

        public List<IResponse> Publish<TRequest, TResponse>(TRequest request)
            where TRequest : INotification
            where TResponse : IResponse
        {
            var responses = new List<IResponse>();

            var notificationHandlers = _services.GetServices<INotificationHandler<TRequest, TResponse>>();

            foreach (var handler in notificationHandlers)
            {
                responses.Add(handler.Handle(request));
            }

            return responses;
        }

        public IResponse Send<TRequest, TResponse>(TRequest request)
            where TResponse : IResponse
            where TRequest : IRequest
        {
            var requestHandler = _services.GetRequiredService<IRequestHandler<TRequest, TResponse>>();

            return requestHandler.Handle(request);
        }
    }
}
