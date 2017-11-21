# 2. Supported Databases
Paradigm ORM currently supports 4 databases, although we'll add support for other databases like SQLite and Oracle in the future. Implementing a new database is a trivial process, so we'll implement them as needed or requested.

## 2.1. MySql
The ORM fully supports MySql databases. Supports stored procedures, tables, views, etc. MySQL databases can also be written and published using our [dbpublisher tool](~/articles/6.dbpublisher/index.md).
While we always try to use the official connector for each database, for MySql we had to go in another direction. Oracle's official connector does not truly supports async methods, so we decided to change the Oracle connector and use another implementation called [MySql Connector](https://mysql-net.github.io/MySqlConnector/). This is a new implementation of MySql protocol that fully supports async methods. Also, it's an active repository and the team behind the connector respond fast and efficiently.

## 2.2. PostgreSql
PostgreSql is also fully supported. Paradigm ORM supports stored procedures, tables, views, etc. PostgreSql databases can also be written and published using our [dbpublisher tool](~/articles/6.dbpublisher/index.md).

## 2.3. Sql Server
Sql Server is of course fully supported. Our ORM supports stored procedures, tables, views, etc. Sql Server databases can also be written and published using our [dbpublisher tool](~/articles/6.dbpublisher/index.md), but if you are using Sql Server, we recommend to use the database project included in the SSDT [here](https://www.visualstudio.com/vs/ssdt/). More information about how to create and manage Sql Server projects can be found [here](https://www.codeproject.com/Articles/825831/SQL-Server-Database-Development-in-Visual-Studio).

## 2.4. Cassandra and ScyllaDB <span class="badge">new</span>
Cassandra and ScyllaDB are also supported with some caveats. Both Cassandra and ScyllaDB are NoSQL data store. Both Cassandra and ScyllaDB supports
complex column families (lists for examples) and so the use of an orm can be unnecessary in some scenarios. But if you are using the full Paradigm
stack as we do, having the chance to work with a multi-tenancy orm and work against different databases from a single solution can be a really nice
feature. Currently there are some features not implemented, because ScyllaDB does not support them in its production-ready branch. Materialized views for example is only supported in the experimental branch. We are using the [Datastax C# Driver](https://github.com/datastax/csharp-driver) as the underlying connector for the ORM.
Both ScyllaDB and Cassandra keyspaces and column families can also be written and published using our [dbpublisher tool](~/articles/6.dbpublisher/index.md).
We're currently testing it and developing the connector using [ScyllaDB](http://www.scylladb.com/).