using DapperMan.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DapperMan.MsSql
{
    public class DeleteQuery : DapperQueryBase, IDeleteQueryBuilder, IDapperQueryGenerator
    {
        private int? commandTimeout = null;
        private readonly string defaultQueryTemplate = "DELETE FROM {source} {filter};";
        protected List<string> Filters { get; private set; } = new List<string>();

        /// <summary>
        /// Creates a new delete query
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connectionString">The name of the connection strin.</param>
        public DeleteQuery(string source, string connectionString)
            : this(source, connectionString, null)
        {

        }

        public DeleteQuery(string source, string connectionString, int? commandTimeout)
            : base(connectionString)
        {
            this.commandTimeout = commandTimeout;
            Source = source;
        }

        /// <summary>
        /// Creates a new delete query
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connection">A connection to the database.</param>
        public DeleteQuery(string source, IDbConnection connection)
            : this(source, connection, null)
        {

        }

        public DeleteQuery(string source, IDbConnection connection, int? commandTimeout)
            : base(connection)
        {
            this.commandTimeout = commandTimeout;
            Source = source;
        }

        public virtual int Execute(object queryParameters = null, IDbTransaction transaction = null)
        {
            return Execute(GenerateStatement(), queryParameters, transaction: transaction, commandTimeout: commandTimeout);
        }

        public virtual async Task<int> ExecuteAsync(object queryParameters = null, IDbTransaction transaction = null)
        {
            return await ExecuteAsync(GenerateStatement(), queryParameters, transaction: transaction, commandTimeout: commandTimeout);
        }

        public virtual string GenerateStatement()
        {
            string filter = string.Join(" AND ", Filters);

            string sql = this.defaultQueryTemplate
                .Replace("{source}", Source)
                .Replace("{filter}", string.IsNullOrWhiteSpace(filter) ? "" : "WHERE " + filter);

            Debug.WriteLine(sql);

            return sql;
        }

        public virtual IDeleteQueryBuilder Where(string filter)
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
