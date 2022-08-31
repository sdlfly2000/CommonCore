using Common.Core.AOP.Cache;

namespace Common.Core.AOP
{
    public interface ICacheAction<out TAspect, out TReference> : IAopAction 
        where TReference : IReference
    {
    }
}