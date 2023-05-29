using Common.Core.AOP.Cache;

namespace Common.Core.AspNet.Test.Models
{
    public class CachedObject : IReference
    {
        public string Code { get; set; }

        public string CacheFieldName { get; set; }

        public string CacheCode => string.Concat(CacheFieldName, Code);

        public CachedObject(string id, string name)
        {
            Code = id;
            CacheFieldName = name;
        }
    }
}
