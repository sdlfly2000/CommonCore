using Common.Core.AOP;
using Common.Core.AOP.Log;
using Common.Core.CQRS.Request;

namespace Common.Core.AspNet.Test.CQRS
{
    [AOPInterception(typeof(IRequestHandler<LogRequest, LogResponse>), typeof(LogRequestHandler))]
    public sealed class LogRequestHandler : IRequestHandler<LogRequest, LogResponse>
    {
        private readonly ILogger<LogRequestHandler> _logger;

        public LogRequestHandler(ILogger<LogRequestHandler> logger)
        {
            _logger = logger;
        }

        [LogTrace(nameof(LogRequestHandler))]
        public async Task<LogResponse> Handle(LogRequest request)
        {
            _logger.LogInformation(request.Message);
            return await Task.FromResult(new LogResponse());
        }
    }
}
