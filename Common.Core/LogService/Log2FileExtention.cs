using Common.Core.LogService.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Text;

namespace Common.Core.LogService
{
    public static class Log2FileExtention
    {
        public static void LogActivity(this ILogger logger, IActivityLog log)
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

            logger.Log(LogLevel.Information, default(EventId), entryBuilder.ToString(), null, (context, ex) =>
            {
                return context;
            });
        }

        #region Private Methods

        private const string SPLIT = ",";

        private static void AppendWithSplit(StringBuilder builder, string content)
        {
            builder.Append(content).Append(SPLIT);
        }

        #endregion
    }
}
