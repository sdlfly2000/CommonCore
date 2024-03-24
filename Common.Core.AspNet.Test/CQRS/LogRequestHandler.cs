using Common.Core.CQRS.Request;
using Common.Core.DependencyInjection;

namespace Common.Core.AspNet.Test.CQRS
{
    [ServiceLocate(typeof(IRequestHandler<LogRequest, LogResponse>))]
    public sealed class LogRequestHandler : IRequestHandler<LogRequest, LogResponse>
    {
        private readonly ILogger<LogRequestHandler> _logger;

        public LogRequestHandler(ILogger<LogRequestHandler> logger)
        {
            _logger = logger;
        }

        public async Task<LogResponse> Handle(LogRequest request)
        {
            _logger.LogInformation(request.Message);
            return await Task.FromResult(new LogResponse());
        }
    }
}
