using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;

namespace Common.Core.LogService
{
    public class Log2File : ILogger, IDisposable
    {
        private readonly StreamWriter _logFileStream;

        public Log2File(IConfiguration configuration)
        {
            _logFileStream = File.AppendText(configuration["LogFilePath"]);
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            throw new NotImplementedException();
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if(_logFileStream == null)
            {
                return;
            }

            var entryBuilder = new StringBuilder(DateTime.UtcNow.ToString());
            entryBuilder.Append(logLevel.ToString());
            entryBuilder.Append(eventId.ToString());
            entryBuilder.Append(nameof(state));
            entryBuilder.Append(exception);
            entryBuilder.Append(formatter);

            _logFileStream.Write(entryBuilder.ToString());

        }

        public void Dispose()
        {
            if (_logFileStream != null)
            {
                _logFileStream.Dispose();
            }
        }
    }
}
