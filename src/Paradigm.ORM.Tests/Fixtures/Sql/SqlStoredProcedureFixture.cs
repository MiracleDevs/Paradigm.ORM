using System;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Descriptors;
using Paradigm.ORM.Data.Extensions;
using Paradigm.ORM.Tests.Mocks.Sql;
using System.Collections.Generic;
using Paradigm.ORM.Data.SqlServer;

namespace Paradigm.ORM.Tests.Fixtures.Sql
{
    public class SqlStoredProcedureFixture: StoredProcedureFixtureBase
    {
        protected override string ConnectionString => "Server=localhost;User=test;Password=test1234;Connection Timeout=3600";

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

        public override object CreateActiveChildEntity()
        {
            return new SingleKeyChildTable
            {
                Name = "Test Child " + Guid.NewGuid(),
                IsActive = true,
                Amount = new decimal(30.34),
                CreatedDate = DateTime.Today,
            };
        }

        public override object CreateNewActiveEntity()
        {
            return new SingleKeyParentTable
            {
                Name = $"Test Parent 1 {Guid.NewGuid()}",
                IsActive = true,
                Amount = (decimal)30.34,
                CreatedDate = new DateTime(2017, 4, 12),
                Childs = new List<SingleKeyChildTable>
                {
                    CreateActiveChildEntity() as SingleKeyChildTable,
                    CreateActiveChildEntity() as SingleKeyChildTable,
                }
            };
        }

        public override object CreateNewInactiveEntity()
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
                        IsActive = false,
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

        public override void CreateStoredProcedures()
        {
            this.Connector.ExecuteNonQuery(@"
                CREATE PROCEDURE [dbo].[SearchParentTable]
                    @ParentName NVARCHAR(200), 
                    @Active BIT 
                AS 
                BEGIN
	                SELECT *
	                FROM [dbo].[SingleKeyParentTable]
	                WHERE Name like concat('%', @ParentName, '%') AND  IsActive = @Active
                END");

            this.Connector.ExecuteNonQuery(@"
                CREATE PROCEDURE [dbo].[SearchParentsAndChilds]
                    @ParentName NVARCHAR(200), 
                    @Active BIT 
                AS 
                BEGIN
	                SELECT * FROM [dbo].[SingleKeyParentTable] WHERE Name like concat('%', @ParentName, '%') AND  IsActive = @Active;
                    SELECT * FROM [dbo].[SingleKeyChildTable] WHERE Name like concat('%', @ParentName, '%') AND  IsActive = @Active;
                END");

            this.Connector.ExecuteNonQuery(@"
                CREATE PROCEDURE [dbo].[UpdateRoutine]
                    @tId int
                AS
                BEGIN
	                UPDATE [dbo].[SingleKeyParentTable] 
                    SET Name = 'Test Parent ChangedNameTest' 
                    WHERE ID = @tId
                END");

            this.Connector.ExecuteNonQuery(@"
                CREATE PROCEDURE [dbo].[GetTotalAmount]
                    @Active BIT 
                AS 
                BEGIN
	                SELECT SUM(Amount)
	                FROM [dbo].[SingleKeyParentTable]
	                WHERE IsActive = @Active
                END");
        }

        public override void DeleteStoredProcedures()
        {
            this.Connector.ExecuteNonQuery(@"
                IF(OBJECT_ID('[dbo].[SearchParentTable]') IS NOT NULL)
                    DROP PROCEDURE [dbo].[SearchParentTable]");
            this.Connector.ExecuteNonQuery(@"
                IF(OBJECT_ID('[dbo].[GetTotalAmount]') IS NOT NULL)
                    DROP PROCEDURE [dbo].[GetTotalAmount]");
            this.Connector.ExecuteNonQuery(@"
                IF(OBJECT_ID('[dbo].[SearchParentsAndChilds]') IS NOT NULL)
                    DROP PROCEDURE [dbo].[SearchParentsAndChilds]");
            this.Connector.ExecuteNonQuery(@"
                IF(OBJECT_ID('[dbo].[UpdateRoutine]') IS NOT NULL)
                    DROP PROCEDURE [dbo].[UpdateRoutine]");
        }
    }
}
