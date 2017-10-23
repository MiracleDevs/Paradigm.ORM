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
        public static IDatabaseConnector Create(Database database)
        {
            switch (database)
            {
                case Database.MySql:
                    return new MySqlDatabaseConnector();

                case Database.PostgreSql:
                    return new PostgreSqlDatabaseConnector();

                case Database.SqlServer:
                    return new SqlDatabaseConnector();

                case Database.Cassandra:
                    return new CqlDatabaseConnector();

                default:
                    throw new ArgumentOutOfRangeException(nameof(database), database, null);
            }
        }
    }
}