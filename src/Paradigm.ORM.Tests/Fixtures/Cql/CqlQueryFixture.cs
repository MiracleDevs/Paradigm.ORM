using System;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Descriptors;
using Paradigm.ORM.Data.Extensions;
using Paradigm.ORM.Tests.Mocks.Cql;
using System.Collections.Generic;
using Paradigm.ORM.Data.Cassandra;

namespace Paradigm.ORM.Tests.Fixtures.Cql
{
    public class CqlQueryFixture : QueryFixtureBase
    {
        private string ConnectionString => "Contact Points=192.168.2.223;Port=9042;Default Keyspace=equipcast;Username=root;Password=Equ1pc45t_M1r4cl3D3v5!";

        public override string SelectClause => @"SELECT * FROM ""test"".""singlekeyparenttable""";

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
                    ""CreatedDate""   date,

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
                    ""CreatedDate""   date,

                   PRIMARY KEY (""Id"")
                )
            ");
        }

        public override object CreateNewEntity()
        {
            return new SingleKeyParentTable
            {
                Id = 1,
                Name = $"Test Parent 1",
                IsActive = true,
                Amount = (decimal)30.34,
                CreatedDate = new DateTime(2017, 4, 12),
                Childs = new List<SingleKeyChildTable>
                {
                    new SingleKeyChildTable
                    {
                        Id = 1,
                        ParentId = 1,
                        Name = "Test Child " + Guid.NewGuid(),
                        IsActive = true,
                        Amount = new decimal(30.34),
                        CreatedDate = DateTime.Today,
                    }
                }
            };
        }

        public override object CreateNewEntity2()
        {
            return new SingleKeyParentTable
            {
                Id = 2,
                Name = $"Test Parent 2 {Guid.NewGuid()}",
                IsActive = false,
                Amount = 215.50m,
                CreatedDate = new DateTime(2017, 6, 21),
                Childs = new List<SingleKeyChildTable>
                {
                    new SingleKeyChildTable
                    {
                        Id=2,
                        ParentId=2,
                        Name = $"Test Child 1 {Guid.NewGuid()}",
                        IsActive = false,
                        Amount = 100.25m,
                        CreatedDate = new DateTime(2017, 6, 22),
                    },
                    new SingleKeyChildTable
                    {
                        Id=3,
                        ParentId=2,
                        Name = $"Test Child 2 {Guid.NewGuid()}",
                        IsActive = true,
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
    }
}
