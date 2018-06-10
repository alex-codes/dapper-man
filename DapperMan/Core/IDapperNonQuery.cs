using System.Data;
using System.Threading.Tasks;

namespace DapperMan.Core
{
    public interface IDapperNonQuery
    {
        int Execute(object queryParameters = null, IDbTransaction transaction = null);
        Task<int> ExecuteAsync(object queryParameters = null, IDbTransaction transaction = null);
    }
}
