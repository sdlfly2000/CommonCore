using System.Reflection;

namespace Common.Core.AOP.Cache.DI
{
    public interface IAopAction
    {
        object BeforeAction(MethodInfo targetMethod, object[] args);

        object AfterAction(MethodInfo targetMethod, object[] args);
    }
}
