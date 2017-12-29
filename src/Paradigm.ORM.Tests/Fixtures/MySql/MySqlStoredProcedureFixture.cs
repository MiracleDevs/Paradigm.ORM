using System;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Descriptors;
using Paradigm.ORM.Data.Extensions;
using Paradigm.ORM.Tests.Mocks.MySql;
using System.Collections.Generic;
using Paradigm.ORM.Data.MySql;

namespace Paradigm.ORM.Tests.Fixtures.MySql
{
    public class MySqlStoredProcedureFixture : StoredProcedureFixtureBase
    {
        private string ConnectionString => "Server=localhost;Database=test;User=test;Password=test1234;Connection Timeout=3600;Allow User Variables=True;POOLING=true";

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
                    `IsActive`      TINYINT            NOT NULL,
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
                    `IsActive`      TINYINT         NOT NULL,
                    `Amount`        DECIMAL(20,9)   NOT NULL,
                    `CreatedDate`   DATETIME        NOT NULL,

                    CONSTRAINT `PK_SingleKeyChildTable` PRIMARY KEY (`Id` ASC),
	                CONSTRAINT `UX_SingleKeyChildTable_Name` UNIQUE (`Name`),
                    CONSTRAINT `FK_SingleKeyChildTable_Parent` FOREIGN KEY (`ParentId`) REFERENCES `singlekeyparenttable` (`Id`)

                )ENGINE=INNODB;
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
                CREATE PROCEDURE `SearchParentTable`
                (
                    `ParentName`        NVARCHAR(200),
                    `Active`            TINYINT
                )
                BEGIN
                    SELECT *
                    FROM `test`.`singlekeyparenttable` t
                    WHERE t.`Name` like concat('%', `ParentName`, '%')
                          AND t.`IsActive` = `Active`;
                END;");

            this.Connector.ExecuteNonQuery(@"
                CREATE PROCEDURE `UpdateRoutine`
                (
	                `Id`        INT
                )
                BEGIN
	                UPDATE `SingleKeyParentTable` AS skpt
	                SET skpt.Name = 'Test Parent ChangedNameTest'
	                WHERE skpt.Id = Id;
                END");

            this.Connector.ExecuteNonQuery(@"
                CREATE PROCEDURE `SearchParentsAndChilds`
                (
	                `ParentName`        NVARCHAR(200),
	                `Active`            TINYINT
                )
                BEGIN
                SELECT * FROM `SingleKeyParentTable` AS skpt
                WHERE skpt.Name like concat('%', ParentName, '%') 
	                  AND skpt.IsActive = Active;
                SELECT * FROM `SingleKeyChildTable` AS skct
                WHERE skct.Name like concat('%', ParentName, '%') 
                      AND skct.IsActive = Active;
                END");

            this.Connector.ExecuteNonQuery(@"
               CREATE PROCEDURE `GetTotalAmount`
               (
	               `Active`	TINYINT
               )
               BEGIN
	               SELECT SUM(skpt.Amount)
	               FROM `SingleKeyParentTable` as skpt
                   WHERE skpt.IsActive = Active;
               END");
        }

        public override void DeleteStoredProcedures()
        {
            this.Connector.ExecuteNonQuery("DROP PROCEDURE IF EXISTS `SearchParentTable`;");
            this.Connector.ExecuteNonQuery("DROP PROCEDURE IF EXISTS `UpdateRoutine`;");
            this.Connector.ExecuteNonQuery("DROP PROCEDURE IF EXISTS `SearchParentsAndChilds`;");
            this.Connector.ExecuteNonQuery("DROP PROCEDURE IF EXISTS `GetTotalAmount`;");
        }
    }
}
