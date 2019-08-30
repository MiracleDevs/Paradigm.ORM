using System;
using FluentAssertions;
using Paradigm.ORM.Data.DatabaseAccess;
using Paradigm.ORM.Data.StoredProcedures;
using Paradigm.ORM.Tests.Fixtures.MySql;
using Paradigm.ORM.Tests.Mocks.MySql;
using Paradigm.ORM.Tests.Mocks.MySql.Routines;
using NUnit.Framework;

namespace Paradigm.ORM.Tests.Tests.StoredProcedures.MySql
{
    [TestFixture]
    public class MySqlStoredProceduresTest
    {
        private MySqlStoredProcedureFixture Fixture { get; set; }

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            // We first connect to create the database with a connection string that does not contain the database parameter
            // to create the database, and the we use the the connection with the database specified.
            // We do this due to a bug in MySql. If we do not specify the database in the connection string we can not execute
            // the stored procedures.
            // TODO Remove this lines when the bug is fixed.
            /*using (var mySqlNoDbConnector = new MySqlDatabaseConnector(this.ConnectionStringNoDb)) {
                mySqlNoDbConnector.ExecuteNonQuery(@"CREATE DATABASE IF NOT EXISTS `test`;");
                mySqlNoDbConnector.Close();
            }*/

            Fixture = new MySqlStoredProcedureFixture();

            if (Fixture == null)
                throw new Exception("Couldn't create the fixture.");

            // TODO Uncomment this line when the bug is fixed.
            //Fixture.CreateDatabase();
            Fixture.CreateParentTable();
            Fixture.CreateChildTable();
            Fixture.CreateStoredProcedures();

            var databaseAccess = new DatabaseAccess(Fixture.Connector, typeof(SingleKeyParentTable));

            databaseAccess.Insert(Fixture.CreateNewActiveEntity());
            databaseAccess.Insert(Fixture.CreateNewActiveEntity());
            databaseAccess.Insert(Fixture.CreateNewInactiveEntity());
            databaseAccess.Insert(Fixture.CreateNewInactiveEntity());
        }

        [Test]
        public void ShouldRetrieveScalarResultOnActiveClients()
        {
            var args = new GetTotalAmountParameters
            {
                Active = true
            };
            var activeEntity = Fixture.CreateNewActiveEntity() as SingleKeyParentTable;

            var result = new ScalarStoredProcedure<GetTotalAmountParameters, decimal>(Fixture.Connector).ExecuteScalar(args);

            result.Should().Be(activeEntity.Amount * 2);
        }

        [Test]
        public void ShouldRetrieveScalarResultOnInactiveClients()
        {
            var args = new GetTotalAmountParameters
            {
                Active = false
            };
            var notActiveEntity = Fixture.CreateNewInactiveEntity() as SingleKeyParentTable;

            var result = new ScalarStoredProcedure<GetTotalAmountParameters, decimal>(Fixture.Connector).ExecuteScalar(args);

            result.Should().Be(notActiveEntity.Amount * 2);
        }

        [Test]
        public void ShouldRetrieveMultipleResults()
        {
            var args = new SearchParentsAndChildsParameters
            {
                ParentName = "Test",
                Active = true
            };

            var results = new ReaderStoredProcedure<SearchParentsAndChildsParameters, SingleKeyParentTable, SingleKeyChildTable>(Fixture.Connector).Execute(args);

            results.Should().NotBeNull();
            results.Item1.Should().NotBeNull();
            results.Item1.Should().HaveCount(2);
            results.Item2.Should().NotBeNull();
            results.Item2.Should().HaveCount(4);

            var activeParentEntity = Fixture.CreateNewActiveEntity() as SingleKeyParentTable;

            foreach (var entity in results.Item1)
            {
                entity.Name.Should().StartWith("Test Parent");
                entity.Should().BeEquivalentTo(activeParentEntity, options => options.Excluding(o => o.Name)
                                                                                     .Excluding(o => o.Id)
                                                                                     .Excluding(o => o.Childs));
            }

            var activeChildEntity = Fixture.CreateActiveChildEntity() as SingleKeyChildTable;
            foreach (var entity in results.Item2)
            {
                entity.Name.Should().StartWith("Test Child");
                entity.Should().BeEquivalentTo(activeChildEntity, options => options.Excluding(o => o.Name)
                                                                                    .Excluding(o => o.Id)
                                                                                    .Excluding(o => o.ParentId));
            }
        }

