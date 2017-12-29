using System;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Descriptors;

namespace Paradigm.ORM.Tests.Fixtures
{
    public abstract class CrudCommandFixtureBase : IDisposable
    {
        public abstract string InsertParentStatement { get; }

        public abstract string LastInsertedIdStatement { get; }

        public abstract string SelectStatement { get; }

        public abstract string SelectOneStatement { get; }

        public abstract string DeleteStatement { get; }

        public abstract string UpdateStatement { get; }

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

        public abstract void CreateParentTable();

        public abstract void CreateChildTable();

        public abstract object CreateNewEntity();

        public abstract ITableTypeDescriptor GetParentDescriptor();

        public abstract void SetEntityId(object first, object second);

        public abstract void Update(object first, object second);

        public abstract void CheckUpdate(object first, object second);
    }
}
