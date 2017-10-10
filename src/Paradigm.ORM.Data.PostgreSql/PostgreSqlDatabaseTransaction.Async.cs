using System.Threading.Tasks;

namespace Paradigm.ORM.Data.PostgreSql
{
    internal partial class PostgreSqlDatabaseTransaction
    {
        #region Public Methods

        /// <summary>
        /// Commits the transaction.
        /// </summary>
        /// <returns></returns>
        public async Task CommitAsync()
        {
            await this.Transaction.CommitAsync();
        }

        /// <summary>
        /// Rolls back the transaction.
        /// </summary>
        /// <returns></returns>
        public async Task RollbackAsync()
        {
            await this.Transaction.RollbackAsync();
        }

        #endregion
    }
}