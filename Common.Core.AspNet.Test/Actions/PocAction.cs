using Common.Core.AOP;
using Common.Core.AspNet.Test.Attributes;

namespace Common.Core.AspNet.Test.Actions
{
    [AOPInterception(typeof(IPocAction), typeof(PocAction))]
    public class PocAction : IPocAction
    {
        [PocCache]
        public string GetKey1()
        {
            Console.WriteLine("In Poc Action 1");
            return string.Empty; 
        }

        [PocCache]
        public string GetKey2()
        {
            Console.WriteLine("In Poc Action 2");
            return string.Empty;
        }
    }
}
