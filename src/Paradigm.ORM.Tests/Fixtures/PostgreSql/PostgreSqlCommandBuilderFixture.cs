using System;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.PostgreSql;
using Paradigm.ORM.Tests.Mocks;
using Paradigm.ORM.Tests.Mocks.PostgreSql;

namespace Paradigm.ORM.Tests.Fixtures.PostgreSql
{
    public class PostgreSqlCommandBuilderFixture : CommandBuilderFixtureBase
    {
        private string ConnectionString => "Server=localhost;User Id=test;Password=test1234;Timeout=3";

        protected override IDatabaseConnector CreateConnector()
        {
            return new PostgreSqlDatabaseConnector(this.ConnectionString);
        }

        public override string SelectQuery => "SELECT \"Id\",\"Name\",\"IsActive\",\"Amount\",\"CreatedDate\" FROM \"SimpleTable\"";

        public override string SelectWhereClause => "\"Name\" = \"John Doe\"";

        public override string SelectOneQuery => "SELECT \"Id\",\"Name\",\"IsActive\",\"Amount\",\"CreatedDate\" FROM \"SimpleTable\" WHERE \"Id\"=@Id";

        public override string SelectWithWhereQuery => $"{SelectQuery} WHERE {SelectWhereClause}";

        public override string SelectWithTwoPrimaryKeysQuery => "SELECT \"Id\",\"Id2\",\"Name\" FROM \"TwoPrimaryKeyTable\" WHERE \"Id\"=@Id AND \"Id2\"=@Id2";

        public override string InsertQuery => "INSERT INTO \"SimpleTable\" (\"Name\",\"IsActive\",\"Amount\",\"CreatedDate\") VALUES (@Name,@IsActive,@Amount,@CreatedDate)";

        public override string DeleteOneEntityQuery => $"DELETE FROM \"SimpleTable\" WHERE \"Id\" IN ({Entity1.Id})";

        public override string DeleteTwoEntitiesQuery => $"DELETE FROM \"SimpleTable\" WHERE \"Id\" IN ({Entity1.Id},{Entity2.Id})";

        public override string UpdateQuery => "UPDATE \"SimpleTable\" SET \"Id\"=@Id,\"Name\"=@Name,\"IsActive\"=@IsActive,\"Amount\"=@Amount,\"CreatedDate\"=@CreatedDate WHERE \"Id\"=@Id";

        public override string LastInsertIdQuery => "SELECT LASTVAL()";

        public override ISimpleTable Entity1 => new SimpleTable
        {
            Id = 723,
            Name = "John Doe",
            IsActive = true,
            Amount = 3600,
            CreatedDate = new DateTime(2017, 5, 23, 13, 55, 43, 0)
        };

        public override ISimpleTable Entity2 => new SimpleTable
        {
            Id = 15,
            Name = "Jane Doe",
            IsActive = false,
            Amount = 1800,
            CreatedDate = new DateTime(2015, 9, 12, 19, 11, 23, 0)
        };

    }
}
