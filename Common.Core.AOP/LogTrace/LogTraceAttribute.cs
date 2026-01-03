using ArxOne.MrAdvice.Advice;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Common.Core.AOP.LogTrace
{
    /// <summary>
    /// Log Trace Attribute to log method execution time and trace id, and will catch exceptions and return a response with the exception message.
    /// Depends on IRequestTraceService to get the TraceId.
    /// Depends on ILogger to log the information.
    /// Depends on the ReturnType to create a response instance in case of exception, which should have a constructor with (string message, bool success).
    /// </summary>
    public class LogTraceAttribute : Attribute, IMethodAsyncAdvice
    {
        public Type ReturnType { get; set; }

        public LogTraceAttribute(Type returnType)
        {
            ReturnType = returnType;
        }

        /// <summary>
        /// Depends on IRequestTraceService to get the TraceId and logs the execution time of the method.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Advise(MethodAsyncAdviceContext context)
        {
            var stopWatch = Stopwatch.StartNew();

            var serviceProvider = context.GetMemberServiceProvider();

            var requestTraceService = serviceProvider?.GetRequiredService<ITraceIdService>();

            var logger = serviceProvider?.GetRequiredService<ILogger>();

            logger?.Information($"Trace Id: {{TraceId}}, Executing {{MetricExecutionTarget}}", requestTraceService?.TraceId, context.Target);

            try 
            { 
                await context.ProceedAsync().ConfigureAwait(false);
                logger?.Information($"Trace Id: {{TraceId}}, Executed successfully {{MetricExecutionTargetSuccess}} in {{MetricExecutionTimeInMs}} ms.", requestTraceService?.TraceId, context.Target, stopWatch.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                logger?.Error(ex, $"Trace Id: {{TraceId}}, Executed fail {{MetricExecutionTargetFailure}}.", requestTraceService?.TraceId, context.Target);
                
                if (ReturnType is not null)
                {
                    var response = Activator.CreateInstance(ReturnType, ex.Message, false);
                    context.ReturnValue = Task.FromResult(response);
                }
                else
                {
                    throw;
                }
            }
            finally
            {
                stopWatch.Stop();
            }          
        }
    }
}
