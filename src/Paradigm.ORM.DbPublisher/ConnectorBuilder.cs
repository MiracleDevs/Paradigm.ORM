using Paradigm.ORM.Data.Cassandra;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.MySql;
using Paradigm.ORM.Data.PostgreSql;
using Paradigm.ORM.Data.SqlServer;
using Paradigm.ORM.DbPublisher.Configuration;

namespace Paradigm.ORM.DbPublisher
{
    public static class ConnectorBuilder
    {
        public static IDatabaseConnector Build(PublishConfiguration configuration)
        {
            switch (configuration.DatabaseType)
            {
                case DatabaseType.SqlServer:
                    return new SqlDatabaseConnector(configuration.ConnectionString);

                case DatabaseType.MySql:
                    return new MySqlDatabaseConnector(configuration.ConnectionString);

                case DatabaseType.PostgreSql:
                    return new PostgreSqlDatabaseConnector(configuration.ConnectionString);

                case DatabaseType.Cassandra:
                    return new CqlDatabaseConnector(configuration.ConnectionString);

                default:
                    return null;
            }
        }
    }
}