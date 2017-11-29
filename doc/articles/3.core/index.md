# 3. Core Components

Paradigm ORM contains 4 libraries:

- Core library: This is a DB agnostic layer that contains shared logic and ORM specific code and interfaces
- 4 implementation libraries: Implements core interfaces and provides connection to specific databases
    - MySql
    - PostgreSql
    - SQL Server
    - Cassandra (and ScyllaDB)

Objects and classes covered in this document are implemented for each of the supported databases. The ORM is constructed over `System.Data` interfaces, and uses the particular clients to implement these interfaces. Even if the ORM is designed to use these, and have a similar interface, it uses its own set of interfaces that allows to add sync and async methods for all the objects consistently, even if one of the client does not allows async actions. Now we'll see a brief description of what each object do.

> [!NOTE]
> While we always try to use the official connector for each database, for MySql we had to go in another direction. Oracle's official connector does not truly supports async methods, so we decided to change the Oracle connector and use another implementation called [MySql Connector](https://mysql-net.github.io/MySqlConnector/). This is a new implementation of MySql protocol that fully supports async methods. Also, it's an active repository and the team behind the connector respond fast and efficiently.


## 3.1. Database Connectors

The @Paradigm.ORM.Data.Database.IDatabaseConnector represents a connection to a database.
It also works as factory for database specific objects:
- Commands
- Transactions
- Formatters
- etc.
Each database implementation has its own **Database Connector**:
- MySql: @Paradigm.ORM.Data.MySql.MySqlDatabaseConnector
- PostgreSql: @Paradigm.ORM.Data.PostgreSql.PostgreSqlDatabaseConnector
- SqlServer: @Paradigm.ORM.Data.SqlServer.SqlDatabaseConnector
- Cassandra: @Paradigm.ORM.Data.Cassandra.CqlDatabaseConnector

Connecting to a database using the connector is a trivial operation. All the database classes are designed to share the `System.Data` method conventions, with some minor changes to allow a clear interface for the rest of the engine. The basic idea is to call the constructor and pass the connection string, or to create the instance with the parameterless constructor and then call the Initialize method.

#### Sync Connection
Connecting synchronously:
````csharp

