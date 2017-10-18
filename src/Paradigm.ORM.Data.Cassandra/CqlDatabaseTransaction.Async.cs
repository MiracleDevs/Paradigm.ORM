using System.Threading.Tasks;

namespace Paradigm.ORM.Data.Cassandra
{
    internal partial class CqlDatabaseTransaction
    {
        #region Public Methods

        /// <summary>
        /// Commits the transaction.
        /// </summary>
        public Task CommitAsync()
        {
            this.Transaction.Commit();
            return Task.FromResult(default(object));
        }

        /// <summary>
        /// Rolls back the transaction.
        /// </summary>
        public Task RollbackAsync()
        {
            this.Transaction.Rollback();
            return Task.FromResult(default(object));
        }

        #endregion
    }
}