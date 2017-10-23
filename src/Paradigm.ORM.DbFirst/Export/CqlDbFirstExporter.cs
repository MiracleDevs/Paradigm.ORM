using Paradigm.ORM.Data.Cassandra;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.DbFirst.Configuration;

namespace Paradigm.ORM.DbFirst.Export
{
    internal class CqlDbFirstExporter : DbFirstExporterBase
    {
        protected override IDatabaseConnector CreateConnector(DbFirstConfiguration configuration)
        {
            return new CqlDatabaseConnector(configuration.ConnectionString);
        }
    }
}