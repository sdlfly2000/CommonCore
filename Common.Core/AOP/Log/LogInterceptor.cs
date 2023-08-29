using Castle.DynamicProxy;
using Common.Core.DependencyInjection;
using Common.Core.LogService;
using Common.Core.LogService.Models;
using Microsoft.Extensions.Logging;
using System.Text;
using System;
using System.Reflection.Metadata;
using Microsoft.Extensions.Primitives;

namespace Common.Core.AOP.Log
{
    [ServiceLocate(typeof(ILogInterceptor))]
    public class LogInterceptor : ILogInterceptor
    {
        private readonly ILogger<LogInterceptor> _logger;

        private const string SPLIT = ",";

        public LogInterceptor(ILogger<LogInterceptor> logger)
        {
            _logger = logger;
        }

        public void Intercept(IInvocation invocation)
        {
            if (!invocation.MethodInvocationTarget.IsDefined(typeof(LogTraceAttribute), false))
            {
                invocation.Proceed();
                return;
            }

            var activityName = GetActivityName(invocation.MethodInvocationTarget.GetCustomAttributes(typeof(LogTraceAttribute), false));

            invocation.Proceed();

            var activityLog = new ActivityLog()
            {
                ActivityName = activityName,
                ActivityVector = string.Empty,
                Passed = 1,
                TraceId = string.Empty,
                Exception = string.Empty
            };

            _logger.Log(LogLevel.Information, eventId: default, activityLog, exception: null, (log, ex) =>
            {            
                // Log Title: DateTimeStamp,LogLevel,TraceId,ActivityName,ActivityVector,Passed,Exception

                var entryBuilder = new StringBuilder();
                entryBuilder.Append(DateTime.UtcNow.ToString()).Append(SPLIT);
                entryBuilder.Append(LogLevel.Information.ToString()).Append(SPLIT);
                entryBuilder.Append(log.TraceId).Append(SPLIT);
                entryBuilder.Append(log.ActivityName).Append(SPLIT);
                entryBuilder.Append(log.ActivityVector).Append(SPLIT);
                entryBuilder.Append(log.Passed.ToString()).Append(SPLIT);
                entryBuilder.Append(log.Exception).Append(SPLIT);

                return entryBuilder.ToString();
            });            
        }

        #region Priviate Method

        private string GetActivityName(object[] attributes)
        {
            if((attributes.Length > 1) || (attributes.Length == 0))
            {
                return string.Empty;
            }

            return (attributes[0] as LogTraceAttribute)!.ActivityName;
        }

        #endregion
    }
}