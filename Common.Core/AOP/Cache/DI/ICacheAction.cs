namespace Common.Core.AOP.Cache.DI
{
    public interface ICacheAction<out TAspect, out TReference> : IAopAction
        where TReference : IReference
    {
    }
}