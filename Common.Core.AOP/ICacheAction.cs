namespace Common.Core.AOP
{
    public interface ICacheAction<out T> : IAopAction where T : ICacheAspect
    {
    }
}