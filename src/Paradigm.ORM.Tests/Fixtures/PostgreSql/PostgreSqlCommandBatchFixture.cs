﻿using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.PostgreSql;
using Paradigm.ORM.Tests.Mocks.Batches;

namespace Paradigm.ORM.Tests.Fixtures.PostgreSql
{
    public class PostgreSqlCommandBatchFixture: CommandBatchFixtureBase
    {
        private string ConnectionString => ConnectionStrings.PSql;

        public override string CommandBatchText => @"INSERT INTO ""batch"" (""id"",""name"",""mobile"",""mobile_brand"",""mobile_number"") VALUES (@p0,@p1,@p2,@p3,@p4);INSERT INTO ""batch"" (""id"",""name"",""mobile"",""mobile_brand"",""mobile_number"") VALUES (@p5,@p6,@p7,@p8,@p9);";

        protected override IDatabaseConnector CreateConnector()
        {
            return new PostgreSqlDatabaseConnector(ConnectionString);
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
