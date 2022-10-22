using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Common.Core.LogService
{
    public class Log2File : ILog2File
    {
        private const int Entries_Per_Write = 10;

        private readonly string _logFilePath;
        private readonly LogLevel _logLevel;

        private IList<string> _logEntiries;

        public Log2File(IConfiguration configuration)
        {
            _logFilePath = configuration["LogFileSettings:Path"];
            _logLevel = Enum.Parse<LogLevel>(configuration["LogFileSettings:Level"]);
            _logEntiries = new List<string>();
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return this;
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
            var entryBuilder = new StringBuilder();

            AppendWithSplit(entryBuilder, DateTime.UtcNow.ToString());
            AppendWithSplit(entryBuilder, logLevel.ToString());
            if (eventId != default(EventId)) AppendWithSplit(entryBuilder, eventId.ToString());
            AppendWithSplit(entryBuilder, typeof(TState).Name);
            if (exception != null) AppendWithSplit(entryBuilder, exception.Message);
            entryBuilder.Append(formatter(state, exception));
        
            _logEntiries.Add(entryBuilder.ToString());

            if (_logEntiries.Count > Entries_Per_Write) AppendLogEntriesToLogFile(_logEntiries);
        }

        public void Dispose()
        {
            if (_logEntiries.Any()) AppendLogEntriesToLogFile(_logEntiries);
        }

        #region Private Methods

        private string AddSplit() => " | ";

        private void AppendWithSplit(StringBuilder builder ,string content)
        {
            builder.Append(content).Append(AddSplit());
        }

        private void AppendLogEntriesToLogFile(IList<string> logEntries)
        {
            File.AppendAllLines(_logFilePath,logEntries);
            _logEntiries.Clear();
        }

        #endregion
    }
}
