using System.Threading.Tasks;

namespace Common.Core.CQRS.Request
{
    public interface INotificationHandler<in INotification, IResponse>
    {
        Task<IResponse> Handle(INotification request);
    }
}
