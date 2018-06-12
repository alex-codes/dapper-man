using Dapper;
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
        private string defaultCountQueryTemplate = "SELECT COUNT(*) FROM {source} {filter};";
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

        public override (IEnumerable<T> Results, int TotalRows) Execute<T>(object queryParameters = null, IDbTransaction transaction = null)
        {
            IEnumerable<T> results = null;
            int totalRows = 0;

            void mapper(SqlMapper.GridReader gridReader)
            {
                results = gridReader.Read<T>();
                totalRows = gridReader.Read<int>().First();
            }

            QueryMultiple(GenerateStatement(), mapper, queryParameters, transaction: transaction);

            return (results, totalRows);
        }

        public override async Task<(IEnumerable<T> Results, int TotalRows)> ExecuteAsync<T>(object queryParameters = null, IDbTransaction transaction = null)
        {
            IEnumerable<T> results = null;
            int totalRows = 0;

            void mapper(SqlMapper.GridReader gridReader)
            {
                results = gridReader.Read<T>();
                totalRows = gridReader.Read<int>().First();
            }

            await QueryMultipleAsync(GenerateStatement(), mapper, queryParameters, transaction: transaction);

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
                .Replace("{sort}", string.IsNullOrWhiteSpace(sort) ? "" : "ORDER BY " + sort)
                .Replace("{offset}", offset.ToString())
                .Replace("{pageSize}", pageSize.ToString())
                .Replace("  ", "");

            string rowCountSql = defaultCountQueryTemplate
                .Replace("{source}", Source)
                .Replace("{filter}", string.IsNullOrWhiteSpace(filter) ? "" : "WHERE " + filter)
                .Replace("  ", "");

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
