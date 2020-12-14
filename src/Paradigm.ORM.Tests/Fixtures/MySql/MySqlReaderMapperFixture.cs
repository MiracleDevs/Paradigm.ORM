using System;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Descriptors;
using Paradigm.ORM.Data.Extensions;
using Paradigm.ORM.Tests.Mocks.MySql;
using Paradigm.ORM.Data.MySql;
using System.IO;

namespace Paradigm.ORM.Tests.Fixtures.MySql
{
    public class MySqlReaderMapperFixture: ReaderMapperFixtureBase
    {
        private string ConnectionString => ConnectionStrings.MySql;

        public override string SelectStatement => @"SELECT `Id`,`Name`,`IsActive`,`Amount`,`CreatedDate`,`TinyIntProperty`, `BoolProperty`,
                                                           `SmallintProperty`,`MediumIntProp`,`IntProp`,`BigIntProperty`,`FloatProperty`,
                                                           `DoubleProperty`,`YearProperty`,`TimeProperty`,`DateProperty`,`DatetimeProperty`,
                                                           `TimestampProperty`,`CharProperty`,`VarcharProperty`,`TinytextProperty`,
                                                           `TextProperty`,`MediumtextProperty`,`LongtextProperty`,`BlobProperty`,`TinyBlobProperty`,
                                                           `MediumBlobProperty`,`LongBlobProperty`,`BinaryProperty`,`VarBinaryProperty`
                                                    FROM   `test`.`all_columns`";

        protected override IDatabaseConnector CreateConnector()
        {
            return new MySqlDatabaseConnector(this.ConnectionString);
        }

        public override void CreateDatabase()
        {
            // We assume the database is created, we only create and drop the content of it
        }

        public override void DropDatabase()
        {
            this.Connector.ExecuteNonQuery("DROP TABLE IF EXISTS `all_columns`;");
        }

        public override void CreateTable()
        {
            this.Connector.ExecuteNonQuery(@"
                CREATE TABLE IF NOT EXISTS `test`.`all_columns`
                (
                    `Id`                    INT             NOT NULL AUTO_INCREMENT,
                    `Name`                  NVARCHAR(200)   NOT NULL,
                    `IsActive`              BOOL            NOT NULL,
                    `Amount`                DECIMAL(20,9)   NOT NULL,
                    `CreatedDate`           DATETIME        NOT NULL,
                    `TinyIntProperty`       TINYINT         NOT NULL,
                    `BoolProperty`          BOOLEAN         NOT NULL,
                    `SmallintProperty`      SMALLINT        NOT NULL,
                    `MediumIntProp`         MEDIUMINT       NOT NULL,
                    `IntProp`               INT             NOT NULL,
                    `BigIntProperty`        BIGINT          NOT NULL,
                    `FloatProperty`         FLOAT           NOT NULL,
                    `DoubleProperty`        DOUBLE          NOT NULL,
                    `YearProperty`          YEAR            NOT NULL,
                    `TimeProperty`          TIME            NOT NULL,
                    `DateProperty`          DATE            NOT NULL,
                    `DatetimeProperty`      DATETIME        NOT NULL,
                    `TimestampProperty`     TIMESTAMP       NOT NULL,
                    `CharProperty`          CHAR            NOT NULL,
                    `VarcharProperty`       VARCHAR(20)     NOT NULL,
                    `TinytextProperty`      TINYTEXT        NOT NULL,
                    `TextProperty`          TEXT            NOT NULL,
                    `MediumtextProperty`    MEDIUMTEXT      NOT NULL,
                    `LongtextProperty`      LONGTEXT        NOT NULL,
                    `BlobProperty`          BLOB            NOT NULL,
                    `TinyBlobProperty`      TINYBLOB        NOT NULL,
                    `MediumBlobProperty`    MEDIUMBLOB      NOT NULL,
                    `LongBlobProperty`      LONGBLOB        NOT NULL,
                    `BinaryProperty`        BINARY(235)     NOT NULL,
                    `VarBinaryProperty`     VARBINARY(255)  NOT NULL,

                    CONSTRAINT `PK_allcolumns` PRIMARY KEY (`Id` ASC),
	                CONSTRAINT `UX_allcolumns_Name` UNIQUE (`Name`)

                )ENGINE=INNODB;
            ");
        }

        public override object CreateNewEntity()
        {
            return new AllColumnsClass
            {
                Name = "Test " + Guid.NewGuid(),
                IsActive = true,
                Amount = (decimal)30.34,
                CreatedDate = DateTime.Today,
                TinyIntProperty = false,
                BoolProperty = true,
                SmallintProperty = short.MaxValue,
                MediumIntProp = 8388607,
                IntProp = int.MinValue,
                BigIntProperty = long.MinValue,
                FloatProperty = 3.14159f,
                DoubleProperty = 3.141592,
                YearProperty = 1957,
                TimeProperty = new TimeSpan(0, 13, 38, 33,0),
                DateProperty = DateTime.Today,
                DatetimeProperty = DateTime.Today.AddDays(30),
                TimestampProperty = new DateTime(2017, 12, 15, 13, 41, 55, 0),
                CharProperty = "2",
                VarcharProperty = "VarChar",
                TinytextProperty = "TinyText",
                TextProperty = "Text",
                MediumtextProperty = "MediumText",
                LongtextProperty = "LongText",
                BinaryProperty = File.ReadAllBytes(Directory.GetCurrentDirectory() + "\\Mocks\\Data\\small_image.png"),
                TinyBlobProperty = File.ReadAllBytes(Directory.GetCurrentDirectory() + "\\Mocks\\Data\\small_image.png"),
                BlobProperty = File.ReadAllBytes(Directory.GetCurrentDirectory() + "\\Mocks\\Data\\image.png"),
                MediumBlobProperty = File.ReadAllBytes(Directory.GetCurrentDirectory() + "\\Mocks\\Data\\large_image.png"),
                LongBlobProperty = File.ReadAllBytes(Directory.GetCurrentDirectory() + "\\Mocks\\Data\\large_image.png"),
                VarBinaryProperty = File.ReadAllBytes(Directory.GetCurrentDirectory() + "\\Mocks\\Data\\small_image.png")
            };
        }

        public override ITableTypeDescriptor GetDescriptor()
        {
            return DescriptorCache.Instance.GetTableTypeDescriptor(typeof(AllColumnsClass));
        }
    }
}
