using ArxOne.MrAdvice.Advice;
using System;
using System.Threading.Tasks;

namespace Common.Core.AOP.CatchException
{
    public class CatchAppExceptionAttribute : Attribute, IMethodAsyncAdvice
    {
        public Type ReturnType { get; set; }

        public CatchAppExceptionAttribute(Type returnType)
        {
            ReturnType = returnType;
        }

        public async Task Advise(MethodAsyncAdviceContext context)
        {
            try
            {
                await context.ProceedAsync().ConfigureAwait(false);
            }
            catch(Exception e)
            {
                var response = Activator.CreateInstance(ReturnType, e.Message, false);
                context.ReturnValue = Task.FromResult(response);
            }
        }
    }
}
