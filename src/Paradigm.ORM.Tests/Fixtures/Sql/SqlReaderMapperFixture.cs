using System;
using System.IO;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Descriptors;
using Paradigm.ORM.Data.Extensions;
using Paradigm.ORM.Data.SqlServer;
using Paradigm.ORM.Tests.Mocks.Sql;

namespace Paradigm.ORM.Tests.Fixtures.Sql
{
    public class SqlReaderMapperFixture : ReaderMapperFixtureBase
    {
        private string ConnectionString => "Server=.;User=test;Password=test1234;Connection Timeout=3600";

        public override string SelectStatement => @"SELECT [Id],[Name],[IsActive],[Amount],[CreatedDate],[BoolProperty],[TinyintProperty],
                                                           [SmallintProperty],[BigintProperty],[RealProperty],[FloatProperty],[MoneyProperty],
                                                           [SmallmoneyProperty],[NumericProperty],[DecimalProperty],[DateProperty],[DateTimeProperty],
                                                           [DateTime2Property],[SmallDateTimeProperty],[DateTimeOffsetProperty],[CharProperty],
                                                           [TextProperty],[VarcharProperty],[NCharProperty],[NTextProperty],[NVarcharProperty],[XmlProperty],
                                                           [BinaryProperty],[VarBinaryProperty],[ImageProperty]
                                                    FROM [Test].[dbo].[AllColumns]";

        protected override IDatabaseConnector CreateConnector()
        {
            return new SqlDatabaseConnector(this.ConnectionString);
        }

        public override void CreateDatabase()
        {
            this.Connector.ExecuteNonQuery(@"
                IF (NOT EXISTS (SELECT [name] FROM [master].[dbo].[sysdatabases] WHERE [name]='Test'))
                    CREATE DATABASE [Test]");

            this.Connector.ExecuteNonQuery(@"USE [Test]");
        }

        public override void DropDatabase()
        {
            this.Connector.ExecuteNonQuery(@"
                IF (OBJECT_ID('[dbo].[AllColumns]') IS NOT NULL)
                    DROP TABLE [dbo].[AllColumns]");         
        }

        public override void CreateTable()
        {
            this.Connector.ExecuteNonQuery(@"
                IF (OBJECT_ID('[dbo].[AllColumns]') IS NULL)
                CREATE TABLE [AllColumns]
                (
                    [Id]                            INT                 NOT NULL IDENTITY,
                    [Name]                          NVARCHAR(200)       NOT NULL,
                    [IsActive]                      BIT			    	NOT NULL,
                    [Amount]                        DECIMAL(20,9)       NOT NULL,
                    [CreatedDate]                   DATETIME            NOT NULL,
                    [BoolProperty]                  BIT                 NOT NULL,
                    [TinyintProperty]               TINYINT             NOT NULL,
                    [SmallintProperty]              SMALLINT            NOT NULL,
                    [BigintProperty]                BIGINT              NOT NULL,
                    [RealProperty]                  REAL                NOT NULL,
                    [FloatProperty]                 FLOAT               NOT NULL,
                    [MoneyProperty]                 MONEY               NOT NULL,
                    [SmallmoneyProperty]            SMALLMONEY          NOT NULL,
                    [NumericProperty]               NUMERIC(10,5)       NOT NULL,
                    [DecimalProperty]               DECIMAL(5,2)        NOT NULL,
                    [DateProperty]                  DATE                NOT NULL,
                    [DateTimeProperty]              DATETIME            NOT NULL,
                    [DateTime2Property]             DATETIME2           NOT NULL,
                    [SmallDateTimeProperty]         SMALLDATETIME       NOT NULL,
                    [DateTimeOffsetProperty]        DATETIMEOFFSET      NOT NULL,
                    [CharProperty]                  CHAR                NOT NULL,
                    [TextProperty]                  TEXT                NOT NULL,
                    [VarcharProperty]               VARCHAR(4)          NOT NULL,
                    [NCharProperty]                 NCHAR               NOT NULL,
                    [NTextProperty]                 NTEXT               NOT NULL,
                    [NVarcharProperty]              NVARCHAR(16)        NOT NULL,
                    [XmlProperty]                   XML                 NOT NULL,
                    [BinaryProperty]                BINARY(235)         NOT NULL,
                    [VarBinaryProperty]             BINARY(235)         NOT NULL,
                    [ImageProperty]                 IMAGE               NOT NULL,

                    CONSTRAINT [PK_AllColumns] PRIMARY KEY ([Id] ASC),
	                CONSTRAINT [UX_AllColumns_Name] UNIQUE ([Name])
                );
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
                BoolProperty = false,
                TinyintProperty = 23,
                SmallintProperty = 15000,
                BigintProperty = 154898789214,
                RealProperty = 3.14159f,
                FloatProperty = 3.14159232,
                MoneyProperty = 354687987.50m,
                SmallmoneyProperty = 25512.30m,
                NumericProperty = 12345.56789m,
                DecimalProperty = 123.56m,
                DateProperty = new DateTime(2017, 06, 23, 0, 0, 0, 0),
                DateTimeProperty = new DateTime(2017, 06, 23, 16, 45, 12, 0),
                DateTime2Property = new DateTime(2017, 06, 23, 16, 45, 12, 0),
                SmallDateTimeProperty = new DateTime(1900, 01, 01, 16, 45, 0, 0),
                DateTimeOffsetProperty = new DateTime(1900, 01, 01, 16, 45, 0, 0),
                //TimeSpanProperty = new TimeSpan(15, 22, 33, 0),
                CharProperty = "a",
                TextProperty = "This is a test for a text type field in Sql Server data base.",
                VarcharProperty = "Test",
                NCharProperty = "こ",
                NTextProperty = "これは、SQL Serverデータベースのテキスト型フィールドのテストです",
                NVarcharProperty = "これはテストです",
                XmlProperty = "<xml><tag property=\"1\">A text!</tag></xml>",
                BinaryProperty = File.ReadAllBytes(Directory.GetCurrentDirectory() + "\\Mocks\\Data\\small_image.png"),
                VarBinaryProperty = File.ReadAllBytes(Directory.GetCurrentDirectory() + "\\Mocks\\Data\\small_image.png"),
                ImageProperty = File.ReadAllBytes(Directory.GetCurrentDirectory() + "\\Mocks\\Data\\image.png"),
            };
        }

        public override ITableTypeDescriptor GetDescriptor()
        {
            return DescriptorCache.Instance.GetTableTypeDescriptor(typeof(AllColumnsClass));
        }
    }
}
