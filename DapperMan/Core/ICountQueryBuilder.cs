using System.Data;
using System.Threading.Tasks;

namespace DapperMan.Core
{
    public interface ICountQueryBuilder
    {
        int Execute(object queryParameters = null, IDbTransaction transaction = null);
        Task<int> ExecuteAsync(object queryParameters = null, IDbTransaction transaction = null);
        ICountQueryBuilder Where(string filter);
    }
}
