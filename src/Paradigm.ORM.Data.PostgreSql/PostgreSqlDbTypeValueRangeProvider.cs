using System;
using Paradigm.ORM.Data.Database;

namespace Paradigm.ORM.Data.PostgreSql
{
    internal class PostgreSqlDbTypeValueRangeProvider: IDbTypeValueRangeProvider
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
        /// smallint, samllserial, integer, serial, bigint, bigserial, date, timestamp, timestamp without time zone,
        /// timestamp with time zone, interval, time ,time without time zone, time with time zone.
        /// </remarks>
        public object GetMaxValue(string dataType)
        {
            switch (dataType.ToLower())
            {
                case "smallint":
                case "samllserial":
                    return 32767;

                case "integer":
                case "serial":
                    return 2147483647;

                case "bigint":
                case "bigserial":
                    return 9223372036854775807;

                case "date":
                    return new DateTime(9999, 12, 31);

                case "timestamp":
                case "timestamp without time zone":
                    return new DateTime(9999, 12, 31, 23, 59, 59);

                case "timestamp with time zone":
                    return new DateTime(9999, 12, 31, 23, 59, 59, DateTimeKind.Utc);

                case "interval":
                case "time":
                    return new TimeSpan(256204776, 0, 0);

                case "time without time zone":
                    return new TimeSpan(0, 24, 0);

                case "time with time zone":
                    return new TimeSpan(0, 24, 0);

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
        /// smallint, samllserial, integer, serial, bigint, bigserial, date, timestamp, timestamp without time zone,
        /// timestamp with time zone, interval, time ,time without time zone, time with time zone.
        /// </remarks>
        public object GetMinValue(string dataType)
        {
            switch (dataType.ToLower())
            {
                case "smallint":
                case "samllserial":
                    return -32768;

                case "integer":
                case "serial":
                    return -2147483648;

                case "bigint":
                case "bigserial":
                    return 9223372036854775808;

                case "date":
                    return new DateTime(1, 1, 1);

                case "timestamp":
                case "timestamp without time zone":
                    return new DateTime(1, 1, 1, 0, 0, 0);

                case "timestamp with time zone":
                    return new DateTime(1, 1, 1, 0, 0, 0, DateTimeKind.Utc);

                case "interval":
                case "time":
                    return new TimeSpan(-256204776, 0, 0);

                case "time without time zone":
                    return new TimeSpan(0, 0, 0);

                case "time with time zone":
                    return new TimeSpan(0, 0, 0);

                default:
                    return null;
            }
        }
    }
}