[![Build Status](https://travis-ci.org/MiracleDevs/Paradigm.ORM.svg?branch=master)](https://travis-ci.org/MiracleDevs/Paradigm.ORM)


| Library    | Nuget | Install
|-|-|-|
| Data       | [![NuGet](https://img.shields.io/nuget/v/Nuget.Core.svg)](https://www.nuget.org/packages/Paradigm.ORM.Data/)            | `Install-Package Paradigm.ORM.Data` |
| MySql      | [![NuGet](https://img.shields.io/nuget/v/Nuget.Core.svg)](https://www.nuget.org/packages/Paradigm.ORM.Data.MySql/)      | `Install-Package Paradigm.ORM.Data.MySql` |
| PostgreSQL | [![NuGet](https://img.shields.io/nuget/v/Nuget.Core.svg)](https://www.nuget.org/packages/Paradigm.ORM.Data.PostgreSql/) | `Install-Package Paradigm.ORM.Data.PostgreSql` |
| SQL Server | [![NuGet](https://img.shields.io/nuget/v/Nuget.Core.svg)](https://www.nuget.org/packages/Paradigm.ORM.Data.SqlServer/)  | `Install-Package Paradigm.ORM.Data.SqlServer` |
| Cassandra | [![NuGet](https://img.shields.io/nuget/v/Nuget.Core.svg)](https://www.nuget.org/packages/Paradigm.ORM.Data.Cassandra/)  | `Install-Package Paradigm.ORM.Data.Cassandra` |

# Paradigm.ORM
.NET Core ORM with dbfirst support, and code scaffolding features. This ORM supports different database sources.


Self Contained Deploy (SCD)
---

Bellow you can find portable versions for all major OSs.
If you are planning to use the tools in several projects, we recommend to add the SCD folder to your PATH.

| Tool | OS | Zip File |
|-|-|-|
| DbFirst | Windows x86 | [Download](https://raw.githubusercontent.com/MiracleDevs/Paradigm.ORM/master/dist/dbfirst.win-x86.zip) |
| DbFirst | Windows x64 | [Download](https://raw.githubusercontent.com/MiracleDevs/Paradigm.ORM/master/dist/dbfirst.win-x64.zip) |
| DbFirst | Linux x64   | [Download](https://raw.githubusercontent.com/MiracleDevs/Paradigm.ORM/master/dist/dbfirst.linux-x64.zip) |
| DbFirst | OSX x64     | [Download](https://raw.githubusercontent.com/MiracleDevs/Paradigm.ORM/master/dist/dbfirst.osx-x64.zip) |
||||
| DbPublisher | Windows x86 | [Download](https://raw.githubusercontent.com/MiracleDevs/Paradigm.ORM/master/dist/dbpublisher.win-x86.zip) |
| DbPublisher | Windows x64 | [Download](https://raw.githubusercontent.com/MiracleDevs/Paradigm.ORM/master/dist/dbpublisher.win-x64.zip) |
| DbPublisher | Linux x64   | [Download](https://raw.githubusercontent.com/MiracleDevs/Paradigm.ORM/master/dist/dbpublisher.linux-x64.zip) |
| DbPublisher | OSX x64     | [Download](https://raw.githubusercontent.com/MiracleDevs/Paradigm.ORM/master/dist/dbpublisher.osx-x64.zip) |
||||
| DataExport | Windows x86 | [Download](https://raw.githubusercontent.com/MiracleDevs/Paradigm.ORM/master/dist/dataexport.win-x86.zip) |
| DataExport | Windows x64 | [Download](https://raw.githubusercontent.com/MiracleDevs/Paradigm.ORM/master/dist/dataexport.win-x64.zip) |
| DataExport | Linux x64   | [Download](https://raw.githubusercontent.com/MiracleDevs/Paradigm.ORM/master/dist/dataexport.linux-x64.zip) |
| DataExport | OSX x64     | [Download](https://raw.githubusercontent.com/MiracleDevs/Paradigm.ORM/master/dist/dataexport.osx-x64.zip) |

Change log
---

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


