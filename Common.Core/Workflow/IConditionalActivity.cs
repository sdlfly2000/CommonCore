namespace Workflow.Core
{
    public interface IConditionalActivity<TContext> : IActivity<TContext> 
        where TContext : class
    {
    }
}
