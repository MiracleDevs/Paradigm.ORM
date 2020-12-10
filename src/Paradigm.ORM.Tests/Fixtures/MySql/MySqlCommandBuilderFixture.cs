using System;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.MySql;
using Paradigm.ORM.Tests.Mocks;
using Paradigm.ORM.Tests.Mocks.MySql;

namespace Paradigm.ORM.Tests.Fixtures.MySql
{
    public class MySqlCommandBuilderFixture : CommandBuilderFixtureBase
    {
        private string ConnectionString => "Server=localhost;Database=test;User=root;Password=Paradigm_Test_1234;Connection Timeout=3600;Allow User Variables=True;POOLING=true";

        protected override IDatabaseConnector CreateConnector()
        {
            return new MySqlDatabaseConnector(this.ConnectionString);
        }

        public override string SelectQuery => "SELECT `Id`,`Name`,`IsActive`,`Amount`,`CreatedDate` FROM `test`.`simple_table`";

        public override string SelectWhereClause => "`Name` = \"John Doe\"";

        public override string SelectOneQuery => "SELECT `Id`,`Name`,`IsActive`,`Amount`,`CreatedDate` FROM `test`.`simple_table` WHERE `Id`=@Id";

        public override string SelectWithWhereQuery => "SELECT `Id`,`Name`,`IsActive`,`Amount`,`CreatedDate` FROM `test`.`simple_table` WHERE " + SelectWhereClause;

        public override string SelectWithTwoPrimaryKeysQuery => "SELECT `Id1`,`Id2`,`Name` FROM `test`.`two_primary_key_table` WHERE `Id1`=@Id1 AND `Id2`=@Id2";

        public override string InsertQuery => "INSERT INTO `test`.`simple_table` (`Name`,`IsActive`,`Amount`,`CreatedDate`) VALUES (@Name,@IsActive,@Amount,@CreatedDate)";

        public override string DeleteOneEntityQuerySingleKey => "DELETE FROM `test`.`simple_table` WHERE `Id`=@Id";

        public override string DeleteOneEntityQueryMultipleKey => "DELETE FROM `test`.`two_primary_key_table` WHERE `Id1`=@Id1 AND `Id2`=@Id2";

        public override string UpdateQuery => "UPDATE `test`.`simple_table` SET `Id`=@Id,`Name`=@Name,`IsActive`=@IsActive,`Amount`=@Amount,`CreatedDate`=@CreatedDate WHERE `Id`=@Id";

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
