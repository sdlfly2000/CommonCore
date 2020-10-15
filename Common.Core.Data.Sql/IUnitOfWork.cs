using Microsoft.EntityFrameworkCore;

namespace Common.Core.Data.Sql
{
    public interface IUnitOfWork<T> where T : class
    {
        DbSet<T> LoadAll();
    }
}