using Common.Core.CQRS.Request;
using Common.Core.CQRS.Shared;
using Common.Core.DependencyInjection;

namespace Common.Core.AspNet.Test.CQRS
{
    [ServiceLocate(typeof(INotificationHandler<LogNotification, LogResponse>))]
    [CQRSNotificationHandler(nameof(LogNotification), typeof(INotificationHandler<LogNotification, LogResponse>))]
    public class LogNotificationHandler1 : INotificationHandler<LogNotification, LogResponse>
    {
        private readonly ILogger _logger;

        public LogNotificationHandler1(ILogger<LogNotificationHandler1> logger)
        {
            _logger = logger;
        }

        public async Task<LogResponse> Handle(LogNotification request)
        {
            _logger.LogInformation("In Notification Handler 1");
            return await Task.FromResult(new LogResponse());
        }
    }
}
