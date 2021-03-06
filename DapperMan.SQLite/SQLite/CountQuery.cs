﻿using DapperMan.Core;
using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DapperMan.SQLite
{
    /// <summary>
    /// Build a query to count rows in a table.
    /// </summary>
    public class CountQuery : SQLiteQueryBase, ICountQueryBuilder, IQueryGenerator
    {
        private string defaultQueryTemplate = "SELECT COUNT(*) as [Count] FROM {source} {filter};";

        /// <summary>
        /// Creates a new count query.
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connection">A connection to the database. The connection is NOT closed upon completion of the query.</param>
        public CountQuery(string source, IDbConnection connection)
            : this(source, connection, null)
        {
        }

        /// <summary>
        /// Creates a new count query.
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connection">A connection to the database. The connection is NOT closed upon completion of the query.</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        public CountQuery(string source, IDbConnection connection, int? commandTimeout)
            : base(source, connection, commandTimeout)
        {
        }

        /// <summary>
        /// Executes the query.
        /// </summary>
        /// <param name="queryParameters">Parameters to pass to the statement.</param>
        /// <param name="transaction">An active database transaction used for rollbacks.</param>
        /// <returns>
        /// The count of rows that match the criteria.
        /// </returns>
        public virtual int Execute(object queryParameters = null, IDbTransaction transaction = null)
        {
            return Query<int>(GenerateStatement(), queryParameters, transaction: transaction).First();
        }

        /// <summary>
        /// Executes the query.
        /// </summary>
        /// <param name="queryParameters">Parameters to pass to the statement.</param>
        /// <param name="transaction">An active database transaction used for rollbacks.</param>
        /// <returns>
        /// The count of rows that match the criteria.
        /// </returns>
        public virtual async Task<int> ExecuteAsync(object queryParameters = null, IDbTransaction transaction = null)
        {
            var results = await QueryAsync<int>(GenerateStatement(), queryParameters, transaction: transaction);
            return results.First();
        }

        /// <summary>
        /// Generates the sql statement to be executed.
        /// </summary>
        /// <returns>
        /// The completed sql statement to be executed.
        /// </returns>
        public virtual string GenerateStatement()
        {
            if (string.IsNullOrWhiteSpace(Source))
            {
                throw new ArgumentNullException(nameof(Source));
            }

            string filter = string.Join(" AND ", Filters);

            string sql = defaultQueryTemplate
                .Replace("{source}", Source)
                .Replace("{filter}", string.IsNullOrWhiteSpace(filter) ? "" : "WHERE " + filter)
                .TrimEmptySpace();

            Debug.WriteLine(sql);

            return sql;
        }

        /// <summary>
        /// Adds a filter to the query.
        /// </summary>
        /// <param name="filter">The filter string to add to the query.</param>
        /// <returns>
        /// The ICountQueryBuilder instance.
        /// </returns>
        public virtual ICountQueryBuilder Where(string filter)
        {
            AddFilter(filter);
            return this;
        }
    }
}
