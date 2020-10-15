using Microsoft.EntityFrameworkCore;
using System;

namespace Common.Core.Data.Sql
{
    public class UnitOfWork<TEnity> : IUnitOfWork<TEnity>, IDisposable 
        where TEnity : class
    {
        private DbContext _context;

        public UnitOfWork(DbContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public DbSet<TEnity> LoadAll()
        {
            return _context.Set<TEnity>();
        }
    }
}
