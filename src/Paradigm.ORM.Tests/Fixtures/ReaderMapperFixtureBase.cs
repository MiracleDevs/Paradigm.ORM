using System;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Descriptors;

namespace Paradigm.ORM.Tests.Fixtures
{
    public abstract class ReaderMapperFixtureBase : IDisposable
    {
        protected abstract string ConnectionString { get; }

        public abstract string SelectStatement { get; }

        public IDatabaseConnector Connector { get; }

        protected ReaderMapperFixtureBase()
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

        public abstract void CreateTable();

        public abstract object CreateNewEntity();

        public abstract ITableTypeDescriptor GetDescriptor();
    }
}
