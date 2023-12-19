namespace Common.Core.AspNet.Test.CQRS
{
    public interface IRequestHandler<in IRequest, out TResponse>
    {
        TResponse Handle(IRequest request);
    }
}
