using System;
using System.Collections.Generic;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Descriptors;
using Paradigm.ORM.Data.Extensions;
using Paradigm.ORM.Tests.Mocks.MySql;
using Paradigm.ORM.Data.MySql;

namespace Paradigm.ORM.Tests.Fixtures.MySql
{
    public class MySqlCrudCommandFixture: CrudCommandFixtureBase
    {
        private string ConnectionString => "Server=localhost;Database=test;User=root;Password=Paradigm_Test_1234;Connection Timeout=3600;Allow User Variables=True;POOLING=true";

        private int Ids { get; set; }

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
            this.Connector.ExecuteNonQuery("DROP TABLE IF EXISTS `single_key_child_table`;");
            this.Connector.ExecuteNonQuery("DROP TABLE IF EXISTS `single_key_parent_table`;");
            this.Connector.ExecuteNonQuery("DROP TABLE IF EXISTS `two_primary_key_table`;");
        }

        public override void CreateTables()
        {
            this.Connector.ExecuteNonQuery(@"
                CREATE TABLE IF NOT EXISTS `test`.`single_key_parent_table`
                (
                    `Id`            INT             NOT NULL AUTO_INCREMENT,
                    `Name`          NVARCHAR(200)   NOT NULL,
                    `IsActive`      BOOL            NOT NULL,
                    `Amount`        DECIMAL(20,9)   NOT NULL,
                    `CreatedDate`   DATETIME        NOT NULL,

                    CONSTRAINT `PK_SingleKeyParentTable` PRIMARY KEY (`Id` ASC),
	                CONSTRAINT `UX_SingleKeyParentTable_Name` UNIQUE (`Name`)

                )ENGINE=INNODB;
            ");

            this.Connector.ExecuteNonQuery(@"
                CREATE TABLE IF NOT EXISTS `test`.`single_key_child_table`
                (
                    `Id`            INT             NOT NULL AUTO_INCREMENT,
                    `ParentId`      INT             NOT NULL,
                    `Name`          NVARCHAR(200)   NOT NULL,
                    `IsActive`      BOOL            NOT NULL,
                    `Amount`        DECIMAL(20,9)   NOT NULL,
                    `CreatedDate`   DATETIME        NOT NULL,

                    CONSTRAINT `PK_SingleKeyChildTable` PRIMARY KEY (`Id` ASC),
	                CONSTRAINT `UX_SingleKeyChildTable_Name` UNIQUE (`Name`),
                    CONSTRAINT `FK_SingleKeyChildTable_Parent` FOREIGN KEY (`ParentId`) REFERENCES `single_key_parent_table` (`Id`)

                )ENGINE=INNODB;
            ");

            this.Connector.ExecuteNonQuery(@"
                CREATE TABLE IF NOT EXISTS `test`.`two_primary_key_table`
                (
                    `Id1`            INT             NOT NULL,
                    `Id2`            INT             NOT NULL,
                    `Name`           NVARCHAR(200)   NOT NULL,

                    CONSTRAINT `PK_TwoPrimaryKeyTable` PRIMARY KEY (`Id1` ASC, `Id2` ASC)
                )ENGINE=INNODB;
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
