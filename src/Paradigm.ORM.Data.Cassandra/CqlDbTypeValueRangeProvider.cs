using System;
using Paradigm.ORM.Data.Database;

namespace Paradigm.ORM.Data.Cassandra
{
    /// <summary>
    /// Retrieves the data type value minimum and maximums.
    /// </summary>
    internal class CqlDbTypeValueRangeProvider: IDbTypeValueRangeProvider
    {
        /// <summary>
        /// Gets the maximum value for a given sql type.
        /// </summary>
        /// <param name="dataType">Name of the sql type.</param>
        /// <returns>
        /// The maximum value possible for a given type; otherwise null.
        /// </returns>
        /// <remarks>
        /// The expected data type names are:
        /// tinyint, smallint, mediumint, int, bigint, date, datetime, timestamp, time.
        /// </remarks>
        public object GetMaxValue(string dataType)
        {
            return dataType.ToLower() switch
            {
                "tinyint" => 127,
                "smallint" => 32767,
                "int" => 2147483647,
                "bigint" => 9223372036854775807,
                "date" => new DateTime(9999, 12, 31),
                "time" => new TimeSpan(23, 59, 59),
                _ => null
            };
        }

        /// <summary>
        /// Gets the minimum value for a given sql type.
        /// </summary>
        /// <param name="dataType">Name of the sql type.</param>
        /// <returns>
        /// The minimum value possible for a given type; otherwise null.
        /// </returns>
        /// <remarks>
        /// The expected data type names are:
        /// tinyint, smallint, mediumint, int, bigint, date, datetime, timestamp, time.
        /// </remarks>
        public object GetMinValue(string dataType)
        {
            return dataType.ToLower() switch
            {
                "tinyint" => -128,
                "smallint" => -32768,
                "int" => -2147483648,
                "bigint" => -9223372036854775808,
                "date" => new DateTime(1970, 01, 01),
                "time" => new TimeSpan(0, 0, 0),
                _ => null
            };
        }
    }
}