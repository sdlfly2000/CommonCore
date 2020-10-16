using System.Reflection;
using System.Linq;

namespace Common.Core.AOP
{
    public class CacheProxy : CommonProxy
    {
        public ICacheAction<ICacheAspect> CacheAction { get; set; }
        
        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            object obj;
            if (args.Length > 0)
            {
                obj = CacheAction.BeforeAction(targetMethod, args);

                if (obj != null)
                {
                    return obj;
                }
            }

            obj = targetMethod.Invoke(Wrapped, args);

            var parameters = args.Append(obj).ToArray();

            return CacheAction.AfterAction(targetMethod, parameters);
        }
    }
}
