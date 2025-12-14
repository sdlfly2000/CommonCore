using Common.Core.DependencyInjection;

namespace Common.Core.Middlewares.RequestTrace
{
    [ServiceLocate(typeof(IRequestTraceService), ServiceType.Scoped)]
    public class RequestTraceService : IRequestTraceService
    {
        public string TraceId { get; set; }
    }
}
