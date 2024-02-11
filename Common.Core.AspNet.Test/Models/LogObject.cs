using Common.Core.AOP.Cache;

namespace Common.Core.AspNet.Test.Models
{
    public class LogObject : IReference
    {
        public string Code { get; set; }

        public string CacheFieldName { get; set; }

        public string CacheCode => string.Concat(CacheFieldName, Code);

        public LogObject(string id, string name)
        {
            Code = id;
            CacheFieldName = name;
        }
    }
}
