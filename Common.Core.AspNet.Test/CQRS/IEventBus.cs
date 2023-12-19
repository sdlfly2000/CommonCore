namespace Common.Core.AspNet.Test.CQRS
{
    public interface IEventBus
    {
        IResponse Send<TRequest, TResponse>(TRequest request)
            where TResponse : IResponse
            where TRequest : IRequest;
    }
}
