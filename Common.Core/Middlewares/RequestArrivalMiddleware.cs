using Common.Core.DependencyInjection;
using Common.Core.Middlewares.RequestTrace;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Common.Core.Middlewares
{
    [ServiceLocate(default)]
    public class RequestArrivalMiddleware : IMiddleware
    {
        private readonly IRequestTraceService _requestTraceService;

        public RequestArrivalMiddleware(
            IRequestTraceService requestTraceService)
        {
            _requestTraceService = requestTraceService;
        }

        async Task IMiddleware.InvokeAsync(HttpContext context, RequestDelegate next)
        {
            _requestTraceService.TraceId = Guid.NewGuid().ToString();

            await next.Invoke(context);
        }
    }
}
