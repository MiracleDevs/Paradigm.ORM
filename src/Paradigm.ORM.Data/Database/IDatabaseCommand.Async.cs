using System.Threading.Tasks;

namespace Paradigm.ORM.Data.Database
{
    public partial interface IDatabaseCommand
    {
        /// <summary>
        /// Sends the CommandText to the <see cref="IDatabaseConnector" />
        /// and builds a <see cref="IDatabaseReader"/>.
        /// </summary>
        /// <returns>A database reader object.</returns>
        Task<IDatabaseReader> ExecuteReaderAsync();

        /// <summary>
        /// Executes a SQL statement against the connection and returns the number
        /// of rows affected.
        /// </summary>
        /// <returns>
        /// The number of rows affected.
        /// </returns>
        Task<int> ExecuteNonQueryAsync();

        /// <summary>
        /// Executes the query, and returns the first column of the first row in the result
        /// set returned by the query. Additional columns or rows are ignored.
        /// </summary>
        /// <returns>
        /// The first column of the first row in the result set, or a null reference 
        /// if the result set is empty.
        /// </returns>
        Task<object> ExecuteScalarAsync();
    }
}