using(var connector = new MySqlDatabaseConnector("Server=localhost;Database=TestDb;Uid=root;Password=*****")
{
    connector.Open();
    // your logic here.
    connector.Close();
}

````
#### Async Connection
Connecting asynchronously:
````csharp

using(var connector = new MySqlDatabaseConnector("Server=localhost;Database=TestDb;Uid=root;Password=*****"))
{
    await connector.OpenAsync();
    // your logic here.
    await connector.CloseAsync();
}

````

#### Dependency Injection

As mentioned before, there are two constructors available: a parameterless constructor, and one that receives the connection string. Internally, the second constructor, calls the [Initialize(string)](xref:Paradigm.ORM.Data.Database.IDatabaseConnector#MiracleDevs_ORM_Data_Database_IDatabaseConnector_Initialize_System_String_) method.
If you are planning on using Dependency Injection, the database connector should be created using the parameterless constructor, so you must call the initialize method by hand:

````csharp
// register the connector.
builder.RegisterScoped<IDatabaseConnector, MySqlDatabaseConnector>();

...

// resolve a database connection and initialize the connection.
// we do not manually dispose the connector because the service provider
// will dispose it for us.
var connector = serviceProvider.GetService<IDatabaseConnector>();
connector.Initialize("Server=localhost;Database=TestDb;Uid=root;Password=*****");
connector.Open();
// your logic here
connector.Close();
````

If you intend to use MiracleDevs Enterprise libraries for an MVC site, note that there is already a Middleware prepared to instantiate and configure a connection for each request. [ADD HERE THE LINK TO THE DOC]


## 3.2. Database Commands
As mentioned before, one of the responsibilities of @Paradigm.ORM.Data.Database.IDatabaseConnector is to create command instances. A command object provides a way to execute instructions on the database.
Like `System.Data`, @Paradigm.ORM.Data.Database.IDatabaseCommand needs to be configured before being executed. You should provide the command text, the instruction that will be executed on the database, and the command type; normal values are `Text` or `StoredProcedure`.
Also, there are 3 different ways in which a command can be executed:
- **Reader:** If you are planning to read data from the database and you need the results.
- **Scalar:** If you are planning to return a scalar value (integer, string, date, etc.).
- **NonQuery:** For non data retrieval operations like inserts, updates, etc.

````csharp
if (!connector.IsOpened())
    connection.Open();

using(var command = connector.CreateCommand())
{
    command.CommandText = "SELECT * FROM `SomeTable`";
    command.CommandType = CommandType.Text;

    using(var reader = command.ExecuteReader())
    {
        // map the results here.
    }
}
````
There are plenty of ways to simplify this operation using extension methods like [this one](xref:Paradigm.ORM.Data.Extensions.DatabaseConnectorExtensions#MiracleDevs_ORM_Data_Extensions_DatabaseConnectorExtensions_ExecuteReader_MiracleDevs_ORM_Data_Database_IDatabaseConnector_System_String_System_Action_MiracleDevs_ORM_Data_Database_IDatabaseReader__).
The ORM also ships with several objects that can greatly simplify the command execution. They are explained in detail in the corresponding section, but here it's a list of some of them:
- @Paradigm.ORM.Data.DatabaseAccess.DatabaseAccess
- @Paradigm.ORM.Data.DatabaseAccess.Generic.DatabaseAccess`1
- @Paradigm.ORM.Data.Querying.Query`1
- @Paradigm.ORM.Data.Querying.CustomQuery`1
- @Paradigm.ORM.Data.StoredProcedures.ScalarStoredProcedure`2
- @Paradigm.ORM.Data.StoredProcedures.NonQueryStoredProcedure`1
- @Paradigm.ORM.Data.StoredProcedures.ReaderStoredProcedure`2
- @Paradigm.ORM.Data.StoredProcedures.ReaderStoredProcedure`3
- @Paradigm.ORM.Data.StoredProcedures.ReaderStoredProcedure`4
- @Paradigm.ORM.Data.StoredProcedures.ReaderStoredProcedure`5
- @Paradigm.ORM.Data.StoredProcedures.ReaderStoredProcedure`6
- @Paradigm.ORM.Data.StoredProcedures.ReaderStoredProcedure`7
- @Paradigm.ORM.Data.StoredProcedures.ReaderStoredProcedure`8
- @Paradigm.ORM.Data.StoredProcedures.ReaderStoredProcedure`9

The same code we saw above, translates to:
````csharp
connector.ExecuteReader("SELECT * FROM `SomeTable`", reader => {
    // map the results here.
});
````

## 3.3. Database Transactions
The connector also allows creation and usage of database transactions. Internally, the connector creates a transaction stack, and will set the transaction for all the commands being executed inside a given scope, so you should just need to worry about creating and disposing the transactions.

````csharp
using(var transaction = connector.CreateTransaction())
{
    connector.ExecuteNonQuery("INSERT INTO [dbo].[Table] ([Value1], [Value2]) VALUES (1, 2)");
    transaction.Commit();
}
````

Rolling back a transaction scope is also possible. You can do it either by executing the methods `transaction.Rollback()` or `await transaction.RollbackAsync()` or letting the transaction be disposed without an explicit commitment. By default, transactions will be rolled back at disposal or connection close if they were not committed. More info can be found [here](https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/local-transactions).

## 3.4. Command Builder Factory
Paradigm ORM provides a way to generate standard CRUD commands automatically, using mapping information. Instead of having to create `SELECT`, `INSERT`, `UPDATE`, `DELETE` sentences, the ORM provides what we call Command Builders. There are 6 standard command builders:
| Name                                                              | Function                               |
|-------------------------------------------------------------------|----------------------------------------|
| @Paradigm.ORM.Data.CommandBuilders.ISelectOneCommandBuilder    | Creates a command that is used to execute a SELECT statement for one specific row. Expects the id values to retrieve the row. |
| @Paradigm.ORM.Data.CommandBuilders.ISelectCommandBuilder       | Creates a command that is used to execute a SELECT statement. |
| @Paradigm.ORM.Data.CommandBuilders.ILastInsertIdCommandBuilder | Creates a command that is used to retrieve the last inserted id. The ORM allows batch operations, and therefore it needs a way to retrieve ids sequentially inside a batch. |
| @Paradigm.ORM.Data.CommandBuilders.IInsertCommandBuilder       | Creates a command that is used to execute an INSERT statement. |
| @Paradigm.ORM.Data.CommandBuilders.IUpdateCommandBuilder       | Creates a command that is used to execute an UPDATE statement. |
| @Paradigm.ORM.Data.CommandBuilders.IDeleteCommandBuilder       | Creates a command that is used to execute a DELETE statement. |

These classes are used internally by the `DatabaseAccess` classes to handle CRUD operations, but they can be accessed by the user in case they are needed. The @Paradigm.ORM.Data.Database.IDatabaseConnector can be used to retrieve the command builder factory for that specific database using [this method](xref:Paradigm.ORM.Data.Database.IDatabaseConnector#MiracleDevs_ORM_Data_Database_IDatabaseConnector_GetCommandBuilderFactory):

```csharp

var descriptor = new TableTypeDescriptor(typeof(Client));
var factory = connector.GetCommandBuilderFactory();

var selectOneCommandBuilder = factory.CreateSelectOneCommandBuilder(descriptor);
var selectCommandBuilder = factory.CreateSelectCommandBuilder(descriptor);
var insertCommandBuilder = factory.CreateInsertCommandBuilder(descriptor);
var updateCommandBuilder = factory.CreateUpdateCommandBuilder(descriptor);
var deleteCommandBuilder = factory.CreateDeleteCommandBuilder(descriptor);
var lastInsertIdCommandBuilder = factory.CreateLastInsertIdCommandBuilder(descriptor);

```

If for some reason you need to use all the commands more than once, you can use @Paradigm.ORM.Data.CommandBuilders.CommandBuilderManager which handles all the command builders for you.

> [!TIP]
> If you plan to use these objects in your implementation, have in mind that each command builder will create a command instance only when instantiated. If you need new commands each time, you'll have to dispose and create new builders. The command builders prevent additional creation and disposal of commands to avoid unnecessary garbage collection. When executing a command builder that requires parameters, the command builder itself will be in charge of setting the correct parameter values.

## 3.5. Schema Provider
The connector also provides a way to retrieve the database schema. The schema can be retrieved partially or in full. Out of the box, the ORM allows you to get:
- [Tables](xref:Paradigm.ORM.Data.Database.Schema.ISchemaProvider#MiracleDevs_ORM_Data_Database_Schema_ISchemaProvider_GetTables_System_String_System_String___)
- [Views](xref:Paradigm.ORM.Data.Database.Schema.ISchemaProvider#MiracleDevs_ORM_Data_Database_Schema_ISchemaProvider_GetViews_System_String_System_String___)
- [Stored Procedures](xref:Paradigm.ORM.Data.Database.Schema.ISchemaProvider#MiracleDevs_ORM_Data_Database_Schema_ISchemaProvider_GetStoredProcedures_System_String_System_String___)
- [Columns](xref:Paradigm.ORM.Data.Database.Schema.ISchemaProvider#MiracleDevs_ORM_Data_Database_Schema_ISchemaProvider_GetColumns_System_String_System_String_)
- [Constraints](xref:Paradigm.ORM.Data.Database.Schema.ISchemaProvider#MiracleDevs_ORM_Data_Database_Schema_ISchemaProvider_GetConstraints_System_String_System_String_)
- [Parameters](xref:Paradigm.ORM.Data.Database.Schema.ISchemaProvider#MiracleDevs_ORM_Data_Database_Schema_ISchemaProvider_GetParameters_System_String_System_String_)

If you need specific schema info, or other database objects like functions, you'll have to create your own queries. If multiple users request more schema retrieval options, we'll consider them for future releases.
````csharp
const string databaseName = "databaseName";
var provider = connector.GetSchemaProvider();
var tables = provider.GetTables(databaseName);
var procedures = provider.GetStoredProcedures(databaseName);

foreach(var table in tables)
{
    var columns = provider.GetColumns(databaseName, table.Name);
    var constraints = provider.GetConstraints(databaseName, table.Name);
}

foreach(var procedure in procedures)
{
    var parameters = provider.GetParameters(databaseName, procedure.Name);
}
````

## 3.6. Command Format Provider
This helper class can be also retrieved from the `IDatabaseConnector` object, and provides a way to properly format strings like table names, column values. Its implementation can vary between databases.

````csharp
var formatProvider = connector.GetCommandFormatProvider();

var columnName = formatProvider.GetEscapedName("ColumnName");
var separator = formatProvider.GetQuerySeparator();
var stringValue = formatProvider.GetColumnValue("some string value", typeof(string));
````

There are currently 3 methods available:

**[GetEscapedName(string)](xref:Paradigm.ORM.Data.CommandBuilders.ICommandFormatProvider#MiracleDevs_ORM_Data_CommandBuilders_ICommandFormatProvider_GetEscapedName_System_String_)**: formats the object name with the proper enclosing characters. For Example:

- Sql Server: `[ColumnName]`
- My Sql: `` `ColumnName` ``
- Postgre Sql: `"ColumnName"`

**[GetQuerySeparator()](xref:Paradigm.ORM.Data.CommandBuilders.ICommandFormatProvider#MiracleDevs_ORM_Data_CommandBuilders_ICommandFormatProvider_GetQuerySeparator)**: returns the query separator string for the database. Currently all the databases use the same separator: `;`.

**[GetColumnValue(Object, Type)](xref:Paradigm.ORM.Data.CommandBuilders.ICommandFormatProvider#MiracleDevs_ORM_Data_CommandBuilders_ICommandFormatProvider_GetColumnValue_System_Object_System_Type_)**: Formats the column value with the proper enclosing type if any. For example:

- int: `4`
- float: `3.45`
- string: `'some string value'`
- date: `'03-12-14'`


## 3.7. Db Type to .NET Type Converter
This class converts from DB types to .NET types and back. The DB type is expressed as a string type like `tinyint` or `varchar`.

````csharp
var converter = connector.GetDbStringTypeConverter();

var netType = converter.GetType("varchar", true);       // netType will be string.
var dbType = convert.GetDbStringType(typeof(DateTime));  // dbType will be datetime.
````

The converter also provides methods to the .NET type from schema objects like parameters or columns. If you need to retrieve the schema and get the proper .NET type, you can use both classes in conjunction:

````csharp
var converter = connector.GetDbStringTypeConverter();
var provider = connector.GetSchemaProvider();

var tables = provider.GetTables(databaseName);

foreach(var table in tables)
{
    var columns = provider.GetColumns(databaseName, table.Name);

    foreach(var column in columns)
    {
        var columnType = converter.GetType(column);
    }
}

````

## 3.8. Db Type Value Range Provider
This class provides [min, max] ranges for db types that have a fixed size, like int or date. Given a DB string type, the provider can return the minimum or maximum value. This class can be used to validate entities before sending them to the database.
````csharp
// Sql Server connector
var provider = connector.GetDbTypeValueRangeProvider();

var min = provider.GetMinValue("int"); // will return -2147483648
var max = provider.GetMaxValue("int"); // will return  2147483647

````