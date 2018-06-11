using DapperMan.Core;
using DapperMan.Core.Attributes;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace DapperMan.MsSql
{
    public class InsertQuery : DapperQueryBase, IInsertQueryBuilder, IDapperQueryGenerator
    {
        private int? commandTimeout = null;
        private string defaultQyeryTemplate = "INSERT INTO {source} ({fields}) VALUES ({values});";
        private string keyField = null;
        private string[] propNames = null;
        private string scopeIdentityTemplate = "SELECT CAST(SCOPE_IDENTITY() as int);";

        public InsertQuery(string source, string connectionString)
            : this(source, connectionString, null)
        {
        }

        public InsertQuery(string source, string connectionString, int? commandTimeout)
           : base(connectionString)
        {
            this.commandTimeout = commandTimeout;
            Source = source;
        }

        public InsertQuery(string source, IDbConnection connection)
            : this(source, connection, null)
        {
        }

        public InsertQuery(string source, IDbConnection connection, int? commandTimeout)
            : base(connection)
        {
            this.commandTimeout = commandTimeout;
            Source = source;
        }

        public int Execute<T>(object queryParameters = null, ObjectCache propertyCache = null, IDbTransaction transaction = null) where T : class
        {
            ReflectType<T>(propertyCache);

            if (!string.IsNullOrWhiteSpace(keyField))
            {
                return Query<int>(GenerateStatement(), queryParameters, transaction: transaction).First();
            }
            else
            {
                return Execute(GenerateStatement(), queryParameters, transaction: transaction);
            }
        }

        public async Task<int> ExecuteAsync<T>(object queryParameters = null, ObjectCache propertyCache = null, IDbTransaction transaction = null) where T : class
        {
            ReflectType<T>(propertyCache);

            if (!string.IsNullOrWhiteSpace(keyField))
            {
                var result = await QueryAsync<int>(GenerateStatement(), queryParameters, transaction: transaction);
                return result.First();
            }
            else
            {
                return await ExecuteAsync(GenerateStatement(), queryParameters, transaction: transaction);
            }
        }

        protected string FormatPropertyNames(string[] props)
        {
            return string.Join(",", props);
        }

        protected string FormatPropertyParameters(string[] props)
        {
            return string.Join(",", props.Select(s => "@" + s));
        }

        public virtual string GenerateStatement()
        {
            string sql = defaultQyeryTemplate
                .Replace("{source}", Source)
                .Replace("{fields}", FormatPropertyNames(propNames))
                .Replace("{values}", FormatPropertyParameters(propNames));

            if (!string.IsNullOrWhiteSpace(keyField))
            {
                sql += scopeIdentityTemplate;
            }

            Debug.WriteLine(sql);

            return sql;
        }

        private void ReflectType<T>(ObjectCache propertyCache) where T : class
        {
            propNames = ReflectionHelper.ReflectProperties<T>(propertyCache, new[] { typeof(IdentityAttribute), typeof(InsertIgnoreAttribute) });
            keyField = ReflectionHelper.GetIdentityField<T>(propertyCache);
        }
    }
}
