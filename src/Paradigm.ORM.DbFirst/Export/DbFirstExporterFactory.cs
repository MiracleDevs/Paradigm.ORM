using System;
using Paradigm.ORM.DbFirst.Configuration;

namespace Paradigm.ORM.DbFirst.Export
{
    internal static class DbFirstExporterFactory
    {
        public static IDbFirstExporter Create(DatabaseType databaseType)
        {
            switch(databaseType)
            {
                case DatabaseType.MySql:
                    return new MySqlDbFirstExporter();

                case DatabaseType.SqlServer:
                    return new SqlDbFirstExporter();

                case DatabaseType.PostgreSql:
                    return new PostgreSqlDbFirstExporter();

                case DatabaseType.Cassandra:
                    return new CqlDbFirstExporter();

                default:
                    throw new Exception("Database type not supported. Only [mysql, tsql] types are supported.");
            }
        }
    }
}