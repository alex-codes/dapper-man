using System.Data;
using System.Threading.Tasks;

namespace DapperMan.Core
{
    public interface IDapperCacheableNonQuery
    {
        int Execute<T>(object queryParameters = null, PropertyCache propertyCache = null, IDbTransaction transaction = null) where T : class;
        Task<int> ExecuteAsync<T>(object queryParameters = null, PropertyCache propertyCache = null, IDbTransaction transaction = null) where T : class;
    }
}
