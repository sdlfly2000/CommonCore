using ArxOne.MrAdvice.Advice;
using System.Reflection;

namespace Common.Core.AOP
{
    public static class MrAdviceExtension
    {
        public static IServiceProvider? GetMemberServiceProvider(this MethodAsyncAdviceContext context)
        {
            var targetType = context.Target.GetType();

            return targetType.GetRuntimeFields()
                .SingleOrDefault(field => field.FieldType == typeof(IServiceProvider))?
                .GetValue(context.Target) as IServiceProvider;
        }
    }    
}
