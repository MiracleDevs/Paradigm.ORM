using System.Threading.Tasks;
using Paradigm.ORM.Data.Database;

namespace Paradigm.ORM.Data.SqlServer
{
    internal partial class SqlDatabaseCommand
    {
        #region Public Methods

        /// <summary>
        /// Sends the CommandText to the <see cref="SqlDatabaseConnector" />
        /// and builds a <see cref="SqlDatabaseReader" />.
        /// </summary>
        /// <returns>
        /// A database reader object.
        /// </returns>
        public async Task<IDatabaseReader> ExecuteReaderAsync()
        {
            this.Command.Transaction = this.Connector.ActiveTransaction?.Transaction;
            return new SqlDatabaseReader(await this.Command.ExecuteReaderAsync());
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
            this.Command.Transaction = this.Connector.ActiveTransaction?.Transaction;
            return await this.Command.ExecuteNonQueryAsync();
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
            this.Command.Transaction = this.Connector.ActiveTransaction?.Transaction;
            return await this.Command.ExecuteScalarAsync();
        }

        #endregion
    }
}