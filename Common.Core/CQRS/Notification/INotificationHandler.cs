namespace Common.Core.CQRS.Request
{
    public interface INotificationHandler<in IRequest, out TResponse>
    {
        TResponse Handle(IRequest request);
    }
}
