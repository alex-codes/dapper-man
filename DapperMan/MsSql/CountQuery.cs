using DapperMan.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DapperMan.MsSql
{
    public class CountQuery : DapperQueryBase, IDapperCountQueryBuilder, IDapperQueryGenerator
    {
        private string defaultQueryTemplate = "SELECT [Count] = COUNT(*) FROM {source} {filter};";
        protected List<string> Filters { get; private set; } = new List<string>();

        public CountQuery(string source, string connectionString)
            : this(source, connectionString, null)
        {

        }

        public CountQuery(string source, string connectionString, int? commandTimeout)
            : base(connectionString)
        {
            CommandTimeout = commandTimeout;
            Source = source;
        }

        public CountQuery(string source, IDbConnection connection)
            : this(source, connection, null)
        {

        }

        public CountQuery(string source, IDbConnection connection, int? commandTimeout)
            : base(connection)
        {
            CommandTimeout = commandTimeout;
            Source = source;
        }

        public int Execute(object queryParameters = null, IDbTransaction transaction = null)
        {
            return Query<int>(GenerateStatement(), queryParameters, transaction: transaction).First();
        }

        public async Task<int> ExecuteAsync(object queryParameters = null, IDbTransaction transaction = null)
        {
            var results = await QueryAsync<int>(GenerateStatement(), queryParameters, transaction: transaction);
            return results.First();
        }

        public string GenerateStatement()
        {
            string filter = string.Join(" AND ", Filters);

            string sql = defaultQueryTemplate
                .Replace("{source}", Source)
                .Replace("{filter}", string.IsNullOrWhiteSpace(filter) ? "" : "WHERE " + filter)
                .Replace("  ", "");

            Debug.WriteLine(sql);

            return sql;
        }

        public virtual IDapperCountQueryBuilder Where(string filter)
        {
            if (string.IsNullOrWhiteSpace(filter))
            {
                throw new ArgumentNullException(nameof(filter));
            }

            Filters.Add(filter);
            return this;
        }
    }
}
