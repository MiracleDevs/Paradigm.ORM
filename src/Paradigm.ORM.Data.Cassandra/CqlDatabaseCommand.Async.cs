using System;
using System.Threading.Tasks;
using Cassandra.Data;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Exceptions;

namespace Paradigm.ORM.Data.Cassandra
{
    internal partial class CqlDatabaseCommand
    {
        #region Public Methods

        /// <summary>
        /// Sends the CommandText to the <see cref="CqlDatabaseConnector" />
        /// and builds a <see cref="CqlDatabaseReader" />.
        /// </summary>
        /// <returns>
        /// A database reader object.
        /// </returns>
        public async Task<IDatabaseReader> ExecuteReaderAsync()
        {
            try
            {
                if (this.Connector.ActiveTransaction != null)
                    this.Command.Transaction = this.Connector.ActiveTransaction.Transaction;

                this.Connector.LogProvider?.Info($"Execute Reader: {this.Command.CommandText}");
                return new CqlDatabaseReader(await this.Command.ExecuteReaderAsync() as CqlReader);
            }
            catch (Exception e)
            {
                this.Connector.LogProvider?.Error(e.Message);
                throw new DatabaseCommandException(this, e);
            }
        }

        /// <summary>
        /// Executes a SQL statement against the connection and returns the number
        /// of rows affected.
        /// </summary>
        /// <returns>
        /// The number of rows affected.
        /// </returns>
        public async Task<int> ExecuteNonQueryAsync()
        {
            try
            {
                if (this.Connector.ActiveTransaction != null)
                    this.Command.Transaction = this.Connector.ActiveTransaction.Transaction;

                this.Connector.LogProvider?.Info($"Execute Non Query: {this.Command.CommandText}");
                return await this.Command.ExecuteNonQueryAsync();

            }
            catch (Exception e)
            {
                this.Connector.LogProvider?.Error(e.Message);
                throw new DatabaseCommandException(this, e);
            }
        }

        /// <summary>
        /// Executes the query, and returns the first column of the first row in the result
        /// set returned by the query. Additional columns or rows are ignored.
        /// </summary>
        /// <returns>
        /// The first column of the first row in the result set, or a null reference
        /// if the result set is empty.
        /// </returns>
        public async Task<object> ExecuteScalarAsync()
        {
            try
            {
                if (this.Connector.ActiveTransaction != null)
                    this.Command.Transaction = this.Connector.ActiveTransaction.Transaction;

                this.Connector.LogProvider?.Info($"Execute Scalar: {this.Command.CommandText}");
                return await this.Command.ExecuteScalarAsync();
            }
            catch (Exception e)
            {
                this.Connector.LogProvider?.Error(e.Message);
                throw new DatabaseCommandException(this, e);
            }
        }

        #endregion
    }
}