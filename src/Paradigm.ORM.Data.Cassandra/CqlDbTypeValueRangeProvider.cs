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
            switch (dataType.ToLower())
            {
                case "tinyint":
                    return 127;

                case "smallint":
                    return 32767;

                case "int":
                    return 2147483647;

                case "bigint":
                    return 9223372036854775807;

                case "date":
                    return new DateTime(9999, 12, 31);

                case "time":
                    return new TimeSpan(838, 59, 59);

                default:
                    return null;
            }
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
            switch (dataType.ToLower())
            {
                case "tinyint":
                    return -128;

                case "smallint":
                    return -32768;

                case "int":
                    return -2147483648;

                case "bigint":
                    return -9223372036854775808;

                case "date":
                    return new DateTime(1000, 01, 01);

                case "time":
                    return new TimeSpan(-838, 59, 59);

                default:
                    return null;
            }
        }
    }
}