using System;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Descriptors;

namespace Paradigm.ORM.Tests.Fixtures
{
    public abstract class StoredProcedureFixtureBase : IDisposable
    {
        protected abstract string ConnectionString { get; }

        public IDatabaseConnector Connector { get; }

        protected StoredProcedureFixtureBase()
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

        public abstract object CreateNewActiveEntity();

        public abstract object CreateActiveChildEntity();

        public abstract object CreateNewInactiveEntity();

        public abstract void CreateStoredProcedures();

        public abstract void DeleteStoredProcedures();

        public abstract ITableTypeDescriptor GetDescriptor();
    }
}
