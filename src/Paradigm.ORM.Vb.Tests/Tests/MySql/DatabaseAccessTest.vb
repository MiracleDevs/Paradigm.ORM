
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Paradigm.ORM.Data.DatabaseAccess.Generic
Imports Paradigm.ORM.Data.Extensions
Imports Paradigm.ORM.Data.MySql
Imports Paradigm.ORM.Vb.Tests.Mocks

Namespace Tests.MySql

    <TestClass()>
    Public Class DatabaseAccessTest

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

        End Sub

        <TestCleanup()>
        Public Sub Cleanup()

            Connector.ExecuteNonQuery("DROP TABLE `zone`")

        End Sub

        <TestMethod()>
        Public Sub ShouldInsert()

            ' creates the database accesor
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

        End Sub

        <TestMethod()>
        Public Sub ShouldSelect()

            ' creates the database accesor
            Dim zoneDatabaseAccess = New DatabaseAccess(Of Zone)(Connector)

            ' 1. insert a register in the database
            Dim newZone1 As Zone = New Zone With {
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

            Dim newZone2 As Zone = New Zone With {
                    .CompanyId = 1,
                    .StatusId = 1,
                    .SubZoneId = 0,
                    .ParentZoneId = Nothing,
                    .Code = "Z2",
                    .Name = "Zone 2",
                    .Description = "Zone 2",
                    .Latitude = 30.45434543D,
                    .Longitude = -101.2452343D
                    }

            zoneDatabaseAccess.Insert(New Zone() { newZone1, newZone2 })

            ' 2. select the inserted register
            Dim zones = zoneDatabaseAccess.Select()

            Assert.AreEqual(2, zones.Count)
        End Sub

        <TestMethod()>
        Public Sub ShouldUpdate()

            ' creates the database accesor
            Dim zoneDatabaseAccess = New DatabaseAccess(Of Zone)(Connector)

            ' 1. insert a register in the database
            Dim newZone as Zone = new Zone With {
                    .CompanyId = 1,
                    .StatusId = 1,
                    .SubZoneId = 0,
                    .ParentZoneId = Nothing,
                    .Code = "Z1",
                    .Name = "Zone 1",
                    .Description = "Zone 1",
                    .Latitude = 30.45434543d,
                    .Longitude = -101.2452343d
                    }

            zoneDatabaseAccess.Insert(newZone)

            ' 2. select the inserted register and change a variable
            Dim zone2 = zoneDatabaseAccess.Select($"{NameOf(Zone.Code)}=@1", "Z1").First()

            zone2.Description = "Esta es la zone número 2"

            ' 3. update the modified register
            zoneDatabaseAccess.Update(zone2)

            ' 4. select the updated register again and check the changes where applied
            zone2 = zoneDatabaseAccess.Select($"{NameOf(Zone.Code)}=@1", "Z1").First()

            Assert.AreEqual("Esta es la zone número 2", zone2.Description)
        End Sub

        <TestMethod()>
        Public Sub ShouldRemove()

            ' creates the database accesor
            Dim zoneDatabaseAccess = New DatabaseAccess(Of Zone)(Connector)

            ' 1. insert a register in the database
            Dim newZone as Zone = new Zone With {
                    .CompanyId = 1,
                    .StatusId = 1,
                    .SubZoneId = 0,
                    .ParentZoneId = Nothing,
                    .Code = "Z1",
                    .Name = "Zone 1",
                    .Description = "Zone 1",
                    .Latitude = 30.45434543d,
                    .Longitude = -101.2452343d
                    }

            zoneDatabaseAccess.Insert(newZone)

            ' 2. select the inserted register
            Dim zone2 = zoneDatabaseAccess.Select($"{NameOf(Zone.Code)}=@1", "Z1").First()

            ' 3.delete the register from the database
            zoneDatabaseAccess.Delete(zone2)

            ' 4. select the deleted register again and check that the return is empty
            Assert.AreEqual(0, zoneDatabaseAccess.Select().Count)
        End Sub

    End Class
End NameSpace