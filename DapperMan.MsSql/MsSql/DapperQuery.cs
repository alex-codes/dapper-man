using DapperMan.Core;
using System.Data;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DapperMan.Tests")]
namespace DapperMan.MsSql
{
    /// <summary>
    /// A basic query abstraction.
    /// </summary>
    public class DapperQuery : MsSqlQueryBase
    {
        /// <summary>
        /// Creates a new instance of DapperQuery.
        /// </summary>
        /// <param name="connectionString">The connection string to the database.</param>
        public DapperQuery(string connectionString)
            : base(null, connectionString)
        {

        }

        /// <summary>
        /// Creates a new instance of DapperQuery.
        /// </summary>
        /// <param name="connection">A connection to the database. The connection is NOT closed upon completion of the query.</param>
        public DapperQuery(IDbConnection connection)
            : base(null, connection)
        {

        }


        // static factory methods for a cleaner builder pattern


        /// <summary>
        /// Creates a new query. You provide the sql.
        /// </summary>
        /// <param name="connectionString">The connection string to the database.</param>
        /// <returns></returns>
        public static DapperQuery Create(string connectionString)
        {
            return new DapperQuery(connectionString);
        }

        /// <summary>
        /// Creates a new query. You provide the sql.
        /// </summary>
        /// <param name="connection">A connection to the database. The connection is NOT closed upon completion of the query.</param>
        /// <returns></returns>
        public static DapperQuery Create(IDbConnection connection)
        {
            return new DapperQuery(connection);
        }

        /// <summary>
        /// Build a query to count rows in a table.
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connectionString">The connection string to the database.</param>
        /// <returns>
        /// A new CountQuery.
        /// </returns>
        public static ICountQueryBuilder Count(string source, string connectionString)
        {
            return new CountQuery(source, connectionString);
        }

        /// <summary>
        /// Build a query to count rows in a table.
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connectionString">The connection string to the database.</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        /// <returns>
        /// A new CountQuery.
        /// </returns>
        public static ICountQueryBuilder Count(string source, string connectionString, int? commandTimeout)
        {
            return new CountQuery(source, connectionString, commandTimeout);
        }

        /// <summary>
        /// Build a query to count rows in a table.
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connection">A connection to the database. The connection is NOT closed upon completion of the query.</param>
        /// <returns>
        /// A new CountQuery.
        /// </returns>
        public static ICountQueryBuilder Count(string source, IDbConnection connection)
        {
            return new CountQuery(source, connection);
        }

        /// <summary>
        /// Build a query to count rows in a table.
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connection">A connection to the database. The connection is NOT closed upon completion of the query.</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        /// <returns>
        /// A new CountQuery.
        /// </returns>
        public static ICountQueryBuilder Count(string source, IDbConnection connection, int? commandTimeout)
        {
            return new CountQuery(source, connection, commandTimeout);
        }

        /// <summary>
        /// Builds a query to delete data from a table.
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connectionString">The connection string to the database.</param>
        /// <returns>
        /// A new DeleteQuery.
        /// </returns>
        public static IDeleteQueryBuilder Delete(string source, string connectionString)
        {
            return new DeleteQuery(source, connectionString);
        }

        /// <summary>
        /// Builds a query to delete data from a table.
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connectionString">The connection string to the database.</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        /// <returns>
        /// A new DeleteQuery.
        /// </returns>
        public static IDeleteQueryBuilder Delete(string source, string connectionString, int? commandTimeout)
        {
            return new DeleteQuery(source, connectionString, commandTimeout);
        }

        /// <summary>
        /// Builds a query to delete data from a table.
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connection">A connection to the database. The connection is NOT closed upon completion of the query.</param>
        /// <returns>
        /// A new DeleteQuery.
        /// </returns>
        public static IDeleteQueryBuilder Delete(string source, IDbConnection connection)
        {
            return new DeleteQuery(source, connection);
        }

        /// <summary>
        /// Builds a query to delete data from a table.
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connection">A connection to the database. The connection is NOT closed upon completion of the query.</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        /// <returns>
        /// A new DeleteQuery.
        /// </returns>
        public static IDeleteQueryBuilder Delete(string source, IDbConnection connection, int? commandTimeout)
        {
            return new DeleteQuery(source, connection, commandTimeout);
        }

        /// <summary>
        /// Builds a query to determine if a record exists.
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connectionString">The connection string to the database.</param>
        /// <returns>
        /// A new ExistsQuery.
        /// </returns>
        public static IExistsQueryBuilder Exists(string source, string connectionString)
        {
            return new ExistsQuery(source, connectionString);
        }

        /// <summary>
        /// Builds a query to determine if a record exists.
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connectionString">The connection string to the database.</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        /// <returns>
        /// A new ExistsQuery.
        /// </returns>
        public static IExistsQueryBuilder Exists(string source, string connectionString, int? commandTimeout)
        {
            return new ExistsQuery(source, connectionString, commandTimeout);
        }

