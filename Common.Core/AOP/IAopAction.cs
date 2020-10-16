using System.Reflection;

namespace Common.Core.AOP
{
    public interface IAopAction
    {
        object BeforeAction(MethodInfo targetMethod, object[] args);

        object AfterAction(MethodInfo targetMethod, object[] args);
    }
}
