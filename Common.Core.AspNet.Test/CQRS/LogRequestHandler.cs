﻿using Common.Core.DependencyInjection;

namespace Common.Core.AspNet.Test.CQRS
{
    [CQRSRequestHandler(typeof(LogRequest), typeof(IRequestHandler<LogRequest, LogResponse>))]
    [ServiceLocate(typeof(IRequestHandler<LogRequest, LogResponse>))]
    public sealed class LogRequestHandler : IRequestHandler<LogRequest, LogResponse>
    {
        private readonly ILogger<LogRequestHandler> _logger;

        public LogRequestHandler(ILogger<LogRequestHandler> logger)
        {
            _logger = logger;
        }

        public LogResponse Handle(LogRequest request)
        {
            _logger.LogInformation(request.Message);
            return new LogResponse();
        }
    }
}