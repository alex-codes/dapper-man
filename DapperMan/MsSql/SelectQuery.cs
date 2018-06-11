using DapperMan.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DapperMan.MsSql
{
    public class SelectQuery : DapperQueryBase, ISelectQueryBuilder, IDapperQueryGenerator
    {
        private readonly string defaultQueryTemplate = "SELECT * FROM {source} {filter} {sort};";
        protected List<string> Filters { get; private set; } = new List<string>();
        protected List<string> SortOrders { get; private set; } = new List<string>();

        /// <summary>
        /// Creates a new select query
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connectionString">The name of the connection strin.</param>
        public SelectQuery(string source, string connectionString)
            : this(source, connectionString, null)
        {

        }

        public SelectQuery(string source, string connectionString, int? commandTimeout)
            : base(connectionString)
        {
            CommandTimeout = commandTimeout;
            Source = source;
        }

        /// <summary>
        /// Creates a new select query
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connection">A connection to the database.</param>
        public SelectQuery(string source, IDbConnection connection)
            : this(source, connection, null)
        {

        }

        public SelectQuery(string source, IDbConnection connection, int? commandTimeout)
            : base(connection)
        {
            CommandTimeout = commandTimeout;
            Source = source;
        }

        public virtual (IEnumerable<T> Results, int? TotalRows) Execute<T>(object queryParameters = null, IDbTransaction transaction = null) where T : class
        {
            var results = Query<T>(GenerateStatement(), queryParameters, transaction: transaction);
            return (results, results.Count());
        }

        public virtual async Task<(IEnumerable<T> Results, int? TotalRows)> ExecuteAsync<T>(object queryParameters = null, IDbTransaction transaction = null) where T : class
        {
            var results = await QueryAsync<T>(GenerateStatement(), queryParameters, transaction: transaction);
            return (results, results.Count());
        }

        public virtual string GenerateStatement()
        {
            string filter = string.Join(" AND ", Filters);
            string sort = string.Join(", ", SortOrders);

            string sql = defaultQueryTemplate
                .Replace("{source}", Source)
                .Replace("{filter}", string.IsNullOrWhiteSpace(filter) ? "" : "WHERE " + filter)
                .Replace("{sort}", string.IsNullOrWhiteSpace(sort) ? "" : "WHERE " + sort);

            Debug.WriteLine(sql);

            return sql;
        }

        public virtual ISelectQueryBuilder OrderBy(string orderBy)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
            {
                throw new ArgumentNullException(nameof(orderBy));
            }

            SortOrders.Add(orderBy);
            return this;
        }

        public virtual ISelectQueryBuilder SkipTake(int skip, int take)
        {
            var query = new PageableSelectQuery(Source, ConnectionString, Connection, CommandTimeout);

            foreach (string filter in Filters)
            {
                query.Where(filter);
            }

            foreach (string sort in SortOrders)
            {
                query.OrderBy(sort);
            }

            return query.SkipTake(skip, take);
        }

        public virtual ISelectQueryBuilder Where(string filter)
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
