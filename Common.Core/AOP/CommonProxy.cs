using System.Reflection;

namespace Common.Core.AOP
{
    public class CommonProxy : DispatchProxy
    {
        public object Wrapped { get; set; }

        public IAopAction AdditionAction { get; set; }

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            AdditionAction.BeforeAction(targetMethod, args);

            var obj = targetMethod.Invoke(Wrapped, args);

            AdditionAction.AfterAction(targetMethod, args);

            return obj;
        }
    }
}