        /// <summary>
        /// Builds a query to determine if a record exists.
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connection">A connection to the database. The connection is NOT closed upon completion of the query.</param>
        /// <returns>
        /// A new ExistsQuery.
        /// </returns>
        public static IExistsQueryBuilder Exists(string source, IDbConnection connection)
        {
            return new ExistsQuery(source, connection);
        }

        /// <summary>
        /// Builds a query to determine if a record exists.
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connection">A connection to the database. The connection is NOT closed upon completion of the query.</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        /// <returns>
        /// A new ExistsQuery.
        /// </returns>
        public static IExistsQueryBuilder Exists(string source, IDbConnection connection, int? commandTimeout)
        {
            return new ExistsQuery(source, connection, commandTimeout);
        }

        /// <summary>
        /// Build a query to select a single row from a table.
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connectionString">The connection string to the database.</param>
        /// <returns>
        /// A new FindQuery.
        /// </returns>
        public static IFindQueryBuilder Find(string source, string connectionString)
        {
            return new FindQuery(source, connectionString);
        }

        /// <summary>
        /// Build a query to select a single row from a table.
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connectionString">The connection string to the database.</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        /// <returns>
        /// A new FindQuery.
        /// </returns>
        public static IFindQueryBuilder Find(string source, string connectionString, int? commandTimeout)
        {
            return new FindQuery(source, connectionString, commandTimeout);
        }

        /// <summary>
        /// Build a query to select a single row from a table.
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connection">A connection to the database. The connection is NOT closed upon completion of the query.</param>
        /// <returns>
        /// A new FindQuery.
        /// </returns>
        public static IFindQueryBuilder Find(string source, IDbConnection connection)
        {
            return new FindQuery(source, connection);
        }

        /// <summary>
        /// Build a query to select a single row from a table.
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connection">A connection to the database. The connection is NOT closed upon completion of the query.</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        /// <returns>
        /// A new FindQuery.
        /// </returns>
        public static IFindQueryBuilder Find(string source, IDbConnection connection, int? commandTimeout)
        {
            return new FindQuery(source, connection, commandTimeout);
        }

        /// <summary>
        /// Build a query to insert data into a table.
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connectionString">The connection string to the database.</param>
        /// <returns>
        /// A new InsertQuery.
        /// </returns>
        public static IInsertQueryBuilder Insert(string source, string connectionString)
        {
            return new InsertQuery(source, connectionString);
        }

        /// <summary>
        /// Build a query to insert data into a table.
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connectionString">The connection string to the database.</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        /// <returns>
        /// A new InsertQuery.
        /// </returns>
        public static IInsertQueryBuilder Insert(string source, string connectionString, int? commandTimeout)
        {
            return new InsertQuery(source, connectionString, commandTimeout);
        }

        /// <summary>
        /// Build a query to insert data into a table.
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connection">A connection to the database. The connection is NOT closed upon completion of the query.</param>
        /// <returns>
        /// A new InsertQuery.
        /// </returns>
        public static IInsertQueryBuilder Insert(string source, IDbConnection connection)
        {
            return new InsertQuery(source, connection);
        }

        /// <summary>
        /// Build a query to insert data into a table.
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connection">A connection to the database. The connection is NOT closed upon completion of the query.</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        /// <returns>
        /// A new InsertQuery.
        /// </returns>
        public static IInsertQueryBuilder Insert(string source, IDbConnection connection, int? commandTimeout)
        {
            return new InsertQuery(source, connection, commandTimeout);
        }

        /// <summary>
        /// Build a query to select a single page of data from a table.
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connectionString">The connection string to the database.</param>
        /// <returns>
        /// A new PageableSelectQuery.
        /// </returns>
        public static ISelectQueryBuilder PageableSelect(string source, string connectionString)
        {
            return new PageableSelectQuery(source, connectionString);
        }

        /// <summary>
        /// Build a query to select a single page of data from a table.
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connectionString">The connection string to the database.</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        /// <returns>
        /// A new PageableSelectQuery.
        /// </returns>
        public static ISelectQueryBuilder PageableSelect(string source, string connectionString, int? commandTimeout)
        {
            return new PageableSelectQuery(source, connectionString, commandTimeout);
        }

        /// <summary>
        /// Build a query to select a single page of data from a table.
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connection">A connection to the database. The connection is NOT closed upon completion of the query.</param>
        /// <returns>
        /// A new PageableSelectQuery.
        /// </returns>
        public static ISelectQueryBuilder PageableSelect(string source, IDbConnection connection)
        {
            return new PageableSelectQuery(source, connection);
        }

        /// <summary>
        /// Build a query to select a single page of data from a table.
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connection">A connection to the database. The connection is NOT closed upon completion of the query.</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        /// <returns>
        /// A new PageableSelectQuery.
        /// </returns>
        public static ISelectQueryBuilder PageableSelect(string source, IDbConnection connection, int? commandTimeout)
        {
            return new PageableSelectQuery(source, connection, commandTimeout);
        }

