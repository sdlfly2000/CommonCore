namespace Common.Core.AspNet.Test.CQRS
{
    public class LogRequest : IRequest
    {
        private readonly string _message;

        public LogRequest(string message)
        {
            _message = message;
        }

        public string Message { get => _message; }
    }
}
