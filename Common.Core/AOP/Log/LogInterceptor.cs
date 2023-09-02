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

            _logger.LogActivity(new ActivityLog()
            {
                ActivityName = activityName,
                ActivityVector = string.Empty,
                Passed = 1,
                TraceId = string.Empty,
                Exception = string.Empty
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