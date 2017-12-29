using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Tests.Mocks;

namespace Paradigm.ORM.Tests.Fixtures
{
    public abstract class CommandBuilderFixtureBase
    {
        public IDatabaseConnector Connector { get; }

        protected abstract IDatabaseConnector CreateConnector();

        protected CommandBuilderFixtureBase()
        {
            this.Connector = this.CreateConnector();
            this.Connector.Open();
        }

        public void Dispose()
        {
            this.Connector.Close();
            this.Connector?.Dispose();
        }

        public abstract string SelectQuery { get; }

        public abstract string SelectWhereClause { get; }

        public abstract string SelectOneQuery { get; }

        public abstract string SelectWithWhereQuery { get; }

        public abstract string SelectWithTwoPrimaryKeysQuery { get; }

        public abstract string InsertQuery { get; }

        public abstract string DeleteOneEntityQuery { get; }

        public abstract string DeleteTwoEntitiesQuery { get; }

        public abstract string UpdateQuery { get; }

        public abstract string LastInsertIdQuery { get; }

        public abstract ISimpleTable Entity1 { get; }

        public abstract ISimpleTable Entity2 { get; }
    }
}
