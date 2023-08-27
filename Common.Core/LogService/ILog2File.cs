using Common.Core.LogService.Models;
using Microsoft.Extensions.Logging;
using System;

namespace Common.Core.LogService
{
    public interface ILog2File : ILogger, IDisposable
    {
        void LogInformation(string information);
        void LogTrace(string information);
        void LogDebug(string information);
        void LogError(string information);

        void LogActivity(IActivityLog log);
    }
}
