namespace Workflow.Core
{
    public interface ISequencialActivity<TContext> : IActivity<TContext>
        where TContext : class
    {
    }
}
