using System;
using Paradigm.ORM.Data.Database;

namespace Paradigm.ORM.Data.SqlServer
{
    internal class SqlDbTypeValueRangeProvider: IDbTypeValueRangeProvider
    {
        /*
         Type	    Storage	    Minimum Value	        Maximum Value
        (Bytes)	                (Signed/Unsigned)	    (Signed/Unsigned)
        TINYINT	    1	        -128	                127
        SMALLINT    2	        -32768	                32767
        MEDIUMINT	3	        -8388608	            8388607
        INT	        4	        -2147483648	            2147483647
        BIGINT	    8	        -9223372036854775808	9223372036854775807
        DATE                    '0001-01-01'            '9999-12-31'
        DATETIME                '1753-01-01 00:00:00'   '9999-12-31 23:59:59'
        DATETIME2               '0001-01-01 00:00:00'   '9999-12-31 23:59:59'
        DATETIMEOFFSET          '0001-01-01 00:00:00'   '9999-12-31 23:59:59'
        SMALLDATETIME           '1900-01-01 00:00:00'   '2079-06-06 23:59:59'
        TIME                    '00:00:00'              '23:59:59'
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
        /// tinyint, smallint, mediumint, int, bigint, date, datetime, datetime2, datetimeoffset, smalldatetime, time.
        /// </remarks>
        public object GetMaxValue(string dataType)
        {
            return dataType.ToLower() switch
            {
                "tinyint" => 127,
                "smallint" => 32767,
                "mediumint" => 8388607,
                "int" => 2147483647,
                "bigint" => 9223372036854775807,
                "date" => new DateTime(9999, 12, 31),
                "datetime" => new DateTime(9999, 12, 31, 23, 59, 59),
                "datetime2" => new DateTime(9999, 12, 31, 23, 59, 59),
                "datetimeoffset" => new DateTimeOffset(new DateTime(9999, 12, 31, 23, 59, 59)),
                "smalldatetime" => new DateTime(2079, 06, 06, 23, 59, 59),
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
        /// tinyint, smallint, mediumint, int, bigint, date, datetime, datetime2, datetimeoffset, smalldatetime, time.
        /// </remarks>
        public object GetMinValue(string dataType)
        {
            return dataType.ToLower() switch
            {
                "tinyint" => -128,
                "smallint" => -32768,
                "mediumint" => -8388608,
                "int" => -2147483648,
                "bigint" => -9223372036854775808,
                "date" => new DateTime(1000, 01, 01),
                "datetime" => new DateTime(1753, 01, 01, 00, 00, 00),
                "datetime2" => new DateTime(0001, 01, 01, 00, 00, 00),
                "datetimeoffset" => new DateTime(0001, 01, 01, 00, 00, 00),
                "smalldatetime" => new DateTime(1900, 01, 01, 00, 00, 00),
                "time" => new TimeSpan(00, 00, 00),
                _ => null
            };
        }
    }
}