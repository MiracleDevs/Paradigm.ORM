using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Tests.Mocks.Batches;

namespace Paradigm.ORM.Tests.Fixtures
{
    public abstract class CommandBatchFixtureBase
    {
        public IDatabaseConnector Connector { get; }

        public abstract string CommandBatchText { get; }

        protected CommandBatchFixtureBase()
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

        public abstract BatchMock CreateMock();
    }
}
