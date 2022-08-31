using Common.Core.AOP.Cache;

namespace Common.Core.Domain
{
    public interface IAspect
    {
        IReference Reference { get; set; }
    }
}
