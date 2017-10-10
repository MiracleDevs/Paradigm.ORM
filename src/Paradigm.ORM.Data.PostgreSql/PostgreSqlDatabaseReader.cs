using Npgsql;
using System;
using System.IO;
using Paradigm.ORM.Data.Database;

namespace Paradigm.ORM.Data.PostgreSql
{
    /// <summary>
    ///  Reads a forward-only stream of rows from a data source.
    /// </summary>
    /// <seealso cref="Paradigm.ORM.Data.Database.IDatabaseReader" />
    internal partial class PostgreSqlDatabaseReader : IDatabaseReader
    {
        #region Properties

        /// <summary>
        /// Gets or sets the inner reader.
        /// </summary>
        private NpgsqlDataReader Reader { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PostgreSqlDatabaseReader"/> class.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <exception cref="System.ArgumentNullException">reader can not be null.</exception>
        public PostgreSqlDatabaseReader(NpgsqlDataReader reader)
        {
            this.Reader = reader ?? throw new ArgumentNullException(nameof(reader), $"{nameof(reader)} can not be null.");
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Reader?.Dispose();
            this.Reader = null;
        }

        /// <summary>
        /// Advances the DatabaseReader to the next record.
        /// </summary>
        /// <returns>
        /// true if there are more rows; otherwise false.
        /// </returns>
        public bool Read()
        {
            return this.Reader.Read();
        }

        /// <summary>
        /// Advances the data reader to the next result, when reading the results of batch
        /// SQL statements.
        /// </summary>
        /// <returns>
        /// true if there are more result sets; otherwise false.
        /// </returns>
        public bool NextResult()
        {
            return this.Reader.NextResult();
        }

        /// <summary>
        /// Gets the column ordinal, given the name of the column.
        /// </summary>
        /// <param name="name">The name of the column.</param>
        /// <returns>
        /// The zero-based column ordinal.
        /// </returns>
        public int GetOrdinal(string name)
        {
            return this.Reader.GetOrdinal(name);
        }

        /// <summary>
        /// Gets the value of the specified column as a type.
        /// </summary>
        /// <typeparam name="T">The type of the value to be returned. See the remarks section for more information.</typeparam>
        /// <param name="index">The column to be retrieved.</param>
        /// <returns>
        /// The returned type object.
        /// </returns>
        public T GetFieldValue<T>(int index)
        {
            return this.Reader.GetFieldValue<T>(index);
        }

        /// <summary>
        /// Gets the name of the specified column.
        /// </summary>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns>
        /// The name of the specified column.
        /// </returns>
        public string GetName(int index)
        {
            return this.Reader.GetName(index);
        }

        /// <summary>
        /// Gets the value of the specified column as an instance of <see cref="Object" />.
        /// </summary>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns>
        /// The value of the specified column.
        /// </returns>
        public object GetValue(int index)
        {
            return this.Reader.GetValue(index);
        }

        /// <summary>
        /// Gets the value of the specified column as an instance of <see cref="Object" />.
        /// </summary>
        /// <param name="name">The name of the column.</param>
        /// <returns>
        /// The value of the specified column.
        /// </returns>
        public object GetValue(string name)
        {
            return this.Reader.GetValue(this.Reader.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Boolean" />.
        /// </summary>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns>
        /// The value of the specified column.
        /// </returns>
        public bool GetBoolean(int index)
        {
            return this.Reader.GetBoolean(index);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="SByte" />.
        /// </summary>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns>
        /// The value of the specified column.
        /// </returns>
        public sbyte GetSByte(int index)
        {
            return (sbyte)this.Reader.GetByte(index);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Int16" />.
        /// </summary>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns>
        /// The value of the specified column.
        /// </returns>
        public short GetInt16(int index)
        {
            return this.Reader.GetInt16(index);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Int32" />.
        /// </summary>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns>
        /// The value of the specified column.
        /// </returns>
        public int GetInt32(int index)
        {
            return this.Reader.GetInt32(index);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Int64" />.
        /// </summary>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns>
        /// The value of the specified column.
        /// </returns>
        public long GetInt64(int index)
        {
            return this.Reader.GetInt64(index);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Byte" />.
        /// </summary>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns>
        /// The value of the specified column.
        /// </returns>
        public byte GetByte(int index)
        {
            return this.Reader.GetByte(index);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="UInt16" />.
        /// </summary>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns>
        /// The value of the specified column.
        /// </returns>
        public ushort GetUInt16(int index)
        {
            return (ushort)this.Reader.GetInt16(index);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="UInt32" />.
        /// </summary>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns>
        /// The value of the specified column.
        /// </returns>
        public uint GetUInt32(int index)
        {
            return (uint)this.Reader.GetInt32(index);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="UInt64" />.
        /// </summary>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns>
        /// The value of the specified column.
        /// </returns>
        public ulong GetUInt64(int index)
        {
            return (ulong)this.Reader.GetInt64(index);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Single" />.
        /// </summary>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns>
        /// The value of the specified column.
        /// </returns>
        public float GetFloat(int index)
        {
            return this.Reader.GetFloat(index);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Double" />.
        /// </summary>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns>
        /// The value of the specified column.
        /// </returns>
        public double GetDouble(int index)
        {
            return this.Reader.GetDouble(index);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Decimal" />.
        /// </summary>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns>
        /// The value of the specified column.
        /// </returns>
        public decimal GetDecimal(int index)
        {
            return this.Reader.GetDecimal(index);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="TimeSpan" />.
        /// </summary>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns>
        /// The value of the specified column.
        /// </returns>
        public TimeSpan GetTimeSpan(int index)
        {
            return this.Reader.GetTimeSpan(index);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="DateTime" />.
        /// </summary>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns>
        /// The value of the specified column.
        /// </returns>
        public DateTime GetDateTime(int index)
        {
            return this.Reader.GetDateTime(index);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Guid" />.
        /// </summary>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns>
        /// The value of the specified column.
        /// </returns>
        public Guid GetGuid(int index)
        {
            return this.Reader.GetGuid(index);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="String" />.
        /// </summary>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns>
        /// The value of the specified column.
        /// </returns>
        public string GetString(int index)
        {
            return this.Reader.GetString(index);
        }

        /// <summary>
        /// Reads a stream of bytes from the specified column.
        /// </summary>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns></returns>
        public byte[] GetBytes(int index)
        {
            const int bufferSize = 1024;
            var buffer = new byte[bufferSize];
            long readPointer = 0;

            using (var memoryStream = new MemoryStream())
            {
                using (var binaryWriter = new BinaryWriter(memoryStream))
                {
                    long readAmount;

                    do
                    {
                        readAmount = this.Reader.GetBytes(index, readPointer, buffer, 0, bufferSize);
                        readPointer += readAmount;

                        binaryWriter.Write(buffer);
                        binaryWriter.Flush();

                    } while (readAmount == bufferSize);

                    return memoryStream.ToArray();
                }
            }
        }

        /// <summary>
        /// Gets a value that indicates whether the column contains nonexistent or missing
        /// values.
        /// </summary>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns>
        /// true if the specified column is equivalent to System.DBNull; otherwise false.
        /// </returns>
        public bool IsDBNull(int index)
        {
            return this.Reader.IsDBNull(index);
        }

        #endregion
    }
}