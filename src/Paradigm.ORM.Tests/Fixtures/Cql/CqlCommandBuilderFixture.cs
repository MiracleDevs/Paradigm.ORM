using System;
using Paradigm.ORM.Data.Cassandra;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Tests.Mocks;
using Paradigm.ORM.Tests.Mocks.Cql;

namespace Paradigm.ORM.Tests.Fixtures.Cql
{
    public class CqlCommandBuilderFixture : CommandBuilderFixtureBase
    {
        private string ConnectionString => ConnectionStrings.Cql;

        protected override IDatabaseConnector CreateConnector()
        {
            return new CqlDatabaseConnector(this.ConnectionString);
        }

        public override string SelectQuery => @"SELECT ""Id"",""Name"",""IsActive"",""Amount"",""CreatedDate"" FROM ""SimpleTable""";

        public override string SelectWhereClause => @"""Name"" = ""John Doe""";

        public override string SelectOneQuery => @"SELECT ""Id"",""Name"",""IsActive"",""Amount"",""CreatedDate"" FROM ""SimpleTable"" WHERE ""Id""=:Id";

        public override string SelectWithWhereQuery => $"{SelectQuery} WHERE {SelectWhereClause}";

        public override string SelectWithTwoPrimaryKeysQuery => @"SELECT ""Id1"",""Id2"",""Name"",""Date"" FROM ""test"".""TwoPrimaryKeyTable"" WHERE ""Id1""=:Id1 AND ""Id2""=:Id2 AND ""Date""=:Date";

        public override string InsertQuery => @"INSERT INTO ""SimpleTable"" (""Id"",""Name"",""IsActive"",""Amount"",""CreatedDate"") VALUES (:Id,:Name,:IsActive,:Amount,:CreatedDate)";

        public override string DeleteOneEntityQuerySingleKey => @"DELETE FROM ""SimpleTable"" WHERE ""Id""=:Id";

        public override string DeleteOneEntityQueryMultipleKey => @"DELETE FROM ""test"".""TwoPrimaryKeyTable"" WHERE ""Id1""=:Id1 AND ""Id2""=:Id2 AND ""Date""=:Date";

        public override string UpdateQuery => @"UPDATE ""SimpleTable"" SET ""Name""=:Name,""IsActive""=:IsActive,""Amount""=:Amount,""CreatedDate""=:CreatedDate WHERE ""Id""=:Id";

        public override string LastInsertIdQuery => null;

        public override ISimpleTable Entity1 => new SimpleTable
        {
            Id = 723,
            Name = "John Doe",
            IsActive = true,
            Amount = 3600,
            CreatedDate = new DateTime(2017, 5, 23)
        };

        public override ISimpleTable Entity2 => new SimpleTable
        {
            Id = 15,
            Name = "Jane Doe",
            IsActive = false,
            Amount = 1800,
            CreatedDate = new DateTime(2015, 9, 12)
        };

        public override ITwoPrimaryKeyTable TwoPrimaryKeyEntity1 => new TwoPrimaryKeyTable
        {
            Id1 = 1,
            Id2 = 2,
            Name = "First Entity"
        };

        public override ITwoPrimaryKeyTable TwoPrimaryKeyEntity2 => new TwoPrimaryKeyTable
        {
            Id1 = 3,
            Id2 = 4,
            Name = "Second Entity"
        };
    }
}
