using Common.Core.CQRS.Request;
using Common.Core.DependencyInjection;

namespace Common.Core.AspNet.Test.CQRS
{
    [ServiceLocate(typeof(INotificationHandler<LogNotification, IResponse>))]
    public class LogNotificationHandler1 : INotificationHandler<LogNotification, LogResponse>
    {
        private readonly ILogger _logger;

        public LogNotificationHandler1(ILogger<LogNotificationHandler1> logger)
        {
            _logger = logger;
        }

        public LogResponse Handle(LogNotification request)
        {
            _logger.LogInformation("In Notification Handler 1");
            return new LogResponse();
        }
    }
}
