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
        private readonly IServiceProvider _services;

        public EventBus(IServiceProvider services)
        {
            _services = services;
        }

        public async Task<List<IResponse>> Publish<TRequest, TResponse>(TRequest request)
            where TRequest : INotification
            where TResponse : IResponse
        {
            var responses = new ConcurrentBag<IResponse>();

            var notificationHandlers = _services.GetServices<INotificationHandler<TRequest, TResponse>>();

            Parallel.ForEach(notificationHandlers, (handler) => responses.Add(handler.Handle(request)));

            return await Task.FromResult(responses.ToList());
        }

        public async Task<IResponse> Send<TRequest, TResponse>(TRequest request)
            where TResponse : IResponse
            where TRequest : IRequest
        {
            var requestHandler = _services.GetRequiredService<IRequestHandler<TRequest, TResponse>>();

            return await Task.FromResult(requestHandler.Handle(request));
        }
    }
}
