using Common.Core.CQRS.Request;
using Common.Core.CQRS.Shared;
using Common.Core.DependencyInjection;

namespace Common.Core.AspNet.Test.CQRS
{
    [ServiceLocate(typeof(INotificationHandler<LogNotification, LogResponse2>))]
    [CQRSNotificationHandler(nameof(LogNotification), typeof(INotificationHandler<LogNotification, LogResponse2>))]
    public class LogNotificationHandler2 : INotificationHandler<LogNotification, LogResponse2>
    {
        private readonly ILogger<LogNotificationHandler2> _logger;

        public LogNotificationHandler2(ILogger<LogNotificationHandler2> logger)
        {
            _logger = logger;
        }

        public async Task<LogResponse2> Handle(LogNotification request)
        {
            _logger.LogInformation("In Notification Handler 2");
            return await Task.FromResult(new LogResponse2());
        }
    }
}
