using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DapperMan.Core
{
    public interface ISelectQueryBuilder
    {
        (IEnumerable<T> Results, int TotalRows) Execute<T>(object queryParameters = null, IDbTransaction transaction = null) where T : class;
        Task<(IEnumerable<T> Results, int TotalRows)> ExecuteAsync<T>(object queryParameters = null, IDbTransaction transaction = null) where T : class;
        ISelectQueryBuilder OrderBy(string orderBy);
        ISelectQueryBuilder SkipTake(int skip, int take);
        ISelectQueryBuilder Where(string filter);
    }
}
