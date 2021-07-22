using System.Collections.Generic;
using System.Linq;

namespace Workflow.Core
{
    public abstract class Workflow<TContext> : IWorkflow<TContext> where TContext : class
    {
        private IEnumerable<IActivity<TContext>> _activities;

        public Workflow(params IActivity<TContext>[] activities)
        {
            _activities = activities.AsEnumerable();
        }

        public virtual void Execute(TContext context)
        {
            foreach (var activity in _activities)
            {
                activity.Execute(context);
            }
        }
    }
}
