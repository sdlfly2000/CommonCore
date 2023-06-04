﻿using Microsoft.Extensions.Configuration;
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

            var entryBuilder = new StringBuilder();

            AppendWithSplit(entryBuilder, DateTime.UtcNow.ToString());
            AppendWithSplit(entryBuilder, logLevel.ToString());
            if (eventId != default(EventId)) AppendWithSplit(entryBuilder, eventId.ToString());
            AppendWithSplit(entryBuilder, typeof(TState).Name);
            if (exception != null) AppendWithSplit(entryBuilder, exception.Message);
            entryBuilder.Append(formatter(state, exception));
        
            _logEntiries.Add(entryBuilder.ToString());

            if (_logEntiries.Count >= _entriesPerWrite) AppendLogEntriesToLogFile(_logEntiries);
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
            lock (_writeLock)
            {
                File.AppendAllLines(_logFilePath, logEntries);
                _logEntiries.Clear();
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
