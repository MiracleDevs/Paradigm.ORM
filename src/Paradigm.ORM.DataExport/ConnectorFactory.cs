using System;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.MySql;
using Paradigm.ORM.Data.PostgreSql;
using Paradigm.ORM.Data.SqlServer;
using Paradigm.ORM.DataExport.Configuration;
using Paradigm.ORM.Data.Cassandra;

namespace Paradigm.ORM.DataExport
{
    public static class ConnectorFactory
    {
        public static IDatabaseConnector Create(DatabaseType database)
        {
            switch (database)
            {
                case DatabaseType.MySql:
                    return new MySqlDatabaseConnector();

                case DatabaseType.PostgreSql:
                    return new PostgreSqlDatabaseConnector();

                case DatabaseType.SqlServer:
                    return new SqlDatabaseConnector();

                case DatabaseType.Cassandra:
                    return new CqlDatabaseConnector();

                default:
                    throw new ArgumentOutOfRangeException(nameof(database), database, null);
            }
        }
    }
}