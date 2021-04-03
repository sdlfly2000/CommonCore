using Common.Core.Domain;
using System;

namespace Common.Core.Data.Sql
{
    public abstract class Synchronizor<TAspect, TEntity> : ISynchronizor<TAspect>
        where TAspect : IAspect
        where TEntity : class
    {
        private readonly IUnitOfWork<TEntity> _uow;
        private readonly IAspectLoader<TAspect> _aspectLoader;

        public Synchronizor(
            IUnitOfWork<TEntity> uow,
            IAspectLoader<TAspect> aspectLoader)
        {
            _uow = uow;
            _aspectLoader = aspectLoader;
        }

        public void Add(TAspect aspect)
        {
            _uow.Add(Map(aspect));
        }

        public void Synchronize(TAspect aspect)
        {
            var aspectLoaded = _aspectLoader.Load(aspect.Reference);

            if (aspectLoaded.Equals(default(TAspect)))
            {
                Add(aspect);
            }
            else
            {
                Update(aspect);
            }
        }

        public void Update(TAspect aspect)
        {
            _uow.Persist(Map(aspect));
        }

        protected virtual TEntity Map(TAspect aspect)
        {
            throw new NotImplementedException();
        }
    }
}
