using System.Threading.Tasks;

namespace Paradigm.ORM.Data.Database
{
    public partial interface IDatabaseReader
    {
        /// <summary>
        /// Advances the DatabaseReader to the next record.
        /// </summary>
        /// <returns>
        /// true if there are more rows; otherwise false.
        /// </returns>
        Task<bool> ReadAsync();

        /// <summary>
        /// Advances the data reader to the next result, when reading the results of batch
        /// SQL statements.
        /// </summary>
        /// <returns>
        /// true if there are more result sets; otherwise false.
        /// </returns>
        Task<bool> NextResultAsync();

        /// <summary>
        /// Gets the value of the specified column as a type.
        /// </summary>
        /// <typeparam name="T">The type of the value to be returned. See the remarks section for more information.</typeparam>
        /// <param name="index">The column to be retrieved.</param>
        /// <returns>The returned type object.</returns>
        Task<T> GetFieldValueAsync<T>(int index);

        /// <summary>
        /// Gets a value that indicates whether the column contains nonexistent or missing
        /// values.
        /// </summary>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns>true if the specified column is equivalent to System.DBNull; otherwise false.</returns>
        Task<bool> IsDBNullAsync(int index);
    }
}