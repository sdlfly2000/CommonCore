namespace Common.Core.Middlewares.RequestTrace
{
    public interface IRequestTraceService
    {
        public string TraceId {  get; set; }
    }
}
