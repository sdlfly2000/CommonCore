using System;

namespace Workflow.Core
{
    public class YesNoActivity<TContext> : IYesNoActivity<TContext> where TContext : class
    {
        private readonly Func<TContext, bool> _condition;
        private readonly IActivity<TContext> _yesActivity;
        private readonly IActivity<TContext> _noActivity;

        public YesNoActivity(
            Func<TContext, bool> condition, 
            IActivity<TContext> yesActivity, 
            IActivity<TContext> noActivity)
        {
            _condition = condition;
            _yesActivity = yesActivity;
            _noActivity = noActivity;
        }

        public void Execute(TContext context)
        {
            if (_condition(context))
            {
                _yesActivity.Execute(context);
            }
            else
            {
                _noActivity.Execute(context);
            }
        }
    }
}
