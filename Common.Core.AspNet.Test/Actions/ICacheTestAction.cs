using Common.Core.AspNet.Test.Models;

namespace Common.Core.AspNet.Test.Actions
{
    public interface ICacheTestAction
    {
        CachedObject CreateObject(CachedObject cachedObject);
    }
}
