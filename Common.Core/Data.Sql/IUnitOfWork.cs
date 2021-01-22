namespace Common.Core.Data.Sql
{
    public interface IUnitOfWork<in TEntity> where TEntity : class
    {
        void Persist<TEntity>(TEntity entity) where TEntity : class;

        void Add<TEntity>(TEntity entity) where TEntity : class;
    }
}