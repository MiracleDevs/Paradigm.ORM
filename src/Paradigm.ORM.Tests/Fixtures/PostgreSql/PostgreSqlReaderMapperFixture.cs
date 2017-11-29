using System;
using System.IO;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Descriptors;
using Paradigm.ORM.Data.Extensions;
using Paradigm.ORM.Data.PostgreSql;
using Paradigm.ORM.Tests.Mocks.PostgreSql;

namespace Paradigm.ORM.Tests.Fixtures.PostgreSql
{
    public class PostgreSqlReaderMapperFixture: ReaderMapperFixtureBase
    {
        protected override string ConnectionString => "Server=localhost;User Id=test;Password=test1234;Timeout=3";

        public override string SelectStatement => @"SELECT ""Id"",""Name"",""IsActive"",""Amount"",""CreatedDate"", ""BigintProperty"",
                                                           ""BigserialProperty"",""Serial8Property"",""BooleanProperty"",""BoolProperty"",
                                                           ""ByteaProperty"",""IntProperty"",""Int8Property"",""IntegerProperty"",
                                                           ""SmallintProperty"",""Serial4Property"",""FloatProperty"",""DoubleProperty"",
                                                           ""DoubleprecisionProperty"",""MoneyProperty"",""DecimalProperty"",""NumericProperty"",
                                                           ""RealProperty"",""DateProperty"",""TimestampProperty"",""TimestampNoTimeZoneProperty"",
                                                           ""TimestampTimeZoneProperty"",""TimeProperty"",""TimeNoTimeZoneProperty"",""IntervalProperty"",
                                                           ""CharProperty"",""CharacterProperty"",""VarcharProperty"",""CharacterVaryingProperty"",
                                                           ""TextProperty"" 
                                                    FROM ""AllColumns""";

        protected override IDatabaseConnector CreateConnector()
        {
            return new PostgreSqlDatabaseConnector(this.ConnectionString);
        }

        public override void CreateDatabase()
        {
        }

        public override void DropDatabase()
        {
            this.Connector.ExecuteNonQuery("DROP TABLE IF EXISTS \"AllColumns\";");
        }

        public override void CreateTable()
        {
            this.Connector.ExecuteNonQuery(
                "CREATE TABLE IF NOT EXISTS \"AllColumns\"(" +
                    "\"Id\"                             SERIAL," +
                    "\"Name\"                           VARCHAR(200)                    NOT NULL," +
                    "\"IsActive\"                       BOOLEAN                         NOT NULL," +
                    "\"Amount\"                         DECIMAL(20,9)                   NOT NULL," +
                    "\"CreatedDate\"                    DATE                            NOT NULL," +
                    "\"BigintProperty\"                 BIGINT                          NOT NULL," +
                    "\"BigserialProperty\"              BIGSERIAL                       NOT NULL," +
                    "\"Serial8Property\"                SERIAL8                         NOT NULL," +
                    "\"BooleanProperty\"                BOOLEAN                         NOT NULL," +
                    "\"BoolProperty\"                   BOOL                            NOT NULL," +
                    "\"ByteaProperty\"                  BYTEA                           NOT NULL," +
                    "\"IntProperty\"                    INT                             NOT NULL," +
                    "\"Int8Property\"                   INT8                            NOT NULL," +
                    "\"IntegerProperty\"                INTEGER                         NOT NULL," +
                    "\"SmallintProperty\"               SMALLINT                        NOT NULL," +
                    "\"Serial4Property\"                SERIAL4                         NOT NULL," +
                    "\"FloatProperty\"                  FLOAT                           NOT NULL," +
                    "\"DoubleProperty\"                 FLOAT8                          NOT NULL," +
                    "\"DoubleprecisionProperty\"        DOUBLE PRECISION                NOT NULL," +
                    "\"MoneyProperty\"                  MONEY                           NOT NULL," +
                    "\"DecimalProperty\"                DECIMAL                         NOT NULL," +
                    "\"NumericProperty\"                NUMERIC                         NOT NULL," +
                    "\"RealProperty\"                   REAL                            NOT NULL," +
                    "\"DateProperty\"                   DATE                            NOT NULL," +
                    "\"TimestampProperty\"              TIMESTAMP                       NOT NULL," +
                    "\"TimestampNoTimeZoneProperty\"    TIMESTAMP WITHOUT TIME ZONE     NOT NULL," +
                    "\"TimestampTimeZoneProperty\"      TIMESTAMP WITH TIME ZONE        NOT NULL," +
                    "\"TimeProperty\"                   TIME                            NOT NULL," +
                    "\"TimeNoTimeZoneProperty\"         TIME WITHOUT TIME ZONE          NOT NULL," +
                    "\"IntervalProperty\"               INTERVAL                        NOT NULL," +
                    "\"CharProperty\"                   CHAR                            NOT NULL," +
                    "\"CharacterProperty\"              CHARACTER                       NOT NULL," +
                    "\"VarcharProperty\"                VARCHAR(2)                      NOT NULL," +
                    "\"CharacterVaryingProperty\"       CHARACTER VARYING               NOT NULL," +
                    "\"TextProperty\"                   TEXT                            NOT NULL," +

                    "CONSTRAINT \"PK_AllColumns\" PRIMARY KEY (\"Id\")," +
                    "CONSTRAINT \"UX_AllColumns_Name\" UNIQUE (\"Name\")" +
                ");");
        }

        public override object CreateNewEntity()
        {
            return new AllColumnsClass
            {
                Name = "Test " + Guid.NewGuid(),
                IsActive = true,
                Amount = (decimal)30.34,
                CreatedDate = DateTime.Today,
                BigintProperty = -922337203685,
                BigserialProperty = 922337203685,
                Serial8Property = 21474836,
                BooleanProperty = true,
                BoolProperty = false,
                ByteaProperty = File.ReadAllBytes(Directory.GetCurrentDirectory() + "\\Mocks\\Data\\image.png"),
                IntProperty = -98745587,
                Int8Property = 987587,
                IntegerProperty = 98745587,
                SmallintProperty =  -32548,
                Serial4Property = 2414,
                FloatProperty = 3.14159f,
                DoubleProperty = 3.141592,
                DoubleprecisionProperty = -3.141592,
                MoneyProperty = 3.50m,
                DecimalProperty = 2.1685456m,
                NumericProperty = 5485.99m,
                RealProperty = 3.55f,
                DateProperty = new DateTime(2017, 5, 23, 0, 0, 0, 0),
                TimestampProperty = new DateTime(2017, 9, 12, 15, 33, 7, 0),
                TimestampNoTimeZoneProperty = new DateTime(2017, 9, 12, 15, 33, 7, 0),
                TimestampTimeZoneProperty = new DateTime(2017, 9, 12, 15, 33, 7, 0),
                TimeProperty = new TimeSpan(0, 22, 21, 20, 19),
                TimeNoTimeZoneProperty = new TimeSpan(0, 22, 21, 20, 19),
                IntervalProperty = new TimeSpan(0, 22, 21, 20, 19),
                CharProperty = "t",
                CharacterProperty = "e",
                VarcharProperty = "st",
                CharacterVaryingProperty = "test",
                TextProperty = "This is a test for a text type field in PostgreSql data base."
            };
        }

        public override ITableTypeDescriptor GetDescriptor()
        {
            return DescriptorCache.Instance.GetTableTypeDescriptor(typeof(AllColumnsClass));
        }
    }
}
