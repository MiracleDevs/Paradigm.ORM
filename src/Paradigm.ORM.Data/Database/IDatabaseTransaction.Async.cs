using System.Threading.Tasks;

namespace Paradigm.ORM.Data.Database
{
    public partial interface IDatabaseTransaction
    {
        /// <summary>
        /// Commits the transaction.
        /// </summary>
        Task CommitAsync();

        /// <summary>
        /// Rolls back the transaction.
        /// </summary>
        Task RollbackAsync();
    }
}