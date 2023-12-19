namespace Common.Core.AspNet.Test.CQRS
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CQRSRequestHandlerAttribute : Attribute
    {
        private readonly Type _request;
        private readonly Type _requestHandler;

        public CQRSRequestHandlerAttribute(Type Request, Type RequestHandler)
        {
            _request = Request;
            _requestHandler = RequestHandler;
        }

        public Type Request { get => _request; }
        public Type RequestHandler { get => _requestHandler; }
    }
}

