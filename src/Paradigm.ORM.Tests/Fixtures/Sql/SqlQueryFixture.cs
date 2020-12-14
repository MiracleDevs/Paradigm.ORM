using System;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Descriptors;
using Paradigm.ORM.Data.Extensions;
using Paradigm.ORM.Tests.Mocks.Sql;
using System.Collections.Generic;
using Paradigm.ORM.Data.SqlServer;

namespace Paradigm.ORM.Tests.Fixtures.Sql
{
    public class SqlQueryFixture: QueryFixtureBase
    {
        private string ConnectionString => ConnectionStrings.MsSql;

        public override string SelectClause => "SELECT * FROM [Test].[dbo].[SingleKeyParentTable]";

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
        }

        public override void CreateParentTable()
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
        }

        public override void CreateChildTable()
        {
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
        }

        public override object CreateNewEntity()
        {
            return new SingleKeyParentTable
            {
                Name = $"Test Parent 1 {Guid.NewGuid()}",
                IsActive = true,
                Amount = (decimal)30.34,
                CreatedDate = new DateTime(2017, 4, 12),
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

        public override object CreateNewEntity2()
        {
            return new SingleKeyParentTable
            {
                Name = $"Test Parent 2 {Guid.NewGuid()}",
                IsActive = false,
                Amount = 215.50m,
                CreatedDate = new DateTime(2017, 6, 21),
                Childs = new List<SingleKeyChildTable>
                {
                    new SingleKeyChildTable
                    {
                        Name = $"Test Child 1 {Guid.NewGuid()}",
                        IsActive = false,
                        Amount = 100.25m,
                        CreatedDate = new DateTime(2017, 6, 22),
                    },
                    new SingleKeyChildTable
                    {
                        Name = $"Test Child 2 {Guid.NewGuid()}",
                        IsActive = true,
                        Amount = 115.25m,
                        CreatedDate = new DateTime(2017, 6, 23),
                    }
                }
            };
        }

        public override ITableTypeDescriptor GetDescriptor()
        {
            return DescriptorCache.Instance.GetTableTypeDescriptor(typeof(AllColumnsClass));
        }
    }
}
