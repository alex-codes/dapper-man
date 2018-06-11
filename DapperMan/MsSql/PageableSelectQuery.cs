using DapperMan.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DapperMan.MsSql
{
    public class PageableSelectQuery : SelectQuery
    {
        private string defaultQueryTemplate = "SELECT * FROM {source} {filter} {sort} OFFSET {offset} ROWS FETCH NEXT {pageSize} ROWS ONLY;";
        protected int Skip { get; set; }
        protected int Take { get; set; }

        public PageableSelectQuery(string source, string connectionString)
            : this(source, connectionString, null)
        {

        }

        public PageableSelectQuery(string source, string connectionString, int? commandTimeout)
            : base(source, connectionString, commandTimeout)
        {

        }

        public PageableSelectQuery(string source, IDbConnection connection)
            : this(source, connection, null)
        {

        }

        public PageableSelectQuery(string source, IDbConnection connection, int? commandTimeout)
            : base(source, connection, commandTimeout)
        {

        }

        internal PageableSelectQuery(string source, string connectionString, IDbConnection connection, int? commandTimeout)
            : base(source, connectionString, commandTimeout)
        {
            Connection = connection;
        }

        public static new ISelectQueryBuilder Create(string source, string connectionString)
        {
            return Create(source, connectionString, null);
        }

        public static new ISelectQueryBuilder Create(string source, string connectionString, int? commandTimeout)
        {
            return new PageableSelectQuery(source, connectionString, commandTimeout);
        }

        public static new ISelectQueryBuilder Create(string source, IDbConnection connection)
        {
            return Create(source, connection, null);
        }

        public static new ISelectQueryBuilder Create(string source, IDbConnection connection, int? commandTimeout)
        {
            return new PageableSelectQuery(source, connection, commandTimeout);
        }

        public override (IEnumerable<T> Results, int? TotalRows) Execute<T>(object queryParameters = null, IDbTransaction transaction = null)
        {
            var gridReader = QueryMultiple(GenerateStatement(), queryParameters, transaction: transaction);
            var results = gridReader.Read<T>();
            int totalRows = gridReader.Read<int>().First();

            return (results, totalRows);
        }

        public override async Task<(IEnumerable<T> Results, int? TotalRows)> ExecuteAsync<T>(object queryParameters = null, IDbTransaction transaction = null)
        {
            var gridReader = await QueryMultipleAsync(GenerateStatement(), queryParameters, transaction: transaction);
            var results = gridReader.Read<T>();
            int totalRows = gridReader.Read<int>().First();

            return (results, totalRows);
        }

        public override string GenerateStatement()
        {
            string filter = string.Join(" AND ", Filters);
            string sort = string.Join(", ", SortOrders);
            int offset = Skip;
            int pageSize = Take;

            string sql = this.defaultQueryTemplate
                .Replace("{source}", Source)
                .Replace("{filter}", string.IsNullOrWhiteSpace(filter) ? "" : "WHERE " + filter)
                .Replace("{sort}", string.IsNullOrWhiteSpace(sort) ? "" : "WHERE " + sort)
                .Replace("{offset}", offset.ToString())
                .Replace("{pageSize}", pageSize.ToString());

            string rowCountSql = sql
                .Replace("SELECT *", "SELECT COUNT(*)");

            sql += rowCountSql;

            Debug.WriteLine(sql);

            return sql;
        }

        public override ISelectQueryBuilder SkipTake(int skip, int take)
        {
            if (skip < 0 || take < 0)
            {
                throw new ArgumentException("Cannot skip/take less than 0 records");
            }

            Skip = skip;
            Take = take;

            return this;
        }
    }
}
