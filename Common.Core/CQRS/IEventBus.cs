using Common.Core.CQRS.Request;

namespace Common.Core.CQRS
{
    public interface IEventBus
    {
        IResponse Send<TRequest, TResponse>(TRequest request)
            where TResponse : IResponse
            where TRequest : IRequest;
    }
}
