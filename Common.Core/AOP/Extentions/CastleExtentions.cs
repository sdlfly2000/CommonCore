using Castle.DynamicProxy;
using System.Reflection;
using System.Threading.Tasks;

namespace Common.Core.AOP.Extentions;

// Referenced to https://stackoverflow.com/questions/28099669/intercept-async-method-that-returns-generic-task-via-dynamicproxy/43272955#43272955

public static class CastleExtentions
{
    private static readonly MethodInfo handleAsyncMethodInfo; 

    static CastleExtentions()
    {
        handleAsyncMethodInfo = (typeof(CastleExtentions)).GetMethod("HandleAsyncWithResult", BindingFlags.NonPublic | BindingFlags.Static);
    }

    public static void ProceedAsync(this IInvocation invocation)
    {
        var delegateType = GetDelegateType(invocation);

        if (delegateType == MethodType.Synchronous)
        {
            invocation.Proceed();
        }

        if (delegateType == MethodType.AsyncAction)
        {
            invocation.Proceed();
            invocation.ReturnValue = HandleAsync((Task)invocation.ReturnValue);
        }

        if (delegateType == MethodType.AsyncFunction)
        {
            invocation.Proceed();
            ExecuteHandleAsyncWithResultUsingReflection(invocation);
        }
    }

    private static void ExecuteHandleAsyncWithResultUsingReflection(IInvocation invocation)
    {
        var resultType = invocation.Method.ReturnType.GetGenericArguments()[0];
        var mi = handleAsyncMethodInfo.MakeGenericMethod(resultType);
        invocation.ReturnValue = mi.Invoke(invocation.InvocationTarget, new[] { invocation.ReturnValue });
    }

    private static async Task HandleAsync(Task task)
    {
        await task;
    }

    private static async Task<T> HandleAsyncWithResult<T>(Task<T> task)
    {
        return await task;
    }

    private static MethodType GetDelegateType(IInvocation invocation)
    {
        var returnType = invocation.Method.ReturnType;
        if (returnType == typeof(Task))
            return MethodType.AsyncAction;
        if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
            return MethodType.AsyncFunction;
        return MethodType.Synchronous;
    }

    private enum MethodType
    {
        Synchronous,
        AsyncAction,
        AsyncFunction
    }
}
