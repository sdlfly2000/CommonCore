using Common.Core.LogService.Models;
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
        private readonly int _entriesPerWrite;

        private IList<string> _logEntiries;

        private static object _writeLock = new object();

        private const string SPLIT = ",";

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

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            var context = FormatDefaultLogEntry(logLevel, eventId, state, exception, formatter);

            if (state is IActivityLog) 
            {
                context = FormatActivityLogEntry(state, context); 
            }

            context = RemoveLastSplit(context);

            _logEntiries.Add(context.ToString());                        

            if (_logEntiries.Count >= _entriesPerWrite) AppendLogEntriesToLogFile(_logEntiries);
        }            

        public void Dispose()
        {
            if (_logEntiries.Any()) AppendLogEntriesToLogFile(_logEntiries);
        }

        #region Private Methods

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
                Log(LogLevel.Error, default, string.Empty, ex, (state, ex) => string.Empty); 
            }
        }

        private StringBuilder FormatDefaultLogEntry<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            // Log Title: DateTimeStamp,LogLevel,EventId,Passed,Exception,Formatted

            var entryBuilder = new StringBuilder();
            entryBuilder.Append(DateTime.UtcNow.ToString()).Append(SPLIT);
            entryBuilder.Append(LogLevel.Information.ToString()).Append(SPLIT);
            entryBuilder.Append(eventId.Name).Append(SPLIT);
            entryBuilder.Append(exception == null ? 0 : 1).Append(SPLIT);
            entryBuilder.Append(exception == null ? string.Empty : exception.Message).Append(SPLIT);
            entryBuilder.Append(formatter(state, exception)).Append(SPLIT);

            return entryBuilder;
        }

        private StringBuilder FormatActivityLogEntry<TState>(TState state, StringBuilder stringBuilder)
        {
            // Log Title: DateTimeStamp,LogLevel,EventId,Passed,Exception,Formatted,TraceId,ActivityName,ActivityVector

            if (state == null)
            {
                return stringBuilder;
            }

            stringBuilder.Append(((IActivityLog)state).TraceId).Append(SPLIT);
            stringBuilder.Append(((IActivityLog)state).ActivityName).Append(SPLIT);
            stringBuilder.Append(((IActivityLog)state).ActivityVector).Append(SPLIT);

            return stringBuilder;
        }

        private StringBuilder RemoveLastSplit(StringBuilder stringBuilder)
        {
            return stringBuilder.Remove(stringBuilder.Length - 1, 1);
        }

        #endregion
    }
}
