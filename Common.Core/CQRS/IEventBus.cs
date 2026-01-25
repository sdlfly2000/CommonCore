using Common.Core.CQRS.Notification;
using Common.Core.CQRS.Request;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Core.CQRS
{
    public interface IEventBus
    {
        Task<TResponse> Send<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
            where TResponse : IResponse
            where TRequest : IRequest;

        Task<List<IResponse>> Publish<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
            where TResponse : IResponse
            where TRequest : INotification;
    }
}
