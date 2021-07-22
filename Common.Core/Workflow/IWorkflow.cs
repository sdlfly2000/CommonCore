namespace Workflow.Core
{
    public interface IWorkflow<TContext> : IActivity<TContext> where TContext:class
    {
    }
}
