using Common.Core.AOP;
using System.Collections.Concurrent;
using System.Reflection;

namespace Common.Core.AspNet.Test.CQRS
{
    public static class RegisterCQRS
    {
        private static ConcurrentDictionary<Type, Type> _requestHandlerMapper = new ConcurrentDictionary<Type, Type>();

        public static void Register(params string[] domains)
        {
            var asms = domains.Select(Assembly.Load).ToList();

            var exportedTypes = asms.SelectMany(asm => asm.GetExportedTypes().Where(t => !t.IsInterface).ToList()).ToList();

            var requestHandlers = exportedTypes.SelectMany(exportedType => exportedType.GetCustomAttributes(typeof(CQRSRequestHandlerAttribute))).ToList();

            requestHandlers.ForEach(
                requestHandler =>
                    {
                        var requestHandlerAttribute = (CQRSRequestHandlerAttribute)requestHandler;
                        var requestType = requestHandlerAttribute.Request;
                        var requesthandlerType = requestHandlerAttribute.RequestHandler;
                        if (!_requestHandlerMapper.TryAdd(requestType, requesthandlerType))
                        {
                            throw new InvalidOperationException($"Failed to register Request Handlder: {requesthandlerType.Name}");
                        };
                    });
        }

        public static ConcurrentDictionary<Type, Type> RequestHandlerMapper { get => _requestHandlerMapper; }
    }
}
