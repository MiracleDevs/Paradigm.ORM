using System;

namespace Paradigm.ORM.Data.Database
{
    /// <summary>
    /// Provides an interface to work with database transactions.
    /// </summary>
    public partial interface IDatabaseTransaction: IDisposable
    {
        /// <summary>
        /// Commits the transaction.
        /// </summary>
        void Commit();

        /// <summary>
        /// Rolls back the transaction.
        /// </summary>
        void Rollback();
    }
}