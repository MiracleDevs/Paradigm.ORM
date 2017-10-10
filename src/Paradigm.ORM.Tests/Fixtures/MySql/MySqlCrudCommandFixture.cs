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
        protected override string ConnectionString => "Server=192.168.2.160;Database=test;User=test;Password=test1234;Connection Timeout=3600;Allow User Variables=True;POOLING=true";

        public override string InsertParentStatement => "INSERT INTO `test`.`singlekeyparenttable` (`Name`,`IsActive`,`Amount`,`CreatedDate`) VALUES (@Name,@IsActive,@Amount,@CreatedDate)";

        public override string LastInsertedIdStatement => "SELECT LAST_INSERT_ID()";

        public override string SelectStatement => "SELECT `Id`,`Name`,`IsActive`,`Amount`,`CreatedDate` FROM `test`.`singlekeyparenttable`";

        public override string SelectOneStatement => "SELECT `Id`,`Name`,`IsActive`,`Amount`,`CreatedDate` FROM `test`.`singlekeyparenttable` WHERE `Id`=@Id";

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
            this.Connector.ExecuteNonQuery("DROP TABLE IF EXISTS `singlekeychildtable`;");
            this.Connector.ExecuteNonQuery("DROP TABLE IF EXISTS `singlekeyparenttable`;");
        }

        public override void CreateParentTable()
        {
            this.Connector.ExecuteNonQuery(@"
                CREATE TABLE IF NOT EXISTS `test`.`singlekeyparenttable`
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
                CREATE TABLE IF NOT EXISTS `test`.`singlekeychildtable`
                (
                    `Id`            INT             NOT NULL AUTO_INCREMENT,
                    `ParentId`      INT             NOT NULL,
                    `Name`          NVARCHAR(200)   NOT NULL,
                    `IsActive`      BOOL            NOT NULL,
                    `Amount`        DECIMAL(20,9)   NOT NULL,
                    `CreatedDate`   DATETIME        NOT NULL,

                    CONSTRAINT `PK_SingleKeyChildTable` PRIMARY KEY (`Id` ASC),
	                CONSTRAINT `UX_SingleKeyChildTable_Name` UNIQUE (`Name`),
                    CONSTRAINT `FK_SingleKeyChildTable_Parent` FOREIGN KEY (`ParentId`) REFERENCES `singlekeyparenttable` (`Id`)

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

        public override ITableTypeDescriptor GetParentDescriptor()
        {
            return new TableTypeDescriptor(typeof(SingleKeyParentTable));
        }
    }
}
