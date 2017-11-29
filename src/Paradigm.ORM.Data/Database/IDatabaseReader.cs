using System;

namespace Paradigm.ORM.Data.Database
{
    /// <summary>
    ///  Provides an ierface to read a forward-only stream of rows from a data source.
    /// </summary>
    public partial interface IDatabaseReader : IDisposable
    {
        /// <summary>
        /// Advances the DatabaseReader to the next record.
        /// </summary>
        /// <returns>
        /// true if there are more rows; otherwise false.
        /// </returns>
        bool Read();

        /// <summary>
        /// Advances the data reader to the next result, when reading the results of batch
        /// SQL statements.
        /// </summary>
        /// <returns>
        /// true if there are more result sets; otherwise false.
        /// </returns>
        bool NextResult();

        /// <summary>
        /// Gets the column ordinal, given the name of the column.
        /// </summary>
        /// <param name="name">The name of the column. </param>
        /// <returns>The zero-based column ordinal.</returns>
        int GetOrdinal(string name);

        /// <summary>
        /// Gets the value of the specified column as a type.
        /// </summary>
        /// <typeparam name="T">The type of the value to be returned. See the remarks section for more information.</typeparam>
        /// <param name="index">The column to be retrieved.</param>
        /// <returns>The returned type object.</returns>
        T GetFieldValue<T>(int index);

        /// <summary>
        /// Gets the name of the specified column.
        /// </summary>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns>The name of the specified column.</returns>
        string GetName(int index);

        /// <summary>
        /// Gets the value of the specified column as an instance of <see cref="object" />.
        /// </summary>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns>The value of the specified column.</returns>
        object GetValue(int index);

        /// <summary>
        /// Gets the value of the specified column as an instance of <see cref="Object" />.
        /// </summary>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the specified column.</returns>
        object GetValue(string name);

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Boolean" />.
        /// </summary>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns>The value of the specified column.</returns>
        bool GetBoolean(int index);

        /// <summary>
        /// Gets the value of the specified column as a <see cref="SByte" />.
        /// </summary>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns>The value of the specified column.</returns>
        sbyte GetSByte(int index);

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Int16" />.
        /// </summary>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns>The value of the specified column.</returns>
        short GetInt16(int index);

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Int32" />.
        /// </summary>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns>The value of the specified column.</returns>
        int GetInt32(int index);

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Int64" />.
        /// </summary>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns>The value of the specified column.</returns>
        long GetInt64(int index);

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Byte" />.
        /// </summary>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns>The value of the specified column.</returns>
        byte GetByte(int index);

        /// <summary>
        /// Gets the value of the specified column as a <see cref="UInt16" />.
        /// </summary>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns>The value of the specified column.</returns>
        ushort GetUInt16(int index);

        /// <summary>
        /// Gets the value of the specified column as a <see cref="UInt32" />.
        /// </summary>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns>The value of the specified column.</returns>
        uint GetUInt32(int index);

        /// <summary>
        /// Gets the value of the specified column as a <see cref="UInt64" />.
        /// </summary>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns>The value of the specified column.</returns>
        ulong GetUInt64(int index);

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Single" />.
        /// </summary>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns>The value of the specified column.</returns>
        float GetFloat(int index);

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Double" />.
        /// </summary>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns>The value of the specified column.</returns>
        double GetDouble(int index);

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Decimal" />.
        /// </summary>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns>The value of the specified column.</returns>
        decimal GetDecimal(int index);

        /// <summary>
        /// Gets the value of the specified column as a <see cref="TimeSpan" />.
        /// </summary>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns>
        /// The value of the specified column.
        /// </returns>
        TimeSpan GetTimeSpan(int index);

        /// <summary>
        /// Gets the value of the specified column as a <see cref="DateTime" />.
        /// </summary>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns>The value of the specified column.</returns>
        DateTime GetDateTime(int index);

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Guid" />.
        /// </summary>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns>The value of the specified column.</returns>
        Guid GetGuid(int index);

        /// <summary>
        /// Gets the value of the specified column as a <see cref="String" />.
        /// </summary>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns>The value of the specified column.</returns>
        string GetString(int index);

        /// <summary>
        /// Gets a value that indicates whether the column contains nonexistent or missing
        /// values.
        /// </summary>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns>true if the specified column is equivalent to System.DBNull; otherwise false.</returns>
        bool IsDBNull(int index);

        /// <summary>
        /// Reads a stream of bytes from the specified column.
        /// </summary>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns></returns>
        byte[] GetBytes(int index);
    }
}