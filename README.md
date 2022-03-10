[![Build Status](https://github.com/MiracleDevs/Paradigm.ORM/workflows/Paradigm%20ORM/badge.svg)](https://github.com/MiracleDevs/Paradigm.ORM/actions)

# Paradigm.ORM
.NET Core ORM with dbfirst support, and code scaffolding features. This ORM supports different database sources. You can find more information [here](https://www.paradigm.net.co/articles/1.introduction/index.html).



## Nuget Packages
| Library    | Nuget | Install
|-|-|-|
| Data       | [![NuGet](https://img.shields.io/nuget/v/Paradigm.ORM.Data.svg)](https://www.nuget.org/packages/Paradigm.ORM.Data/)            | `Install-Package Paradigm.ORM.Data` |
| MySql      | [![NuGet](https://img.shields.io/nuget/v/Paradigm.ORM.Data.MySql.svg)](https://www.nuget.org/packages/Paradigm.ORM.Data.MySql/)      | `Install-Package Paradigm.ORM.Data.MySql` |
| PostgreSQL | [![NuGet](https://img.shields.io/nuget/v/Paradigm.ORM.Data.PostgreSql.svg)](https://www.nuget.org/packages/Paradigm.ORM.Data.PostgreSql/) | `Install-Package Paradigm.ORM.Data.PostgreSql` |
| SQL Server | [![NuGet](https://img.shields.io/nuget/v/Paradigm.ORM.Data.SqlServer.svg)](https://www.nuget.org/packages/Paradigm.ORM.Data.SqlServer/)  | `Install-Package Paradigm.ORM.Data.SqlServer` |
| Cassandra | [![NuGet](https://img.shields.io/nuget/v/Paradigm.ORM.Data.Cassandra.svg)](https://www.nuget.org/packages/Paradigm.ORM.Data.Cassandra/)  | `Install-Package Paradigm.ORM.Data.Cassandra` |



## Self Contained Deploy (SCD)
Bellow you can find portable versions for all major OSs.
If you are planning to use the tools in several projects, we recommend to add the SCD folder to your PATH.

| Tool | OS | Zip File |
|-|-|-|
| DbFirst | Windows x86 | [Download](https://github.com/MiracleDevs/Paradigm.ORM/releases/latest/download/dbfirst.win-x86.tar.gz) |
| DbFirst | Windows x64 | [Download](https://github.com/MiracleDevs/Paradigm.ORM/releases/latest/download/dbfirst.win-x64.tar.gz) |
| DbFirst | Linux x64   | [Download](https://github.com/MiracleDevs/Paradigm.ORM/releases/latest/download/dbfirst.linux-x64.tar.gz) |
| DbFirst | OSX x64     | [Download](https://github.com/MiracleDevs/Paradigm.ORM/releases/latest/download/dbfirst.osx-x64.tar.gz) |
||||
| DbPublisher | Windows x86 | [Download](https://github.com/MiracleDevs/Paradigm.ORM/releases/latest/download/dbpublisher.win-x86.tar.gz) |
| DbPublisher | Windows x64 | [Download](https://github.com/MiracleDevs/Paradigm.ORM/releases/latest/download/dbpublisher.win-x64.tar.gz) |
| DbPublisher | Linux x64   | [Download](https://github.com/MiracleDevs/Paradigm.ORM/releases/latest/download/dbpublisher.linux-x64.tar.gz) |
| DbPublisher | OSX x64     | [Download](https://github.com/MiracleDevs/Paradigm.ORM/releases/latest/download/dbpublisher.osx-x64.tar.gz) |
||||
| DataExport | Windows x86 | [Download](https://github.com/MiracleDevs/Paradigm.ORM/releases/latest/download/dataexport.win-x86.tar.gz) |
| DataExport | Windows x64 | [Download](https://github.com/MiracleDevs/Paradigm.ORM/releases/latest/download/dataexport.win-x64.tar.gz) |
| DataExport | Linux x64   | [Download](https://github.com/MiracleDevs/Paradigm.ORM/releases/latest/download/dataexport.linux-x64.tar.gz) |
| DataExport | OSX x64     | [Download](https://github.com/MiracleDevs/Paradigm.ORM/releases/latest/download/dataexport.osx-x64.tar.gz) |




## Running the tests
We are working to automate the test suite, but currently, if you want to run the tests, you can create the test databases by
going to the directory `./build/docker/` and run docker compose:

```sh
$ cd ./build/docker
$ docker-compose up -d
```

You'll create 4 databases, one for each connection, that will be available in your localhost on the default ports:
- mysql `3306`
- mssql `1433`
- pgsql `5432`
- scylla (cassandra) `9042`

Once the docker compose provisioned the containers, you'll need to wait a couple of minutes depending on your machine, while each
container starts the database and run the initial scripts. If you run the tests just after the docker-compose up, you'll get a lot of
timeouts or errors.

After running the tests, you can shutdown the containers by:
```sh
$ docker-compose down
```

**Important**: Sometimes mysql can fail to setup the password and privileges. If that happens remove the container and do a `docker-compose up -d` again.




## Change log

Version `3.0.0`:
- Updated projects to .net6.0.
- Updated dependencies to latest versions.
- Updated tests to utilize new features.
- Due to breaking changes on PostgreSQL, a temporal fix has been added to the connection to support old date formats, see more [here](https://www.npgsql.org/doc/types/datetime.html) and [here](https://www.npgsql.org/doc/release-notes/6.0.html#timestamp-rationalization-and-improvements).

Version `2.6.4`
- Added a new feature to the `dbpublisher` tool to ignore script errors on execution. This only works while executing scripts, but the parameter will be dropped and ignored for file generation.
  To ignore the errors produced by a script, add the line `#ignore-errors` to your script file, and the `ScriptBuilder` will configure the script to ignore errors,
  and will remove that line from the content. This can come in handy for cassandra and scylla, because there's no way of checking before altering a table, and subsequent executions
  will produce errors if you are using `dbpublisher` for incremental builds.

Version `2.6.3`
- Updated cassandra connector to cache active connections by connection string instead of disposing them. When working with multiple threads, the ORM used to create multiple connections, and dispose them after finalization.
  Disposing the active connection also disposed the internal cluster and session, producing unintended side effects inside the datastax driver: A lot of leaked references being captured and not collected by GC.
  With this new change, we expect to provide a better performance and memory footprint overall.
  If you need to clean all the static connections, you can still call to `CqlConnectionManager.ClearConnections()` but note that clearing the connections won't free everything, due to how the datastax driver caches internal pool objects.
- Updated nuget dependencies.

Version `2.6.2`
- Moved the open check to a more generic method.

Version `2.6.1`
- Updated connector to open connection before creating a transaction, if the connection was closed.
- Added tests for transactions.

Version `2.6.0`
- Updated connector specific commands to check if the connection has been opened before executing.
- Removed open check from extensions.
- Updated nuget dependencies.

Version `2.5.0`
- Due to a constraint with Tuples, now the reader stored procedure must return a Tuple<List<TResult8>> for the last parameter.
- Updated code to use declarative using.
- Updated code to use async when possible in some cases that was calling the sync version.

Version `2.4.0`
- Updated nuget dependencies.
- Updated project version to use .NET Standard 2.1.
- Updated console projects to use .NET 5.0.
- Fixed tests to use the latest fluent assertion extensions.
- Added docker files to create test databases.
- Merged pull request from @bgrainger to update [MySql connector](https://github.com/mysql-net/MySqlConnector) to the last version.

Version `2.3.2`
- Updated nuget dependencies.
- Added logging provider and a default logger to log commands and command errors.

Version `2.3.1`
- Added more tests for composite partition keys for cassandra/scylla connector.
- Disabled the batch delete for cassandra/scylla due to cql constrains.
- Fixed delete query for cassandra/scylla connector.
- Updated nuget dependencies.


Version `2.3.0`
- Upgraded the cql schema provider, to use the newer scylla "system_schema" keyspace.
- Updated nuget dependencies.
- Updated test cases.


Version `2.2.4`
- Added visual basic tests.
- Updated nuget dependencies.
- Fixed a couple of bugs found with the vb tests.


Version `2.2.3`
- Added new DatabaseCommandException thrown when executing database commands. The DatabaseCommandException contains a reference to the
  executing command, allowing for a better debugging experience.
  Use Command.CommandText to observe the sql or cql query being executed.
  Use Command.Parameters to observe the parameters bound to the query.
- Fixed a bug in Cassandra connector not adding a parameter in one of the AddParameters methods.
- Fixed a bug in CustomQuery sync execution not updated the command text after parameter replacement.
- Improved and updated tests.


Version `2.2.2`
- Removed mandatory data type in ColumnAttribute. The orm will choose the default column types for each database type.
- Changed how the CommandBatch replace parameter names, to prevent name collision.
- Added tests for the command batch name replacement.
- Changed how select parameters are replaced, from @Index to  @pIndex or :pIndex, depending on the database parameter naming conventions.
- Updated NuGet dependencies.


Version `2.2.1`
- Added a cache service for descriptors all over the orm, to prevent tons of small objects filling the heap.
- Removed constructors receiving descriptors. Now all the ORM classes should refer to the cache for descriptors.
- Descriptor constructors are now internal and can not be instantiated outside the ORM.


Version `2.2.0`
- Refactor command handling to allow parallel execution of the ORM without conflicting with some of the
  connectors. The orm does not cache a command inside the command builder any more.
- Refactor command builders and moved shared functionality to the core classes, and removed the
  duplication from the client implementations. Now will be even easier to implement new clients.
- Moved base protected methods from the CommandBuilderBase to the ICommandFormatProvider and added a new
  base CommandFormatProviderBase with shared behavior for all the different format providers.
- Removed IDisposable interface from most of the ORM core classes. The most notable are:
  - Database access
  - Query
  - Custom query
  - All the stored procedure types
  - Schema Provider
- Removed extension methods for the IDatabaseConnector not used any more.


Version `2.1.7`
- Changed how the DatabaseAccess classes utilize the BatchManager to be thread safe.


Version `2.1.6`
- Updated Paradigm.Core and other dependencies.
- Published new versions for the tools.


Version `2.1.5`
- Removed a dependency over generic entities that needed a parameterless constructor
  in all the solution.


Version `2.1.4`
- Removed a dependency over generic entities that needed a parameterless constructor.


Version `2.1.3`
- Added new constructor to `DatabaseReaderMapper` to allow set both the service provider and the
  database connector. This will allow multi-tenancy support using the dbfirst generated code.
- Added new constructors to all the stored procedure types for the same reason as the previous point.
- Added missing ValueConverter inside the database reader value provider.


Version `2.1.2`
- Changed the database reader mappers to work with the `IServiceProvider` class. Now, will try to instantiate
  the entities with the service provider first, and if the service provider can't, will use the activator to
  create a new instance. This will allow the Paradigm.Services framework to fully delegate the instancing to
  DI allowing better DDD.


Version `2.1.1`
- Fixed a problem in cassandra connector where the schema provider can not guess the column type when the column is a
  clustering key with order applied to it.
- Made the modifications to the tests to test the above problem.


Version `2.1.0`
- Added a new Cassandra connector.
  This new connector allows to work against Apache Cassandra o ScyllaDB. There are some limitations imposed by the
  DataStax connector, and other imposed by the orm, but for most cases will be just fine.
> Warning: The ORM will work with column families that mimic sql tables, aka. without lists, maps, or other not standard
> relational databases. Even if Cassandra does not supports joins, the ORM allows to create virtual foreign keys between tables
> and create navigation properties from it.
- Data Export, DbFirst and DbPublisher can work now against Cassandra and ScyllaDB.
- In all the configuration files, now the Database Type changed to Upper Camel Case syntax, the database types are:
    - SqlServer,
    - MySql,
    - PostgreSql,
    - Cassandra
- Updated MySql Connector version.


Version `2.0.1`
- Updated Paradigm.Core to version `2.0.1`.


Version `2.0.0`
- Updated .net core from version 1 to version 2.


Version `1.0.0`
- Uploaded first version of the Paradigm ORM.
