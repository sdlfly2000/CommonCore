namespace Workflow.Core
{
    public interface IActivity<TContext> where TContext : class
    {
        void Execute(TContext context);
    }
}
