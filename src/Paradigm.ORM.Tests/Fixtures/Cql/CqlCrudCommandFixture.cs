using System;
using System.Collections.Generic;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Descriptors;
using Paradigm.ORM.Data.Extensions;
using Paradigm.ORM.Data.Cassandra;
using Paradigm.ORM.Tests.Mocks.Cql;

namespace Paradigm.ORM.Tests.Fixtures.Cql
{
    public class CqlCrudCommandFixture: CrudCommandFixtureBase
    {
        protected override string ConnectionString => "Contact Points=192.168.2.240;Port=9042";

        public override string InsertParentStatement => @"INSERT INTO ""test"".""singlekeyparenttable"" (""Id"",""Name"",""IsActive"",""Amount"",""CreatedDate"") VALUES (:Id,:Name,:IsActive,:Amount,:CreatedDate)";

        public override string LastInsertedIdStatement => "SELECT LAST_INSERT_ID()";

        public override string SelectStatement => @"SELECT ""Id"",""Name"",""IsActive"",""Amount"",""CreatedDate"" FROM ""test"".""singlekeyparenttable""";

        public override string SelectOneStatement => @"SELECT ""Id"",""Name"",""IsActive"",""Amount"",""CreatedDate"" FROM ""test"".""singlekeyparenttable"" WHERE ""Id""=:Id";

        public override string DeleteStatement => @"DELETE FROM ""test"".""singlekeyparenttable"" WHERE ""Id"" IN (1,2)";

        public override string UpdateStatement => @"UPDATE ""test"".""singlekeyparenttable"" SET ""Name""=:Name,""IsActive""=:IsActive,""Amount""=:Amount,""CreatedDate""=:CreatedDate WHERE ""Id""=:Id";

        public int Ids { get; set; }

        protected override IDatabaseConnector CreateConnector()
        {
            return new CqlDatabaseConnector(this.ConnectionString);
        }

        public override void CreateDatabase()
        {
            // We assume the database is created, we only create and drop the content of it
        }

        public override void DropDatabase()
        {
            this.Connector.ExecuteNonQuery(@"DROP TABLE IF EXISTS ""test"".""singlekeychildtable"";");
            this.Connector.ExecuteNonQuery(@"DROP TABLE IF EXISTS ""test"".""singlekeyparenttable"";");
        }

        public override void CreateParentTable()
        {
            this.Connector.ExecuteNonQuery(@"
                CREATE TABLE IF NOT EXISTS ""test"".""singlekeyparenttable""
                (
                    ""Id""            int,
                    ""Name""          text,
                    ""IsActive""      boolean,
                    ""Amount""        decimal,
                    ""CreatedDate""   timestamp,

                   PRIMARY KEY (""Id"")
                )
            ");
        }

        public override void CreateChildTable()
        {
            this.Connector.ExecuteNonQuery(@"
                CREATE TABLE IF NOT EXISTS ""test"".""singlekeychildtable""
                (
                    ""Id""            int,
                    ""ParentId""      int,
                    ""Name""          text,
                    ""IsActive""      boolean,
                    ""Amount""        decimal,
                    ""CreatedDate""   timestamp,

                   PRIMARY KEY (""Id"")
                )
            ");
        }

        public override object CreateNewEntity()
        {
            return new SingleKeyParentTable
            {
                Id= ++this.Ids,
                Name = "Test Parent " + Guid.NewGuid(),
                IsActive = true,
                Amount = (decimal)30.34,
                CreatedDate = DateTime.Now,
                Childs = new List<SingleKeyChildTable>
                {
                    new SingleKeyChildTable
                    {
                        Name = "Test Child " + Guid.NewGuid(),
                        ParentId = 1,
                        IsActive = true,
                        Amount = new decimal(30.34),
                        CreatedDate = DateTime.Now,
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
            ((SingleKeyParentTable) first).Id = 1;
            ((SingleKeyParentTable) second).Id = 2;
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
