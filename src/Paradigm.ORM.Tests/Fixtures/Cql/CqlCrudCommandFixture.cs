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
        private string ConnectionString => ConnectionStrings.Cql;

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
            this.Connector.ExecuteNonQuery(@"DROP TABLE IF EXISTS ""test"".""TwoPrimaryKeyTable"";");
        }

        public override void CreateTables()
        {
            this.Connector.ExecuteNonQuery(@"
                CREATE TABLE IF NOT EXISTS ""test"".""singlekeyparenttable""
                (
                    ""Id""            int,
                    ""Name""          text,
                    ""IsActive""      boolean,
                    ""Amount""        decimal,
                    ""CreatedDate""   date,

                   PRIMARY KEY (""Id"")
                )
            ");

            this.Connector.ExecuteNonQuery(@"
                CREATE TABLE IF NOT EXISTS ""test"".""singlekeychildtable""
                (
                    ""Id""            int,
                    ""ParentId""      int,
                    ""Name""          text,
                    ""IsActive""      boolean,
                    ""Amount""        decimal,
                    ""CreatedDate""   date,

                   PRIMARY KEY (""Id"")
                )
            ");

            this.Connector.ExecuteNonQuery(@"
                CREATE TABLE IF NOT EXISTS ""test"".""TwoPrimaryKeyTable""
                (
                    ""Id1""           int,
                    ""Id2""           int,
                    ""Name""          text,
                    ""Date""          timestamp,

                   PRIMARY KEY ((""Id1"", ""Id2""), ""Date"")
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
                    new()
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
