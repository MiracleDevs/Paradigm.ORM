using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.SqlServer;
using Paradigm.ORM.DbFirst.Configuration;

namespace Paradigm.ORM.DbFirst.Export
{
    internal class SqlDbFirstExporter: DbFirstExporterBase
    {
        protected override IDatabaseConnector CreateConnector(DbFirstConfiguration configuration)
        {
            return new SqlDatabaseConnector(configuration.ConnectionString);
        }
    }
}