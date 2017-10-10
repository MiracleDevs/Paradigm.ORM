using System.Threading.Tasks;

namespace Paradigm.ORM.Data.SqlServer
{
    internal partial class SqlDatabaseReader
    {
        #region Public Methods

        /// <summary>
        /// Advances the DatabaseReader to the next record.
        /// </summary>
        /// <returns>
        /// true if there are more rows; otherwise false.
        /// </returns>
        public async Task<bool> ReadAsync()
        {
            return await this.Reader.ReadAsync();
        }

        /// <summary>
        /// Advances the data reader to the next result, when reading the results of batch
        /// SQL statements.
        /// </summary>
        /// <returns>
        /// true if there are more result sets; otherwise false.
        /// </returns>
        public async Task<bool> NextResultAsync()
        {
            return await this.Reader.NextResultAsync();
        }

        /// <summary>
        /// Gets the value of the specified column as a type.
        /// </summary>
        /// <typeparam name="T">The type of the value to be returned. See the remarks section for more information.</typeparam>
        /// <param name="index">The column to be retrieved.</param>
        /// <returns>
        /// The returned type object.
        /// </returns>
        public async Task<T> GetFieldValueAsync<T>(int index)
        {
            return await this.Reader.GetFieldValueAsync<T>(index);
        }

        /// <summary>
        /// Gets a value that indicates whether the column contains nonexistent or missing
        /// values.
        /// </summary>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns>
        /// true if the specified column is equivalent to System.DBNull; otherwise false.
        /// </returns>
        public async Task<bool> IsDBNullAsync(int index)
        {
            return await this.Reader.IsDBNullAsync(index);
        }

        #endregion
    }
}