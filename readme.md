# DapperMan

A simple set of wrappers for [Dapper](https://github.com/StackExchange/Dapper) to generate small SQL statements. Similar in concept to what you'll find in Dapper.Contrib, but with a consistent API and extra features.


## Select

This generates a simple select statement. The result is an `IEnumerable<T>`, and `TotalRows` represents the length of the results.

```
(IEnumerable<Department> depts, int totalRows) = DapperQuery.Select("HumanResources.Department", connectionString)
    .Where("GroupName = @groupName")
    .Where("DepartmentName != @excludeDept")
    .OrderBy("GroupName")
    .OrderBy("DepartmentName")
    .Execute<Department>(new { groupName = "foo", excludeDept = "bar" });
```

You can also write your `Where` and `OrderBy` statements like this if you prefer:

```
    .Where("GroupName = @groupName AND DepartmentName != @excludeDept)
    .OrderBy("GroupName, DepartmentName")
```

Both will generate the same SQL (formatted for readability)

```
SELECT *
FROM HumanResources.Department
WHERE GroupName = @groupName
    AND DepartmentName != @excludeDept
ORDER BY GroupName, DepartmentName
```

You can also select all rows by excluding the `Where`:

```
(IEnumerable<Department> depts, int totalRows) = DapperQuery.Select("HumanResources.Department", connectionString)
    .OrderBy("DepartmentName")
    .Execute<Department>();
```


## Paging

This will generate a statement that gives you a specific page of data. You must provide an `OrderBy` in order for paging to function. `TotalRows` for a paging operation is the total number of records that match the filter, not the total number of records in the data set.

```
(var results, int totalRows) = DapperQuery.Select("HumanResources.Department", connectionString)
    .Where("GroupName != @excludeGroup")
    .OrderBy("Name")
    .SkipTake(5, 5)
    .Execute<Department>(new { excludeGroup = "Executive General and Administration" });
```

In the above, the first 5 rows are skipped, and then a page of 5 results is taken.

You can also use the `PageableSelect` factory method. This functions exactly the same as above:

```
(var results, int totalRows) = DapperQuery.PageableSelect("HumanResources.Department", connectionString)
    .Where("GroupName != @excludeGroup")
    .OrderBy("Name")
    .SkipTake(5, 5)
    .Execute<Department>(new { excludeGroup = "Executive General and Administration" });
```


## Counting

You can generate a simple count statement:

```
int count = DapperQuery.Count("HumanResources.Department", connectionString)
    .Where("GroupName = @name")
    .Execute(new { name = "Executive General and Administration" });
```


## Insert

Because insert uses reflection to determine how to generate the SQL statement, if your ID column is an auto-incrementing identity field, you'll want to decorate your model with either a `[Identity]` or `[InsertIgnore]` attribute. Either will have the same effect. If your field is marked as `[Identity]`, when the insert statement completes, you will receive the inserted scope identity. You can also place an `[Identity]` on other columns to ensure they are never inserted (e.g. place it on a computed column).

If you do not have an identity column, you do not need to use the `[Identity]` attribute.

```
public class Department
{
    [Identity]
    public int Id { get; set; }
    ...
}

int id = DapperQuery.Insert("HumanResources.Department", connectionString)
    .Execute<Department>(new Department());
```

You can also insert a list of objects:

```
var depts = new List<Department>();
depts.Add(); // add departments

DapperQuery.Insert("HumanResources.Department", connectionString)
    .Execute<Department>(depts);
```


## Update

Because update uses reflection to determine how to generate the SQL statement, if your ID column is an auto-incrementing identity field, you'll want to decorate your model with either a `[Identity]` or `[UpdateIgnore]` attribute. Either will have the same effect. You can also place an `[UpdateIgnore]` on other columns to ensure they are never updated (e.g. place it on a computed column).

A field can be decorated with both an `[InsertIgnore]` and `[UpdateIgnore]` and it will have a similar effect as a single `[Identity]` attribute.

If you do not have an identity column, you do not need to use the `[Identity]` attribute.

```
int rowsUpdated = DapperQuery.Update("HumanResources.Department", connectionString)
    .Where("DepartmentId = @departmentId")
    .Execute<Department>(
        new Department()
        {
            DepartmentId = 18,
            Name = "Testing123-x",
            GroupName = "Testing",
            ModifiedDate = DateTime.Now
        }
    });
```

You can also update a list of objects:

```
var depts = new List<Department>();
depts.Add(); // add departments

DapperQuery.Update("HumanResources.Department", connectionString)
    .Where("DepartmentId = @departmentId")
    .Execute<Department>(depts);
```

If you don't specify a `Where` clause, all records will be updated. In this instance, DepartmentId is an `[Identity]` field, so it will not be updated, but all other fields will be modified:

```
DapperQuery.Update("HumanResources.Department", connectionString)
    .Execute<Department>(
        new Department()
        {
            DepartmentId = 18,
            Name = "Testing123-x",
            GroupName = "Testing",
            ModifiedDate = DateTime.Now
        }
    });
```


## Insert and Update property caching

Insert and update methods allow you to provide an instance of [ObjectCache](https://docs.microsoft.com/en-us/dotnet/framework/performance/caching-in-net-framework-applications) in order to cache property reflection. The `ObjectCache` property is wrapped in a `PropertyCache` object that can be used to configure the cache expiration policy. By default, it uses a 15 minute sliding expiration.

```
// configure PropertyCache for injection elsewhere in your app

public class YourBusinessLayer
{
    public YourBusinessLayer(PropertyCache cache)
    {
        ...
    }

    public void Insert(Department dept)
    {
        dept.DepartmentId = DapperQuery.Insert("HumanResources.Department", connectionString)
            .Execute<Department>(dept, cache);
    }
}
```


## Delete

To delete a record:

```
int rowsDeleted = DapperQuery.Delete("HumanResources.Department", connectionString)
    .Where("DepartmentId = @id")
    .Execute(new { id = 17 });
```

To delete a list of records:

```
var ids = new List<int>();
// add ids

DapperQuery.Delete("HumanResources.Department", connectionString)
    .Where("DepartmentId = @id")
    .Execute(ids);
```

Or if you prefer, this will delete all records:

```
DapperQuery.Delete("HumanResources.Department", connectionString)
    .Execute();
```


## Stored Procedures

To execute a stored procedure:

```
(var results, int rowCount) = await DapperQuery.StoredProcedure("[dbo].[uspGetManagerEmployees]", connectionString)
    .ExecuteAsync<GetManagerEmployeesResult>(new
    {
        BusinessEntityId = 2
    });
```


## Multiple Result Sets

`DapperQueryBase` exposes a `QueryMultiple` function that takes a map parameter which allows you to handle parsing multiple result sets. `PageableSelectQuery` is an example of how to use this:

```
IEnumerable<T> results = null;
int totalRows = 0;

void mapper(SqlMapper.GridReader gridReader)
{
    results = gridReader.Read<T>();
    totalRows = gridReader.Read<int>().First();
}

QueryMultiple(GenerateStatement(), mapper, queryParameters, transaction: transaction);

return (results, totalRows);
```

Here's an alternate example showing how to map the results multiple tables:

```
sql = "
select * from table;
select * from table2;"
select count(*) from table3;

IEnumerable<Table> results = null;
IEnumerable<Table2> results2 = null;
int table3Rows = 0;

void mapper(SqlMapper.GridReader gridReader)
{
    results = gridReader.Read<Table>();
    results2 = gridReader.Read<Table2>();
    table3Rows = gridReader.Read<int>().First();
}

QueryMultiple(sql, mapper);
```


## Async

All execute statements have an async equivalent:

```
// all queries should have a signature similar to this (params removed for brevity)
public (IEnumerable<T> Results, int TotalRows) Execute<T>()
public Task<(IEnumerable<T> Results, int TotalRows)> ExecuteAsync<T>()

//DapperQueryBase signatures (params removed for brevity)
public int Execute()
public Task<int> ExecuteAsync()

public IEnumerable<T> Query<T>()
public Task<IEnumerable<T>> QueryAsync<T>()

public void QueryMultiple()
public Task QueryMultipleAsync()
```


## Construction

All queries have a static factory method on the `DapperQuery` object in order to keep the fluent API a bit cleaner. But each query object also has multiple public constructors so you can compose and inject them if needed. Example:

```
// static factory methods
static ISelectQueryBuilder Select(string source, string connectionString)
static ISelectQueryBuilder Select(string source, string connectionString, int? commandTimeout)
static ISelectQueryBuilder Select(string source, IDbConnection connection)
static ISelectQueryBuilder Select(string source, IDbConnection connection, int? commandTimeout)

// equivalent public constructors
public SelectQuery(string source, string connectionString)
public SelectQuery(string source, string connectionString, int? commandTimeout)
public SelectQuery(string source, IDbConnection connection)
public SelectQuery(string source, IDbConnection connection, int? commandTimeout)
```


## Filtering and Sorting

The `Where` and `OrderBy` allows you to chain clauses in a fluent manner:

```
   .Where("foo = @foo)
   .Where("bar = @bar")
   .Where("baz = @baz")
```

Each clause will be appended with AND. If you want more complicated filtering you can perform it inline and it should work:

```
    .Where("(foo = @foo OR foo = @bar) AND (baz != @baz OR bar NOT IN (select 1))")
```


## How do I _____?

If it isn't explicitly documented here, you probably can't. The goal of this wrapper was to generate sql for 90% of the simple CRUD type operations, not to create a full fledged ORM. If it isn't a simple statement, you can fall back to using Dapper directly. You can also create an instance of the base abstraction in order to keep your code consistent.

```
string sql = "complicated sql statement";

var results = DapperQuery.Create(connectionString)
    .Query<T>(sql, new { your params });


// or, if the sql statement is not a select statement

int count = DapperQuery.Create(connectionString)
    .Execute(sql, new { your params });
```