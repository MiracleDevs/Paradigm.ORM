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
            return database switch
            {
                DatabaseType.MySql => new MySqlDatabaseConnector(),
                DatabaseType.PostgreSql => new PostgreSqlDatabaseConnector(),
                DatabaseType.SqlServer => new SqlDatabaseConnector(),
                DatabaseType.Cassandra => new CqlDatabaseConnector(),
                _ => throw new ArgumentOutOfRangeException(nameof(database), database, null)
            };
        }
    }
}