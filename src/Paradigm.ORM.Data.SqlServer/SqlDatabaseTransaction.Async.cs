using System.Threading.Tasks;

namespace Paradigm.ORM.Data.SqlServer
{
    internal partial class SqlDatabaseTransaction
    {
        #region Public Methods

        /// <summary>
        /// Commits the transaction.
        /// </summary>
        /// <returns></returns>
        public Task CommitAsync()
        {
            this.Transaction.Commit();
            return Task.FromResult(default(object));
        }

        /// <summary>
        /// Rolls back the transaction.
        /// </summary>
        /// <returns></returns>
        public Task RollbackAsync()
        {
            this.Transaction.Rollback();
            return Task.FromResult(default(object));
        }

        #endregion
    }
}