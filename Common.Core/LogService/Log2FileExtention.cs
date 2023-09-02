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
            logger.Log(LogLevel.Information, default(EventId), log, null, (context, ex) =>
            {
                return string.Empty;
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
