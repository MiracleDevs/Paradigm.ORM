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
                case "tsql":
                    return new SqlDatabaseConnector(configuration.ConnectionString);

                case "mysql":
                    return new MySqlDatabaseConnector(configuration.ConnectionString);

                case "postgres":
                    return new PostgreSqlDatabaseConnector(configuration.ConnectionString);

                default:
                    return null;
            }
        }
    }
}