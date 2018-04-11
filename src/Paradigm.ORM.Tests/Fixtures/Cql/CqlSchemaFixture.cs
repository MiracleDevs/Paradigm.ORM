using System;
using Paradigm.ORM.Data.Descriptors;
using Paradigm.ORM.Data.Extensions;
using Paradigm.ORM.Data.Cassandra;
using Paradigm.ORM.Tests.Mocks.Cql;

namespace Paradigm.ORM.Tests.Fixtures.Cql
{
    public class CqlSchemaFixture: IDisposable
    {
        protected string ConnectionString => "Contact Points=localhost;Port=9042";

        public CqlDatabaseConnector Connector { get; }

        public void Dispose()
        {
            this.Connector.Dispose();
        }

        public CqlSchemaFixture()
        { 
            this.Connector = new CqlDatabaseConnector(this.ConnectionString);
        }

        public void DropDatabase()
        {
            this.Connector.ExecuteNonQuery(@"DROP TABLE IF EXISTS ""test"".""table1"";");
            this.Connector.ExecuteNonQuery(@"DROP TABLE IF EXISTS ""test"".""table2"";");
            this.Connector.ExecuteNonQuery(@"DROP TABLE IF EXISTS ""test"".""table3"";");
        }

        public void CreateTables()
        {
            this.Connector.ExecuteNonQuery(@"
                CREATE TABLE IF NOT EXISTS ""test"".""table1"" (
                    ""column01"" uuid,
                    ""column02"" ascii,
                    ""column03"" bigint,
                    ""column04"" blob,
                    ""column05"" boolean,
                    ""column06"" date,
                    ""column07"" decimal,
                    ""column08"" double,
                    ""column09"" float,
                    ""column10"" inet,
                    ""column11"" int,
                    ""column12"" smallint,
                    ""column13"" text,
                    ""column14"" time,
                    ""column15"" timestamp,
                    ""column16"" timeuuid,
                    ""column17"" tinyint,
                    ""column18"" varchar,
                    ""column19"" varint,
                    PRIMARY KEY (""column01"")
                );
            ");

            this.Connector.ExecuteNonQuery(@"
                CREATE TABLE IF NOT EXISTS ""test"".""table2"" (
                    ""column01"" uuid,
                    ""column02"" ascii,
                    ""column03"" bigint,
                    ""column04"" blob,
                    ""column05"" boolean,
                    ""column06"" date,
                    ""column07"" decimal,
                    ""column08"" double,
                    ""column09"" float,
                    ""column10"" inet,
                    ""column11"" int,
                    ""column12"" smallint,
                    ""column13"" text,
                    ""column14"" time,
                    ""column15"" timestamp,
                    ""column16"" timeuuid,
                    ""column17"" tinyint,
                    ""column18"" varchar,
                    ""column19"" varint,
                    PRIMARY KEY (""column01"", ""column15"", ""column13"")
                );
            ");

            this.Connector.ExecuteNonQuery(@"
                CREATE TABLE IF NOT EXISTS ""test"".""table3"" (
                    ""column01"" uuid,
                    ""column02"" ascii,
                    ""column03"" bigint,
                    ""column04"" blob,
                    ""column05"" boolean,
                    ""column06"" date,
                    ""column07"" decimal,
                    ""column08"" double,
                    ""column09"" float,
                    ""column10"" inet,
                    ""column11"" int,
                    ""column12"" smallint,
                    ""column13"" text,
                    ""column14"" time,
                    ""column15"" timestamp,
                    ""column16"" timeuuid,
                    ""column17"" tinyint,
                    ""column18"" varchar,
                    ""column19"" varint,
                    PRIMARY KEY ((""column01"", ""column02""), ""column15"", ""column13"")
                )
                WITH CLUSTERING ORDER BY (""column15"" ASC, ""column13"" DESC);
            ");
        }

        public ITableTypeDescriptor GetDescriptor()
        {
            return DescriptorCache.Instance.GetTableTypeDescriptor(typeof(AllColumnsClass));
        }

        public string GetDatabaseName()
        {
            return "test";
        }
    }
}
