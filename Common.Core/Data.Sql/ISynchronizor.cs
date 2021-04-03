using Common.Core.Domain;

namespace Common.Core.Data.Sql
{
    public interface ISynchronizor<in TAspect> where TAspect : IAspect
    {
        void Synchronize(TAspect aspect);

        void Add(TAspect aspect);

        void Update(TAspect aspect);
    }
}
