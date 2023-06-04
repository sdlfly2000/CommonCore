using Castle.DynamicProxy;
using Common.Core.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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

            _logger.LogTrace("Executing: {0}", invocation.MethodInvocationTarget.Name);
            invocation.Proceed();
            _logger.LogTrace("With Result: {0}", JsonConvert.ToString(invocation.ReturnValue));
            _logger.LogTrace("Exiting: {0}", invocation.MethodInvocationTarget.Name);  
        }
    }
}