        /// <summary>
        /// Build a query to select data from a table.
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connectionString">The connection string to the database.</param>
        /// <returns>
        /// A new SelectQuery.
        /// </returns>
        public static ISelectQueryBuilder Select(string source, string connectionString)
        {
            return new SelectQuery(source, connectionString);
        }

        /// <summary>
        /// Build a query to select data from a table.
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connectionString">The connection string to the database.</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        /// <returns>
        /// A new SelectQuery.
        /// </returns>
        public static ISelectQueryBuilder Select(string source, string connectionString, int? commandTimeout)
        {
            return new SelectQuery(source, connectionString, commandTimeout);
        }

        /// <summary>
        /// Build a query to select data from a table.
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connection">A connection to the database. The connection is NOT closed upon completion of the query.</param>
        /// <returns>
        /// A new SelectQuery.
        /// </returns>
        public static ISelectQueryBuilder Select(string source, IDbConnection connection)
        {
            return new SelectQuery(source, connection);
        }

        /// <summary>
        /// Build a query to select data from a table.
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connection">A connection to the database. The connection is NOT closed upon completion of the query.</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        /// <returns>
        /// A new SelectQuery.
        /// </returns>
        public static ISelectQueryBuilder Select(string source, IDbConnection connection, int? commandTimeout)
        {
            return new SelectQuery(source, connection, commandTimeout);
        }

        /// <summary>
        /// Run a stored procedure.
        /// </summary>
        /// <param name="storedProcedureName"></param>
        /// <param name="connectionString">The connection string to the database.</param>
        /// <returns>
        /// A new StoredProcedureQuery.
        /// </returns>
        public static IStoredProcedureQueryBuilder StoredProcedure(string storedProcedureName, string connectionString)
        {
            return new StoredProcedureQuery(storedProcedureName, connectionString);
        }

        /// <summary>
        /// Run a stored procedure.
        /// </summary>
        /// <param name="storedProcedureName"></param>
        /// <param name="connectionString">The connection string to the database.</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        /// <returns>
        /// A new StoredProcedureQuery.
        /// </returns>
        public static IStoredProcedureQueryBuilder StoredProcedure(string storedProcedureName, string connectionString, int? commandTimeout)
        {
            return new StoredProcedureQuery(storedProcedureName, connectionString, commandTimeout);
        }

        /// <summary>
        /// Run a stored procedure.
        /// </summary>
        /// <param name="storedProcedureName"></param>
        /// <param name="connection">A connection to the database. The connection is NOT closed upon completion of the query.</param>
        /// <returns>
        /// A new StoredProcedureQuery.
        /// </returns>
        public static IStoredProcedureQueryBuilder StoredProcedure(string storedProcedureName, IDbConnection connection)
        {
            return new StoredProcedureQuery(storedProcedureName, connection);
        }

        /// <summary>
        /// Run a stored procedure.
        /// </summary>
        /// <param name="storedProcedureName"></param>
        /// <param name="connection">A connection to the database. The connection is NOT closed upon completion of the query.</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        /// <returns>
        /// A new StoredProcedureQuery.
        /// </returns>
        public static IStoredProcedureQueryBuilder StoredProcedure(string storedProcedureName, IDbConnection connection, int? commandTimeout)
        {
            return new StoredProcedureQuery(storedProcedureName, connection, commandTimeout);
        }

        /// <summary>
        /// Build a query to update data.
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connectionString">The connection string to the database.</param>
        /// <returns>
        /// A new UpdateQuery.
        /// </returns>
        public static IUpdateQueryBuilder Update(string source, string connectionString)
        {
            return new UpdateQuery(source, connectionString);
        }

        /// <summary>
        /// Build a query to update data.
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connectionString">The connection string to the database.</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        /// <returns>
        /// A new UpdateQuery.
        /// </returns>
        public static IUpdateQueryBuilder Update(string source, string connectionString, int? commandTimeout)
        {
            return new UpdateQuery(source, connectionString, commandTimeout);
        }

        /// <summary>
        /// Build a query to update data.
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connection">A connection to the database. The connection is NOT closed upon completion of the query.</param>
        /// <returns>
        /// A new UpdateQuery.
        /// </returns>
        public static IUpdateQueryBuilder Update(string source, IDbConnection connection)
        {
            return new UpdateQuery(source, connection);
        }

        /// <summary>
        /// Build a query to update data.
        /// </summary>
        /// <param name="source">The name and schema of the table.</param>
        /// <param name="connection">A connection to the database. The connection is NOT closed upon completion of the query.</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        /// <returns>
        /// A new UpdateQuery.
        /// </returns>
        public static IUpdateQueryBuilder Update(string source, IDbConnection connection, int? commandTimeout)
        {
            return new UpdateQuery(source, connection, commandTimeout);
        }
    }
}
