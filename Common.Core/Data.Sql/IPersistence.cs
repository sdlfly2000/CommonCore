using System.Threading.Tasks;

namespace Common.Core.Data.Sql
{
    public interface IPersistence
    {
        Task<int> Complete();
    }
}
