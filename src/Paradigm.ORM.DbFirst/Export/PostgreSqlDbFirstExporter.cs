using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.PostgreSql;
using Paradigm.ORM.DbFirst.Configuration;

namespace Paradigm.ORM.DbFirst.Export
{
    internal class PostgreSqlDbFirstExporter : DbFirstExporterBase
    {
        protected override IDatabaseConnector CreateConnector(DbFirstConfiguration configuration)
        {
            return new PostgreSqlDatabaseConnector(configuration.ConnectionString);
        }
    }
}