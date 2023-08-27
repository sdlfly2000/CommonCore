using Castle.DynamicProxy;
using Common.Core.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Common.Core.AOP.Log
{
    [ServiceLocate(typeof(ILogInterceptor))]
    public class LogInterceptor : ILogInterceptor
    {
        private readonly ILogger<LogInterceptor> _logger;

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

            if (activityName != string.Empty)
            {
                _logger.LogTrace("Complete: {0}", activityName);
            }
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