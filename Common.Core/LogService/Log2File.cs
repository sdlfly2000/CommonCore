using Common.Core.LogService.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
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
        private readonly int _entriesPerWrite;

        private IList<string> _logEntiries;

        private static object _writeLock = new object();

        public Log2File(IConfiguration configuration)
        {
            _logFilePath = configuration["LogFileSettings:Path"];
            _logLevel = Enum.Parse<LogLevel>(configuration["LogFileSettings:Level"]);

            _entriesPerWrite = configuration["LogFileSettings:EntryPerWrite"] != null 
                ? int.Parse(configuration["LogFileSettings:EntryPerWrite"])
                : Entries_Per_Write;
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
            DoLog(LogLevel.Information, information);
        }

        public void LogTrace(string information)
        {
            DoLog(LogLevel.Trace, information);
        }

        public void LogDebug(string information)
        {
            DoLog(LogLevel.Debug, information);
        }

        public void LogError(string information)
        {
            DoLog(LogLevel.Error, information);
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            _logEntiries.Add(formatter(state, exception));

            if (_logEntiries.Count >= _entriesPerWrite) AppendLogEntriesToLogFile(_logEntiries);
        }

        public void LogActivity(IActivityLog log)
        {
            // Log Title: DateTimeStamp,LogLevel,TraceId,ActivityName,ActivityVector,Passed,Exception

            var entryBuilder = new StringBuilder();

            AppendWithSplit(entryBuilder, DateTime.UtcNow.ToString());
            AppendWithSplit(entryBuilder, LogLevel.Information.ToString());
            AppendWithSplit(entryBuilder, log.TraceId);
            AppendWithSplit(entryBuilder, log.ActivityName);
            AppendWithSplit(entryBuilder, log.ActivityVector);
            AppendWithSplit(entryBuilder, log.Passed.ToString());
            AppendWithSplit(entryBuilder, log.Exception);

            Log(LogLevel.Information, default(EventId), entryBuilder.ToString(), null, (context, ex) =>
            {
                return context;
            });
        }

        public void Dispose()
        {
            if (_logEntiries.Any()) AppendLogEntriesToLogFile(_logEntiries);
        }

        #region Private Methods

        private string AddSplit() => ",";

        private void AppendWithSplit(StringBuilder builder ,string content)
        {
            builder.Append(content).Append(AddSplit());
        }

        private void AppendLogEntriesToLogFile(IList<string> logEntries)
        {
            try
            {
                lock (_writeLock)
                {
                    File.AppendAllLines(_logFilePath, logEntries);
                    _logEntiries.Clear();
                }
            }
            catch(Exception ex)
            {
                LogError(ex.Message);
            }
        }

        private void DoLog(LogLevel logLevel, string message)
        {
            Log(logLevel, default(EventId), message, null, (context, ex) =>
            {
                return context;
            });
        }

        #endregion
    }
}
