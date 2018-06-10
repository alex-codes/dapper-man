using System.Data;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace DapperMan.Core
{
    public interface IDapperCacheableNonQuery
    {
        int Execute<T>(object queryParameters = null, ObjectCache propertyCache = null, IDbTransaction transaction = null) where T : class;
        Task<int> ExecuteAsync<T>(object queryParameters = null, ObjectCache propertyCache = null, IDbTransaction transaction = null) where T : class;
    }
}
