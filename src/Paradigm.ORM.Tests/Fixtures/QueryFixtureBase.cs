using System;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Descriptors;

namespace Paradigm.ORM.Tests.Fixtures
{
    public abstract class QueryFixtureBase : IDisposable
    {
        public abstract string SelectClause { get; }

        public IDatabaseConnector Connector { get; }

        protected QueryFixtureBase()
        {
            this.Connector = this.CreateConnector();
            this.Connector.Open();
        }

        public void Dispose()
        {
            this.Connector.Close();
            this.Connector?.Dispose();
        }

        protected abstract IDatabaseConnector CreateConnector();

        public abstract void CreateDatabase();

        public abstract void DropDatabase();

        public abstract void CreateParentTable();

        public abstract void CreateChildTable();

        public abstract object CreateNewEntity();

        public abstract object CreateNewEntity2();

        public abstract ITableTypeDescriptor GetDescriptor();
    }
}
