using System.Threading.Tasks;
using Paradigm.ORM.Data.Database;
using MySql.Data.MySqlClient;

namespace Paradigm.ORM.Data.MySql
{
    internal partial class MySqlDatabaseCommand
    {
        #region Public Methods

        /// <summary>
        /// Sends the CommandText to the <see cref="MySqlDatabaseConnector" />
        /// and builds a <see cref="MySqlDatabaseReader" />.
        /// </summary>
        /// <returns>
        /// A database reader object.
        /// </returns>
        public async Task<IDatabaseReader> ExecuteReaderAsync()
        {
            this.Command.Transaction = this.Connector.ActiveTransaction?.Transaction;
            return new MySqlDatabaseReader(await this.Command.ExecuteReaderAsync() as MySqlDataReader);
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