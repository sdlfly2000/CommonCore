using System.Reflection;

namespace Common.Core.AOP.Cache
{
    public interface IAopAction
    {
        object BeforeAction(MethodInfo targetMethod, object[] args);

        object AfterAction(MethodInfo targetMethod, object[] args);
    }
}
