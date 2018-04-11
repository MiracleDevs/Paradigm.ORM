using Paradigm.ORM.Data.Cassandra;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Tests.Mocks.Batches;

namespace Paradigm.ORM.Tests.Fixtures.Cql
{
    public class CqlCommandBatchFixture: CommandBatchFixtureBase
    {
        private string ConnectionString => "Contact Points=localhost;Port=9042";

        public override string CommandBatchText => @"INSERT INTO ""batch"" (""id"",""name"",""mobile"",""mobile_brand"",""mobile_number"") VALUES (:p0,:p1,:p2,:p3,:p4);";

        protected override IDatabaseConnector CreateConnector()
        {
            return new CqlDatabaseConnector(ConnectionString);
        }

        public override BatchMock CreateMock()
        {
            return new BatchMock
            {
                Id = 1,
                Name = "John Smith",
                Mobile = "Android",
                MobileBrand = "HTC",
                MobileNumber = "+14562345"
            };
        }
    }
}
