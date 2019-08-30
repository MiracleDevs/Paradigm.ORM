using System;
using System.Collections.Generic;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Descriptors;
using Paradigm.ORM.Data.Extensions;
using Paradigm.ORM.Data.SqlServer;
using Paradigm.ORM.Tests.Mocks.Sql;

namespace Paradigm.ORM.Tests.Fixtures.Sql
{
    public class SqlCrudCommandFixture: CrudCommandFixtureBase
    {
        private string ConnectionString => "Server=.;User=test;Password=test1234;Connection Timeout=3600";

        public int Ids { get; set; }

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
                IF (OBJECT_ID('[dbo].[SingleKeyChildTable]') IS NOT NULL)
                    DROP TABLE [dbo].[SingleKeyChildTable]");

            this.Connector.ExecuteNonQuery(@"
                IF (OBJECT_ID('[dbo].[SingleKeyParentTable]') IS NOT NULL)
                    DROP TABLE [dbo].[SingleKeyParentTable]");

            this.Connector.ExecuteNonQuery(@"
                IF (OBJECT_ID('[dbo].[TwoPrimaryKeyTable]') IS NOT NULL)
                    DROP TABLE [dbo].[TwoPrimaryKeyTable]");
        }

        public override void CreateTables()
        {
            this.Connector.ExecuteNonQuery(@"
                IF (OBJECT_ID('[dbo].[SingleKeyParentTable]') IS NULL)
                CREATE TABLE [SingleKeyParentTable]
                (
                    [Id]            INT             NOT NULL IDENTITY,
                    [Name]          NVARCHAR(200)   NOT NULL,
                    [IsActive]      BIT				NOT NULL,
                    [Amount]        DECIMAL(20,9)   NOT NULL,
                    [CreatedDate]   DATETIME        NOT NULL,

                    CONSTRAINT [PK_SingleKeyParentTable] PRIMARY KEY ([Id] ASC),
	                CONSTRAINT [UX_SingleKeyParentTable_Name] UNIQUE ([Name])
                );
            ");

            this.Connector.ExecuteNonQuery(@"
                IF (OBJECT_ID('[dbo].[SingleKeyChildTable]') IS NULL)
                CREATE TABLE [SingleKeyChildTable]
                (
                    [Id]            INT             NOT NULL IDENTITY,
                    [ParentId]      INT             NOT NULL,
                    [Name]          NVARCHAR(200)   NOT NULL,
                    [IsActive]      BIT				NOT NULL,
                    [Amount]        DECIMAL(20,9)   NOT NULL,
                    [CreatedDate]   DATETIME        NOT NULL,

                    CONSTRAINT [PK_SingleKeyChildTable] PRIMARY KEY ([Id] ASC),
	                CONSTRAINT [UX_SingleKeyChildTable_Name] UNIQUE ([Name]),
                    CONSTRAINT [FK_SingleKeyChildTable_Parent] FOREIGN KEY ([ParentId]) REFERENCES [SingleKeyParentTable] ([Id])
                );
            ");

            this.Connector.ExecuteNonQuery(@"
                IF (OBJECT_ID('[dbo].[TwoPrimaryKeyTable]') IS NULL)
                CREATE TABLE [TwoPrimaryKeyTable]
                (
                    [Id1]            INT             NOT NULL,
                    [Id2]            INT             NOT NULL,
                    [Name]           NVARCHAR(200)   NOT NULL,

                    CONSTRAINT [PK_TwoPrimaryKeyTable] PRIMARY KEY ([Id1] ASC, [Id2] ASC)
                );
            ");
        }

        public override object CreateNewEntity()
        {
            return new SingleKeyParentTable
            {
                Name = "Test Parent " + Guid.NewGuid(),
                IsActive = true,
                Amount = (decimal)30.34,
                CreatedDate = DateTime.Today,
                Childs = new List<SingleKeyChildTable>
                {
                    new SingleKeyChildTable
                    {
                        Name = "Test Child " + Guid.NewGuid(),
                        IsActive = true,
                        Amount = new decimal(30.34),
                        CreatedDate = DateTime.Today,
                    }
                }
            };
        }

        public override object CreateNewTwoKeysEntity()
        {
            return new TwoPrimaryKeyTable()
            {
                Id1 = ++this.Ids,
                Id2 = ++this.Ids,
                Name = "Test Parent " + Guid.NewGuid(),
            };
        }

        public override ITableTypeDescriptor GetParentDescriptor()
        {
            return DescriptorCache.Instance.GetTableTypeDescriptor(typeof(SingleKeyParentTable));
        }

        public override ITableTypeDescriptor GetMultipleKeyDescriptor()
        {
            return DescriptorCache.Instance.GetTableTypeDescriptor(typeof(TwoPrimaryKeyTable));
        }

        public override void SetEntityId(object first, object second)
        {
            ((SingleKeyParentTable)first).Id = 1;
            ((SingleKeyParentTable)second).Id = 2;
        }

        public override void Update(object first, object second)
        {
            ((SingleKeyParentTable)first).Name = "Updated Parent " + Guid.NewGuid();
            ((SingleKeyParentTable)second).Name = "Updated Parent " + Guid.NewGuid();
        }

        public override void CheckUpdate(object first, object second)
        {
            if (!((SingleKeyParentTable)first).Name.StartsWith("Updated Parent") ||
                !((SingleKeyParentTable)second).Name.StartsWith("Updated Parent"))
            {
                throw new Exception("Entities not updated.");
            }
        }
    }
}
