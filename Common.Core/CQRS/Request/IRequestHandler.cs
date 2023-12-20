namespace Common.Core.CQRS.Request
{
    public interface IRequestHandler<in IRequest, out TResponse>
    {
        TResponse Handle(IRequest request);
    }
}
