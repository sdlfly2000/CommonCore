﻿using Common.Core.DependencyInjection;

namespace Common.Core.AspNet.Test.CQRS
{
    [ServiceLocate(typeof(IEventBus), ServiceType.Scoped)]
    public class EventBus : IEventBus
    {
        private readonly IServiceProvider _services;

        public EventBus(IServiceProvider services)
        {
            _services = services;
        }

        public IResponse Send<TRequest, TResponse>(TRequest request) 
            where TResponse : IResponse
            where TRequest: IRequest 
        {
            var requestHandler = _services.GetRequiredService<IRequestHandler<TRequest, TResponse>>();

            return requestHandler.Handle(request);
        }
    }
}
