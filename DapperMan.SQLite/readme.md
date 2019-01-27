
# DapperMan.SQLite

Query extensions for SQLite. See the main project's readme for usage.

## SQLite Connections

Because SQLite recommends that you create the database connection once and reuse it throughout the lifetime if your application, there are no constructors that will create an `SQLiteConnection` objct for you. You must provide create, close and dispose the `IDbConnection` instance. The most convenient way to do this is to implement `IDisposable`.

```
class SQLiteHelper : IDisposable
{
    private IDbConnection connection;

    public SQLiteHelper(string pathToDb)
    {
        connection = new SQLiteconnection(pathToDb);
        connection.Open();
    }

    void Insert(Department dept)
    {
        DapperQuery.Insert("Department", connection)
            .Execute<Department>(dept);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this); 
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            connection.Close();
            connection.Dispose();
        }
    }
}
```


## System.Data.SQLite vs Microsoft.Data.SQLite

The package has a dependency on `System.Data.SQLite`, but you should be able to use `Microsoft.Data.SQLite` or any other SQLite provider that you prefer because each query constructor accepts an `IDbConnection` parameter. This has not been tested but it should work.