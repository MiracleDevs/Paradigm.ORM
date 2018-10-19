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
        private string ConnectionString => "Server=192.168.2.90;User Id=test;Password=test1234;Timeout=3;Database=test";

        public int Ids { get; set; }

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
            this.Connector.ExecuteNonQuery(@"DROP TABLE IF EXISTS ""TwoPrimaryKeyTable"";");
        }

        public override void CreateTables()
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

            this.Connector.ExecuteNonQuery(
                @"CREATE TABLE IF NOT EXISTS ""TwoPrimaryKeyTable""
                (
                    ""Id1""           int,
                    ""Id2""           int,
                    ""Name""          text,

                   CONSTRAINT ""PK_TwoPrimaryKeyTable"" PRIMARY KEY (""Id1"", ""Id2"")
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
