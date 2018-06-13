using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DapperMan.Core
{
    public interface IStoredProcedureQueryBuilder
    {
        int Execute(object queryParameters = null, IDbTransaction transaction = null);
        Task<int> ExecuteAsync(object queryParameters = null, IDbTransaction transaction = null);
        (IEnumerable<T> Results, int ReturnValue) Execute<T>(object queryParameters = null, IDbTransaction transaction = null) where T : class;
        Task<(IEnumerable<T> Results, int ReturnValue)> ExecuteAsync<T>(object queryParameters = null, IDbTransaction transaction = null) where T : class;
    }
}
