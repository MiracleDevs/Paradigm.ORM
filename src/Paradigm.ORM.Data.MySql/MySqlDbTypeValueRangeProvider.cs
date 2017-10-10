using System;
using Paradigm.ORM.Data.Database;

namespace Paradigm.ORM.Data.MySql
{
    /// <summary>
    /// Retrieves the data type value minimum and maximums.
    /// </summary>
    internal class MySqlDbTypeValueRangeProvider: IDbTypeValueRangeProvider
    {
        /*
         Type	    Storage	    Minimum Value	        Maximum Value
        (Bytes)	                (Signed/Unsigned)	    (Signed/Unsigned)
        TINYINT	    1	        -128	                127
        SMALLINT    2	        -32768	                32767
        MEDIUMINT	3	        -8388608	            8388607
        INT	        4	        -2147483648	            2147483647
        BIGINT	    8	        -9223372036854775808	9223372036854775807
        DATE                    '1000-01-01'            '9999-12-31'
        DATETIME                '1000-01-01 00:00:00'   '9999-12-31 23:59:59'
        TIMESTAMP               '1970-01-01 00:00:01'   '2038-01-19 03:14:07'
        TIME                    '-838:59:59'            '838:59:59'
        */

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

                case "mediumint":
                    return 8388607;

                case "int":
                    return 2147483647;

                case "bigint":
                    return 9223372036854775807;

                case "date":
                    return new DateTime(9999, 12, 31);

                case "datetime":
                    return new DateTime(9999, 12, 31, 23, 59, 59);

                case "timestamp":
                    return new DateTime(2038, 01, 19, 03, 14, 07);

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

                case "mediumint":
                    return -8388608;

                case "int":
                    return -2147483648;

                case "bigint":
                    return -9223372036854775808;

                case "date":
                    return new DateTime(1000, 01, 01);

                case "datetime":
                    return new DateTime(1000, 01, 01, 00, 00, 00);

                case "timestamp":
                    return new DateTime(1970, 01, 01, 00, 00, 01);

                case "time":
                    return new TimeSpan(-838, 59, 59);

                default:
                    return null;
            }
        }
    }
}