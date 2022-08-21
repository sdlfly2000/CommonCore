using System.Reflection;
using System.Linq;

namespace Common.Core.AOP
{
    public class CacheProxy : CommonProxy
    {
        private readonly ICacheAction _cacheAction;

        public CacheProxy(ICacheAction cacheAction)
        {
            _cacheAction = cacheAction;
        }
        
        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            object obj;
            if (args.Length > 0)
            {
                obj = _cacheAction.BeforeAction(targetMethod, args);

                if (obj != null)
                {
                    return obj;
                }
            }

            obj = targetMethod.Invoke(Wrapped, args);

            var parameters = args.Append(obj).ToArray();

            return _cacheAction.AfterAction(targetMethod, parameters);
        }
    }
}