        [Test]
        public void ShouldRetrieveTwoResultsOnActiveClients()
        {
            var args = new SearchParentTableParameters
            {
                ParentName = "Test Par",
                Active = true
            };

            var results = new ReaderStoredProcedure<SearchParentTableParameters, SingleKeyParentTable>(Fixture.Connector).Execute(args);

            results.Should().NotBeNull();
            results.Should().HaveCount(2);

            var activeParentEntity = Fixture.CreateNewActiveEntity() as SingleKeyParentTable;
            foreach (var entity in results)
            {
                entity.Name.Should().StartWith("Test Parent");
                entity.Should().BeEquivalentTo(activeParentEntity, options => options.Excluding(o => o.Name)
                                                                                  .Excluding(o => o.Id)
                                                                                  .Excluding(o => o.Childs));
            }
        }

        [Test]
        public void ShouldRetrieveTwoResultsOnInactiveClients()
        {
            var args = new SearchParentTableParameters
            {
                ParentName = "Test Par",
                Active = false
            };

            var results = new ReaderStoredProcedure<SearchParentTableParameters, SingleKeyParentTable>(Fixture.Connector).Execute(args);

            results.Should().NotBeNull();
            results.Should().HaveCount(2);

            var activeParentEntity = Fixture.CreateNewInactiveEntity() as SingleKeyParentTable;
            foreach (var entity in results)
            {
                entity.Name.Should().StartWith("Test Parent");
                entity.Should().BeEquivalentTo(activeParentEntity, options => options.Excluding(o => o.Name)
                                                                                  .Excluding(o => o.Id)
                                                                                  .Excluding(o => o.Childs));
            }
        }

        [Test]
        public void NonQueryProcedureShouldModifyEntity()
        {
            var updateRoutineParametersArgs = new UpdateRoutineParameters
            {
                Id = 1
            };

            var nonQuery = new NonQueryStoredProcedure<UpdateRoutineParameters>(Fixture.Connector);
            nonQuery.ExecuteNonQuery(updateRoutineParametersArgs);

            var searchClientArgs = new SearchParentTableParameters
            {
                ParentName = "Test Parent ChangedNameTest",
                Active = true
            };
            var results = new ReaderStoredProcedure<SearchParentTableParameters, SingleKeyParentTable>(Fixture.Connector).Execute(searchClientArgs);

            results.Should().NotBeNull();
            results.Should().HaveCount(1);
            results[0].Name.Should().Be("Test Parent ChangedNameTest");
        }

        [Test]
        public void ShouldThrowArgumentException()
        {
            Action results = () => new ReaderStoredProcedure<SearchParentTableParameters, SingleKeyParentTable>(Fixture.Connector).Execute(null);
            results.Should().Throw<ArgumentNullException>();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Fixture.DeleteStoredProcedures();
            Fixture.DropDatabase();
            Fixture.Dispose();
        }
    }

    /*
CREATE DATABASE IF NOT EXISTS `test`;
USE `test`;

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


DROP PROCEDURE IF EXISTS `SearchParentTable`;
DELIMITER $$
CREATE PROCEDURE `SearchParentTable`
(
    `ParentName`        VARCHAR(200),
    `Active`            TINYINT
)
BEGIN

  SELECT *
    FROM `test`.`singlekeyparenttable` t
   WHERE t.`Name` like concat('%', `ParentName`, '%')
     AND t.`IsActive` = `Active`;

END$$
DELIMITER ;
*/
}
