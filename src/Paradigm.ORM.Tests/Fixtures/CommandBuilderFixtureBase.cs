﻿using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Tests.Mocks;

namespace Paradigm.ORM.Tests.Fixtures
{
    public abstract class CommandBuilderFixtureBase
    {
        public IDatabaseConnector Connector { get; }

        public abstract string SelectQuery { get; }

        public abstract string SelectWhereClause { get; }

        public abstract string SelectOneQuery { get; }

        public abstract string SelectWithWhereQuery { get; }

        public abstract string SelectWithTwoPrimaryKeysQuery { get; }

        public abstract string InsertQuery { get; }

        public abstract string DeleteOneEntityQuerySingleKey { get; }

        public abstract string DeleteOneEntityQueryMultipleKey { get; }

        public abstract string UpdateQuery { get; }

        public abstract string LastInsertIdQuery { get; }

        public abstract ISimpleTable Entity1 { get; }

        public abstract ISimpleTable Entity2 { get; }

        public abstract ITwoPrimaryKeyTable TwoPrimaryKeyEntity1 { get; }

        public abstract ITwoPrimaryKeyTable TwoPrimaryKeyEntity2 { get; }      

        protected CommandBuilderFixtureBase()
        {
            this.Connector = this.CreateConnector();
            this.Connector.Open();
        }

        protected abstract IDatabaseConnector CreateConnector();

        public void Dispose()
        {
            this.Connector.Close();
            this.Connector?.Dispose();
        }
    }
}
