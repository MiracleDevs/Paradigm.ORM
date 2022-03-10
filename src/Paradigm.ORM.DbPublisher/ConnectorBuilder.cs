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
            return configuration.DatabaseType switch
            {
                DatabaseType.SqlServer => new SqlDatabaseConnector(configuration.ConnectionString),
                DatabaseType.MySql => new MySqlDatabaseConnector(configuration.ConnectionString),
                DatabaseType.PostgreSql => new PostgreSqlDatabaseConnector(configuration.ConnectionString),
                DatabaseType.Cassandra => new CqlDatabaseConnector(configuration.ConnectionString),
                _ => null
            };
        }
    }
}