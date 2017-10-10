using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.MySql;
using Paradigm.ORM.DbFirst.Configuration;

namespace Paradigm.ORM.DbFirst.Export
{
    internal class MySqlDbFirstExporter : DbFirstExporterBase
    {
        protected override IDatabaseConnector CreateConnector(DbFirstConfiguration configuration)
        {
            return new MySqlDatabaseConnector(configuration.ConnectionString);
        }
    }
}