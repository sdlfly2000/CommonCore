using Common.Core.DependencyInjection;

namespace Common.Core.AspNet.Test.Actions
{
    [ServiceLocate(default)]
    public class ImplementToServiceAction
    {
        public string WhoAmI()
        {
            return "OK";
        }
    }
}
