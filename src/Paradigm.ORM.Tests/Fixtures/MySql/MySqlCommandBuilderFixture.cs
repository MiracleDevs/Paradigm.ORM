using System;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.MySql;
using Paradigm.ORM.Tests.Mocks;
using Paradigm.ORM.Tests.Mocks.MySql;

namespace Paradigm.ORM.Tests.Fixtures.MySql
{
    public class MySqlCommandBuilderFixture : CommandBuilderFixtureBase
    {
        private string ConnectionString => "Server=localhost;Database=test;User=test;Password=test1234;Connection Timeout=3600;Allow User Variables=True;POOLING=true";

        protected override IDatabaseConnector CreateConnector()
        {
            return new MySqlDatabaseConnector(this.ConnectionString);
        }

        public override string SelectQuery => "SELECT `Id`,`Name`,`IsActive`,`Amount`,`CreatedDate` FROM `test`.`simpletable`";

        public override string SelectWhereClause => "`Name` = \"John Doe\"";

        public override string SelectOneQuery => "SELECT `Id`,`Name`,`IsActive`,`Amount`,`CreatedDate` FROM `test`.`simpletable` WHERE `Id`=@Id";

        public override string SelectWithWhereQuery => "SELECT `Id`,`Name`,`IsActive`,`Amount`,`CreatedDate` FROM `test`.`simpletable` WHERE " + SelectWhereClause;

        public override string SelectWithTwoPrimaryKeysQuery => "SELECT `Id1`,`Id2`,`Name` FROM `test`.`twoprimarykeytable` WHERE `Id1`=@Id1 AND `Id2`=@Id2";

        public override string InsertQuery => "INSERT INTO `test`.`simpletable` (`Name`,`IsActive`,`Amount`,`CreatedDate`) VALUES (@Name,@IsActive,@Amount,@CreatedDate)";

        public override string DeleteOneEntityQuerySingleKey => $"DELETE FROM `test`.`simpletable` WHERE `Id` IN ({Entity1.Id})";

        public override string DeleteTwoEntitiesQuerySingleKey => $"DELETE FROM `test`.`simpletable` WHERE `Id` IN ({Entity1.Id},{Entity2.Id})";

        public override string DeleteOneEntityQueryMultipleKey => $"DELETE FROM `test`.`twoprimarykeytable` WHERE (`Id1`={TwoPrimaryKeyEntity1.Id1} AND `Id2`={TwoPrimaryKeyEntity1.Id2})";

        public override string DeleteTwoEntitiesQueryMultipleKey => $"DELETE FROM `test`.`twoprimarykeytable` WHERE (`Id1`={TwoPrimaryKeyEntity1.Id1} AND `Id2`={TwoPrimaryKeyEntity1.Id2}) OR (`Id1`={TwoPrimaryKeyEntity2.Id1} AND `Id2`={TwoPrimaryKeyEntity2.Id2})";

        public override string UpdateQuery => "UPDATE `test`.`simpletable` SET `Id`=@Id,`Name`=@Name,`IsActive`=@IsActive,`Amount`=@Amount,`CreatedDate`=@CreatedDate WHERE `Id`=@Id";

        public override string LastInsertIdQuery => "SELECT LAST_INSERT_ID()";

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
