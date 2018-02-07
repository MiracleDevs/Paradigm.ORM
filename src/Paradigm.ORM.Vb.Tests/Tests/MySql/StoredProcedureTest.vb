Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Paradigm.ORM.Data.DatabaseAccess.Generic
Imports Paradigm.ORM.Data.Extensions
Imports Paradigm.ORM.Data.MySql
Imports Paradigm.ORM.Vb.Tests.Mocks

Namespace Tests.MySql

    <TestClass>
    Public Class StoredProcedureTest

        Public Property Connector As MySqlDatabaseConnector

        <TestInitialize()>
        Public Sub Initialize()

            Connector = New MySqlDatabaseConnector("Server=localhost;Database=Test;User=test;Password=test1234;Connection Timeout=3600")
            Connector.Open()

            Connector.ExecuteNonQuery("
        CREATE TABLE IF NOT EXISTS `zone`
        (
            `Id`                    INT AUTO_INCREMENT NOT NULL,
            `CompanyId`             INT,
            `SubZoneId`             INT NOT NULL,
            `Code`                  VARCHAR(50) NOT NULL,
            `Name`                  VARCHAR(200) NOT NULL,
            `Description`           TEXT NOT NULL,
            `Latitude`              DECIMAL(11, 8),
            `Longitude`             DECIMAL(11, 8),
            `StatusId`              INT NOT NULL,
            `ParentZoneId`          INT,

            CONSTRAINT `PK_zone` PRIMARY KEY (`Id` ASC)
        )ENGINE=INNODB;")

            Connector.ExecuteNonQuery("DROP PROCEDURE IF EXISTS `search_zones`")
            Connector.ExecuteNonQuery("
        CREATE PROCEDURE `search_zones`
        (
	        `Keyword`			    VARCHAR(200)
        )
        BEGIN
            SELECT * FROM `zone` z WHERE z.`Name` LIKE CONCAT('%', `Keyword`, '%');
        END")

        End Sub

        <TestCleanup()>
        Public Sub Cleanup()

            Connector.ExecuteNonQuery("DROP PROCEDURE IF EXISTS `search_zones`")
            Connector.ExecuteNonQuery("DROP TABLE IF EXISTS `zone`")

        End Sub

        <TestMethod()>
        Public Sub ShouldExecuteStoredProcedure()

            Dim zoneDatabaseAccess = New DatabaseAccess(Of Zone)(Connector)

            Dim newZone As Zone = New Zone With {
                    .CompanyId = 1,
                    .StatusId = 1,
                    .SubZoneId = 0,
                    .ParentZoneId = Nothing,
                    .Code = "Z1",
                    .Name = "Zone 1",
                    .Description = "Zone 1",
                    .Latitude = 30.45434543D,
                    .Longitude = -101.2452343D
                    }

            zoneDatabaseAccess.Insert(newZone)

            Dim parameters = New SearchZonesParameters With{ .Keyword = "z" }
            Dim procedure = new SearchZonesStoredProcedure(Connector)

            Dim zones = procedure.Execute(parameters)

            Assert.AreEqual(1, zones.Count)
        End Sub

    End Class
End NameSpace