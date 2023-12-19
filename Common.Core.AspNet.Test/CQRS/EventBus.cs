using Common.Core.DependencyInjection;

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
            if (!RegisterCQRS.RequestHandlerMapper.TryGetValue(request.GetType(), out var requestHandlerType))
            {
                throw new InvalidOperationException($"Request Handler does not exist for Request {request.GetType().Name}.");
            }

            var requestHandler = (IRequestHandler<TRequest, TResponse>)_services.GetRequiredService(requestHandlerType);

            return requestHandler.Handle(request);

        }
    }
}
