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
        protected override string ConnectionString => "Server=192.168.2.160;User=test;Password=test1234;Connection Timeout=3600";

        public override string InsertParentStatement => "INSERT INTO [Test].[dbo].[SingleKeyParentTable] ([Name],[IsActive],[Amount],[CreatedDate]) VALUES (@Name,@IsActive,@Amount,@CreatedDate)";

        public override string LastInsertedIdStatement => "SELECT SCOPE_IDENTITY()";

        public override string SelectStatement => "SELECT [Id],[Name],[IsActive],[Amount],[CreatedDate] FROM [Test].[dbo].[SingleKeyParentTable]";

        public override string SelectOneStatement => "SELECT [Id],[Name],[IsActive],[Amount],[CreatedDate] FROM [Test].[dbo].[SingleKeyParentTable] WHERE [Id]=@Id";

        public override string DeleteStatement => @"DELETE FROM [Test].[dbo].[SingleKeyParentTable] WHERE [Id] IN (1,2)";

        public override string UpdateStatement => @"UPDATE [Test].[dbo].[SingleKeyParentTable] SET [Name]=@Name,[IsActive]=@IsActive,[Amount]=@Amount,[CreatedDate]=@CreatedDate WHERE [Id]=@Id";

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

        public override ITableTypeDescriptor GetParentDescriptor()
        {
            return new TableTypeDescriptor(typeof(SingleKeyParentTable));
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
