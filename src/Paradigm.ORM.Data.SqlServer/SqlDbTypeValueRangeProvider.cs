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

                case "datetime2":
                    return new DateTime(9999, 12, 31, 23, 59, 59);

                case "datetimeoffset":
                    return new DateTimeOffset(new DateTime(9999, 12, 31, 23, 59, 59));

                case "smalldatetime":
                    return new DateTime(2079, 06, 06, 23, 59, 59);

                case "time":
                    return new TimeSpan(23, 59, 59);

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
        /// tinyint, smallint, mediumint, int, bigint, date, datetime, datetime2, datetimeoffset, smalldatetime, time.
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
                    return new DateTime(1753, 01, 01, 00, 00, 00);

                case "datetime2":
                    return new DateTime(0001, 01, 01, 00, 00, 00);

                case "datetimeoffset":
                    return new DateTime(0001, 01, 01, 00, 00, 00);

                case "smalldatetime":
                    return new DateTime(1900, 01, 01, 00, 00, 00);

                case "time":
                    return new TimeSpan(00, 00, 00);

                default:
                    return null;
            }
        }
    }
}