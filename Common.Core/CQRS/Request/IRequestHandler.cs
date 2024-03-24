using System.Threading.Tasks;

namespace Common.Core.CQRS.Request
{
    public interface IRequestHandler<in IRequest, TResponse>
    {
        Task<TResponse> Handle(IRequest request);
    }
}
