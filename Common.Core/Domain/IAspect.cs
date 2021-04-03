using Common.Core.AOP;

namespace Common.Core.Domain
{
    public interface IAspect
    {
        IReference Reference { get; set; }
    }
}
