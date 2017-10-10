using System.Threading.Tasks;

namespace Paradigm.ORM.Data.MySql
{
    internal partial class MySqlDatabaseTransaction
    {
        #region Public Methods

        /// <summary>
        /// Commits the transaction.
        /// </summary>
        public async Task CommitAsync()
        {
            await this.Transaction.CommitAsync();
        }

        /// <summary>
        /// Rolls back the transaction.
        /// </summary>
        public async Task RollbackAsync()
        {
            await this.Transaction.RollbackAsync();
        }

        #endregion
    }
}