using System;

namespace Paradigm.ORM.DbFirst.Export
{
    internal static class DbFirstExporterFactory
    {
        public static IDbFirstExporter Create(string databaseType)
        {
            switch(databaseType.ToLower())
            {
                case "mysql":
                    return new MySqlDbFirstExporter();

                case "tsql":
                    return new SqlDbFirstExporter();

                case "postgresql":
                    return new PostgreSqlDbFirstExporter();

                default:
                    throw new Exception("Database type not supported. Only [mysql, tsql] types are supported.");
            }
        }
    }
}