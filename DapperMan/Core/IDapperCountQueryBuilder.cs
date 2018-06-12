using System.Data;
using System.Threading.Tasks;

namespace DapperMan.Core
{
    public interface IDapperCountQueryBuilder
    {
        int Execute(object queryParameters = null, IDbTransaction transaction = null);
        Task<int> ExecuteAsync(object queryParameters = null, IDbTransaction transaction = null);
        IDapperCountQueryBuilder Where(string filter);
    }
}
