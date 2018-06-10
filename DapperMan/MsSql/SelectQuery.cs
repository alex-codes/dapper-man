using DapperMan.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DapperMan.MsSql
{
    public class SelectQuery : DapperQueryBase, ISelectQueryBuilder, IDapperQueryGenerator
    {
        private int? commandTimeout = null;
        private readonly string defaultQueryTemplate = "SELECT * FROM {source} {filter} {sort};";
        private List<string> filters = new List<string>();
        private List<string> sortOrders = new List<string>();
        private readonly string source;

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
            this.commandTimeout = commandTimeout;
            this.source = source;
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
            this.commandTimeout = commandTimeout;
            this.source = source;
        }

        public static ISelectQueryBuilder Create(string source, string connectionString)
        {
            return Create(source, connectionString, null);
        }

        public static ISelectQueryBuilder Create(string source, string connectionString, int? commandTimeout)
        {
            return new SelectQuery(source, connectionString, commandTimeout);
        }

        public static ISelectQueryBuilder Create(string source, IDbConnection connection)
        {
            return Create(source, connection, null);
        }

        public static ISelectQueryBuilder Create(string source, IDbConnection connection, int? commandTimeout)
        {
            return new SelectQuery(source, connection, commandTimeout);
        }

        public virtual (IEnumerable<T> Results, int? TotalRows) Execute<T>(object queryParameters = null, IDbTransaction transaction = null) where T : class
        {
            var results = Query<T>(GenerateStatement(), queryParameters, transaction: transaction, commandTimeout: commandTimeout);
            return (results, null);
        }

        public virtual async Task<(IEnumerable<T> Results, int? TotalRows)> ExecuteAsync<T>(object queryParameters = null, IDbTransaction transaction = null) where T : class
        {
            var results = await QueryAsync<T>(GenerateStatement(), queryParameters, transaction: transaction, commandTimeout: commandTimeout);
            return (results, null);
        }

        public virtual string GenerateStatement()
        {
            string filter = string.Join(" AND ", filters);
            string sort = string.Join(", ", sortOrders);

            string sql = this.defaultQueryTemplate
                .Replace("{source}", source)
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

            sortOrders.Add(orderBy);
            return this;
        }

        public virtual ISelectQueryBuilder SkipTake(int skip, int take)
        {
            var query = new PageableSelectQuery(source, ConnectionString, Connection, commandTimeout);

            foreach (string filter in filters)
            {
                query.Where(filter);
            }

            foreach (string sort in sortOrders)
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

            filters.Add(filter);
            return this;
        }
    }
}
