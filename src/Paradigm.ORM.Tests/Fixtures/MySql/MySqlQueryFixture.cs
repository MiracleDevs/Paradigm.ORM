using System;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Descriptors;
using Paradigm.ORM.Data.Extensions;
using Paradigm.ORM.Tests.Mocks.MySql;
using Paradigm.ORM.Data.MySql;
using System.Collections.Generic;

namespace Paradigm.ORM.Tests.Fixtures.MySql
{
    public class MySqlQueryFixture: QueryFixtureBase
    {
        private string ConnectionString => ConnectionStrings.MySql;

        public override string SelectClause => "SELECT * FROM `test`.`single_key_parent_table`";

        protected override IDatabaseConnector CreateConnector()
        {
            return new MySqlDatabaseConnector(this.ConnectionString);
        }

        public override void CreateDatabase()
        {
            this.Connector.ExecuteNonQuery(@"
                CREATE DATABASE IF NOT EXISTS `test`;
                USE `test`;
            ");
        }

        public override void DropDatabase()
        {
            this.Connector.ExecuteNonQuery("DROP TABLE IF EXISTS `single_key_child_table`;");
            this.Connector.ExecuteNonQuery("DROP TABLE IF EXISTS `single_key_parent_table`;");
        }

        public override void CreateParentTable()
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
        }

        public override void CreateChildTable()
        {
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
        }

        public override object CreateNewEntity()
        {
            return new SingleKeyParentTable
            {
                Name = $"Test Parent 1 {Guid.NewGuid()}",
                IsActive = true,
                Amount = (decimal)30.34,
                CreatedDate = new DateTime(2017, 4, 12, 16, 25, 56, 0),
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
                CreatedDate = new DateTime(2017, 6, 21, 13, 55, 46, 0),
                Childs = new List<SingleKeyChildTable>
                {
                    new SingleKeyChildTable
                    {
                        Name = $"Test Child 1 {Guid.NewGuid()}",
                        IsActive = false,
                        Amount = 100.25m,
                        CreatedDate = new DateTime(2017, 6, 22, 13, 55, 46, 0),
                    },
                    new SingleKeyChildTable
                    {
                        Name = $"Test Child 2 {Guid.NewGuid()}",
                        IsActive = true,
                        Amount = 115.25m,
                        CreatedDate = new DateTime(2017, 6, 23, 13, 55,46, 0),
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
