using Common.Core.DependencyInjection;

namespace Common.Core.Cache.Redis
{
    [ServiceLocate(typeof(IRedisCacheProcess))]
    public class RedisCacheProcess : IRedisCacheProcess
    {
    }
}
