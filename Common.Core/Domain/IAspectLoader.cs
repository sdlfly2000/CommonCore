using Common.Core.AOP;
using Common.Core.Domain;

namespace Common.Core.Data.Sql
{
    public interface IAspectLoader<out TAspect> where TAspect : IAspect
    {
        TAspect Load(IReference reference);
    }
}
