using System;

namespace Workflow.Core
{
    public class ConditionalActivity<TContext> : IConditionalActivity<TContext> where TContext : class
    {
        private readonly Func<TContext, bool> _condition;
        private readonly IActivity<TContext> _activity;

        public ConditionalActivity(Func<TContext, bool> condition, IActivity<TContext> activity)
        {
            _condition = condition;
            _activity = activity;
        }

        public virtual void Execute(TContext context)
        {
            if (_condition(context))
            {
                _activity.Execute(context);
            }
        }
    }
}
