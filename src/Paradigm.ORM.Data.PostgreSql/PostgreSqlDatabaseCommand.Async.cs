using System;
using Npgsql;
using System.Threading.Tasks;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Exceptions;

namespace Paradigm.ORM.Data.PostgreSql
{
    internal partial class PostgreSqlDatabaseCommand
    {
        #region Public Methods

        /// <summary>
        /// Sends the CommandText to the <see cref="PostgreSqlDatabaseConnector" />
        /// and builds a <see cref="PostgreSqlDatabaseReader" />.
        /// </summary>
        /// <returns>
        /// A database reader object.
        /// </returns>
        public async Task<IDatabaseReader> ExecuteReaderAsync()
        {
            try
            {
                this.Command.Transaction = this.Connector.ActiveTransaction?.Transaction;
                this.Connector.LogProvider?.Info($"Execute Reader: {this.Command.CommandText}");
                return new PostgreSqlDatabaseReader(await this.Command.ExecuteReaderAsync() as NpgsqlDataReader);
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
                this.Command.Transaction = this.Connector.ActiveTransaction?.Transaction;
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
                this.Command.Transaction = this.Connector.ActiveTransaction?.Transaction;
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