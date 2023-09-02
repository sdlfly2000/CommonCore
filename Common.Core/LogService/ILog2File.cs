using Common.Core.LogService.Models;
using Microsoft.Extensions.Logging;
using System;

namespace Common.Core.LogService
{
    public interface ILog2File : ILogger, IDisposable
    {
    }
}
