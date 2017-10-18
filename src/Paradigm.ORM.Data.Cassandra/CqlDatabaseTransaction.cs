using System;
using Cassandra.Data;
using Paradigm.ORM.Data.Database;

namespace Paradigm.ORM.Data.Cassandra
{
    /// <summary>
    /// Represents a SQL transaction to be made in a Cql Server database.
    /// </summary>
    /// <seealso cref="Paradigm.ORM.Data.Database.IDatabaseTransaction" />
    internal partial class CqlDatabaseTransaction: IDatabaseTransaction
    {
        #region Properties

        /// <summary>
        /// Gets or sets the inner transaction.
        /// </summary>
        internal CqlBatchTransaction Transaction { get; set; }

        /// <summary>
        /// Gets or sets the inner connector.
        /// </summary>
        private CqlDatabaseConnector Connector { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CqlDatabaseTransaction"/> class.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="connector">The database connector.</param>
        /// <exception cref="System.ArgumentNullException">transaction can not be null.</exception>
        internal CqlDatabaseTransaction(CqlBatchTransaction transaction, CqlDatabaseConnector connector)
        {
            this.Transaction = transaction ?? throw new ArgumentNullException(nameof(transaction), $"{nameof(transaction)} can not be null.");
            this.Connector = connector;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Transaction?.Dispose();
            this.Transaction = null;

            this.Connector?.PopTransaction();
            this.Connector = null;
        }

        /// <summary>
        /// Commits the transaction.
        /// </summary>
        public void Commit()
        {
            this.Transaction.Commit();
        }

        /// <summary>
        /// Rolls back the transaction.
        /// </summary>
        public void Rollback()
        {
            this.Transaction.Rollback();
        }

        #endregion
    }
}