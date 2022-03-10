using System;
using Paradigm.ORM.DbFirst.Configuration;

namespace Paradigm.ORM.DbFirst.Export
{
    internal static class DbFirstExporterFactory
    {
        public static IDbFirstExporter Create(DatabaseType databaseType)
        {
            return databaseType switch
            {
                DatabaseType.MySql => new MySqlDbFirstExporter(),
                DatabaseType.SqlServer => new SqlDbFirstExporter(),
                DatabaseType.PostgreSql => new PostgreSqlDbFirstExporter(),
                DatabaseType.Cassandra => new CqlDbFirstExporter(),
                _ => throw new Exception("Database type not supported. Only [mysql, tsql] types are supported.")
            };
        }
    }
}