using System;
using System.Collections.Generic;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Descriptors;
using Paradigm.ORM.Data.Extensions;
using Paradigm.ORM.Data.PostgreSql;
using Paradigm.ORM.Tests.Mocks.PostgreSql;

namespace Paradigm.ORM.Tests.Fixtures.PostgreSql
{
    public class PostgreSqlCrudCommandFixture: CrudCommandFixtureBase
    {
        protected override string ConnectionString => "Server=192.168.2.160;User Id=test;Password=test1234;Timeout=3";

        public override string InsertParentStatement => @"INSERT INTO ""SingleKeyParentTable"" (""Name"",""IsActive"",""Amount"",""CreatedDate"") VALUES (@Name,@IsActive,@Amount,@CreatedDate)";

        public override string LastInsertedIdStatement => "SELECT LASTVAL()";

        public override string SelectStatement => @"SELECT ""Id"",""Name"",""IsActive"",""Amount"",""CreatedDate"" FROM ""SingleKeyParentTable""";

        public override string SelectOneStatement =>@"SELECT ""Id"",""Name"",""IsActive"",""Amount"",""CreatedDate"" FROM ""SingleKeyParentTable"" WHERE ""Id""=@Id";

        public override string DeleteStatement => @"DELETE FROM ""SingleKeyParentTable"" WHERE ""Id"" IN (1,2)";

        public override string UpdateStatement => @"UPDATE ""SingleKeyParentTable"" SET ""Id""=@Id,""Name""=@Name,""IsActive""=@IsActive,""Amount""=@Amount,""CreatedDate""=@CreatedDate WHERE ""Id""=@Id";

        protected override IDatabaseConnector CreateConnector()
        {
            return new PostgreSqlDatabaseConnector(this.ConnectionString);
        }

        public override void CreateDatabase()
        {
        }

        public override void DropDatabase()
        {
            this.Connector.ExecuteNonQuery(@"DROP TABLE IF EXISTS ""SingleKeyChildTable"";");
            this.Connector.ExecuteNonQuery(@"DROP TABLE IF EXISTS ""SingleKeyParentTable"";");
        }

        public override void CreateParentTable()
        {
            this.Connector.ExecuteNonQuery(
                @"CREATE TABLE IF NOT EXISTS ""SingleKeyParentTable""(
                    ""Id""            SERIAL, 
                    ""Name""          VARCHAR(200)     NOT NULL,
                    ""IsActive""      BOOLEAN          NOT NULL,
                    ""Amount""        DECIMAL(20,9)    NOT NULL,
                    ""CreatedDate""   DATE             NOT NULL,

                    CONSTRAINT ""PK_SingleKeyParentTable"" PRIMARY KEY (""Id""),
                    CONSTRAINT ""UX_SingleKeyParentTable_Name"" UNIQUE (""Name"")
                );");
        }

        public override void CreateChildTable()
        {
            this.Connector.ExecuteNonQuery(
                @"CREATE TABLE IF NOT EXISTS ""SingleKeyChildTable""(
                    ""Id""            SERIAL,
                    ""ParentId""      INT             NOT NULL,
                    ""Name""          VARCHAR(200)    NOT NULL,
                    ""IsActive""      BOOLEAN         NOT NULL,
                    ""Amount""        DECIMAL(20,9)   NOT NULL,
                    ""CreatedDate""   DATE            NOT NULL,

                    CONSTRAINT ""PK_SingleKeyChildTable"" PRIMARY KEY (""Id""),
                    CONSTRAINT ""UX_SingleKeyChildTable_Name"" UNIQUE (""Name""),
                    CONSTRAINT ""FK_SingleKeyChildTable_Parent"" FOREIGN KEY (""ParentId"") REFERENCES ""SingleKeyParentTable"" (""Id"")
                );");
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
