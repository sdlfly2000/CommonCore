using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;

namespace Common.Core.LogService
{
    public class Log2File : ILog2File
    {
        private readonly StreamWriter _logFileStream;
        private readonly LogLevel _logLevel;

        public Log2File(IConfiguration configuration)
        {
            _logFileStream = File.AppendText(configuration["LogFileSettings:Path"]);
            _logLevel = Enum.Parse<LogLevel>(configuration["LogFileSettings:Level"]);
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return _logLevel <= logLevel;
        }

        public void LogInformation(string information)
        {
            Log(LogLevel.Information, default(EventId), information, null, (context, ex) => 
            {
                return context; 
            });
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (_logFileStream == null)
            {
                return;
            }

            var entryBuilder = new StringBuilder();

            AppendWithSplit(entryBuilder, DateTime.UtcNow.ToString());
            AppendWithSplit(entryBuilder, logLevel.ToString());
            if(eventId != default(EventId)) AppendWithSplit(entryBuilder, eventId.ToString());
            AppendWithSplit(entryBuilder, typeof(TState).Name);            
            if(exception != null) AppendWithSplit(entryBuilder, exception.Message);            
            entryBuilder.Append(formatter(state, exception));

            _logFileStream.WriteLine(entryBuilder.ToString());
        }

        public void Dispose()
        {
            if (_logFileStream != null)
            {
                _logFileStream.Dispose();
            }
        }

        #region Private Methods

        private string AddSplit() => " | ";

        private void AppendWithSplit(StringBuilder builder ,string content)
        {
            builder.Append(content).Append(AddSplit());
        }

        #endregion
    }
}
