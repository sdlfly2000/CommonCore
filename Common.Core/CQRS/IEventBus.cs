using Common.Core.CQRS.Notification;
using Common.Core.CQRS.Request;
using System.Collections.Generic;

namespace Common.Core.CQRS
{
    public interface IEventBus
    {
        IResponse Send<TRequest, TResponse>(TRequest request)
            where TResponse : IResponse
            where TRequest : IRequest;

        List<IResponse> Publish<TRequest, TResponse>(TRequest request)
            where TResponse : IResponse
            where TRequest : INotification;
    }
}
