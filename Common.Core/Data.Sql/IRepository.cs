using System.Collections.Generic;

namespace Common.Core.Data.Sql
{
    public interface IRepository<TEntity>
        where TEntity: class
    {
        IEnumerable<TEntity> LoadAll();

        void Update(TEntity entity);

        void Create(TEntity entity);

        void Persist(TEntity entity);

        void Commit();
    }
}
