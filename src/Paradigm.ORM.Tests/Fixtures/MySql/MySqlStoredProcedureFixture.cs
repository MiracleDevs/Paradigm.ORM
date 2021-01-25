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
        private string ConnectionString => ConnectionStrings.MySql;

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
        }

        public override void CreateParentTable()
        {
            this.Connector.ExecuteNonQuery(@"
                CREATE TABLE IF NOT EXISTS `test`.`single_key_parent_table`
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
                CREATE TABLE IF NOT EXISTS `test`.`single_key_child_table`
                (
                    `Id`            INT             NOT NULL AUTO_INCREMENT,
                    `ParentId`      INT             NOT NULL,
                    `Name`          NVARCHAR(200)   NOT NULL,
                    `IsActive`      TINYINT         NOT NULL,
                    `Amount`        DECIMAL(20,9)   NOT NULL,
                    `CreatedDate`   DATETIME        NOT NULL,

                    CONSTRAINT `PK_SingleKeyChildTable` PRIMARY KEY (`Id` ASC),
	                CONSTRAINT `UX_SingleKeyChildTable_Name` UNIQUE (`Name`),
                    CONSTRAINT `FK_SingleKeyChildTable_Parent` FOREIGN KEY (`ParentId`) REFERENCES `single_key_parent_table` (`Id`)

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
                CREATE PROCEDURE `search_parent_table`
                (
                    `ParentName`        NVARCHAR(200),
                    `Active`            TINYINT
                )
                BEGIN
                    SELECT *
                    FROM `test`.`single_key_parent_table` t
                    WHERE t.`Name` like concat('%', `ParentName`, '%')
                          AND t.`IsActive` = `Active`;
                END;");

            this.Connector.ExecuteNonQuery(@"
                CREATE PROCEDURE `update_routine`
                (
	                `Id`        INT
                )
                BEGIN
	                UPDATE `single_key_parent_table` AS skpt
	                SET skpt.Name = 'Test Parent ChangedNameTest'
	                WHERE skpt.Id = Id;
                END");

            this.Connector.ExecuteNonQuery(@"
                CREATE PROCEDURE `search_parents_and_childs`
                (
	                `ParentName`        NVARCHAR(200),
	                `Active`            TINYINT
                )
                BEGIN
                SELECT * FROM `single_key_parent_table` AS skpt
                WHERE skpt.Name like concat('%', ParentName, '%') 
	                  AND skpt.IsActive = Active;
                SELECT * FROM `single_key_child_table` AS skct
                WHERE skct.Name like concat('%', ParentName, '%') 
                      AND skct.IsActive = Active;
                END");

            this.Connector.ExecuteNonQuery(@"
               CREATE PROCEDURE `get_total_amount`
               (
	               `Active`	TINYINT
               )
               BEGIN
	               SELECT SUM(skpt.Amount)
	               FROM `single_key_parent_table` as skpt
                   WHERE skpt.IsActive = Active;
               END");

            this.Connector.ExecuteNonQuery(@"
               CREATE PROCEDURE `get_8_results`
               (
	               `Active`	TINYINT
               )
               BEGIN             
	               SELECT *
	               FROM `single_key_parent_table` as skpt
                   WHERE skpt.IsActive = Active;

                   SELECT *
	               FROM `single_key_parent_table` as skpt
                   WHERE skpt.IsActive = Active;

                   SELECT *
	               FROM `single_key_parent_table` as skpt
                   WHERE skpt.IsActive = Active;

                   SELECT *
	               FROM `single_key_parent_table` as skpt
                   WHERE skpt.IsActive = Active;

                   SELECT *
	               FROM `single_key_parent_table` as skpt
                   WHERE skpt.IsActive = Active;

                   SELECT *
	               FROM `single_key_parent_table` as skpt
                   WHERE skpt.IsActive = Active;

                   SELECT *
	               FROM `single_key_parent_table` as skpt
                   WHERE skpt.IsActive = Active;

                   SELECT *
	               FROM `single_key_parent_table` as skpt
                   WHERE skpt.IsActive = Active;
               END");
        }

        public override void DeleteStoredProcedures()
        {
            this.Connector.ExecuteNonQuery("DROP PROCEDURE IF EXISTS `search_parent_table`;");
            this.Connector.ExecuteNonQuery("DROP PROCEDURE IF EXISTS `update_routine`;");
            this.Connector.ExecuteNonQuery("DROP PROCEDURE IF EXISTS `search_parents_and_childs`;");
            this.Connector.ExecuteNonQuery("DROP PROCEDURE IF EXISTS `get_total_amount`;");
            this.Connector.ExecuteNonQuery("DROP PROCEDURE IF EXISTS `get_8_results`;");
        }
    }
}
