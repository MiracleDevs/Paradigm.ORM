using System;
using System.IO;
using System.Net;
using System.Numerics;
using Cassandra;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Descriptors;
using Paradigm.ORM.Data.Extensions;
using Paradigm.ORM.Data.Cassandra;
using Paradigm.ORM.Tests.Mocks.Cql;

namespace Paradigm.ORM.Tests.Fixtures.Cql
{
    public class CqlReaderMapperFixture : ReaderMapperFixtureBase
    {
        protected override string ConnectionString => "Contact Points=192.168.2.240;Port=9042";

        public override string SelectStatement => @"SELECT * FROM ""test"".""allcolumns""";

        protected override IDatabaseConnector CreateConnector()
        {
            return new CqlDatabaseConnector(this.ConnectionString);
        }

        public override void CreateDatabase()
        {
            // We assume the database is created, we only create and drop the content of it
        }

        public override void DropDatabase()
        {
            this.Connector.ExecuteNonQuery(@"DROP TABLE IF EXISTS ""test"".""allcolumns"";");
        }

        public override void CreateTable()
        {
            this.Connector.ExecuteNonQuery(@"
                CREATE TABLE IF NOT EXISTS ""test"".""allcolumns"" (
                    ""column01"" uuid PRIMARY KEY,
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
                    ""column19"" varint
                );
            ");
        }

        public override object CreateNewEntity()
        {
            return new AllColumnsClass
            {
                /* 01 */ Id = Guid.NewGuid(),
                /* 02 */ Ascii = "simple ascii string.",
                /* 03 */ BigInt = 0,
                /* 04 */ Blob = File.ReadAllBytes(Directory.GetCurrentDirectory() + "\\Mocks\\Data\\small_image.png"),
                /* 05 */ Boolean = true,
                /* 06 */ Date = new DateTime(2017, 10, 10),
                /* 07 */ Decimal = 0,
                /* 08 */ Double = 0,
                /* 09 */ Float = 0,
                /* 10 */ Inet = new IPAddress(new byte[] { 127, 0, 0, 1 }),
                /* 11 */ Int = 0,
                /* 12 */ SmallInt = 0,
                /* 13 */ Text = "trying some text.",
                /* 14 */ Time = new TimeSpan(10, 0, 0),
                /* 15 */ Timestamp =new DateTime(2017, 10, 10, 12, 0, 0, 0),
                /* 16 */ TimeUuid  = TimeUuid.NewId(DateTime.Now),
                /* 17 */ TinyInt =  0,
                /* 18 */ VarChar = "Text var char.",
                /* 19 */ VarInt = new BigInteger(0)
            };
        }

        public override ITableTypeDescriptor GetDescriptor()
        {
            return new TableTypeDescriptor(typeof(AllColumnsClass));
        }
    }
}
