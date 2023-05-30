using Common.Core.AOP;
using Common.Core.AOP.Log;

namespace Common.Core.AspNet.Test.Actions
{
    [AOPInterception(typeof(ILogTestAction), typeof(LogTestAction))]
    public class LogTestAction : ILogTestAction
    {
        public string TestLog()
        {
            return "Hello World";
        }
    }
}
