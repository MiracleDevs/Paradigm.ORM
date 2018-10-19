using System;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Descriptors;

namespace Paradigm.ORM.Tests.Fixtures
{
    public abstract class CrudCommandFixtureBase : IDisposable
    {
        public IDatabaseConnector Connector { get; }

        protected CrudCommandFixtureBase()
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

        public abstract void CreateTables();

        public abstract object CreateNewEntity();

        public abstract object CreateNewTwoKeysEntity();

        public abstract ITableTypeDescriptor GetParentDescriptor();

        public abstract ITableTypeDescriptor GetMultipleKeyDescriptor();

        public abstract void SetEntityId(object first, object second);

        public abstract void Update(object first, object second);

        public abstract void CheckUpdate(object first, object second);  
    }
}
