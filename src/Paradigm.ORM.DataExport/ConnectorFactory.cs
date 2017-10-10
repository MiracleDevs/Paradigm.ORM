using System;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.MySql;
using Paradigm.ORM.Data.PostgreSql;
using Paradigm.ORM.Data.SqlServer;
using Paradigm.ORM.DataExport.Configuration;

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

                default:
                    throw new ArgumentOutOfRangeException(nameof(database), database, null);
            }
        }
    }
}