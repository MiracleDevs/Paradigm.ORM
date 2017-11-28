using System;
using Paradigm.ORM.Data.Converters;
using Paradigm.ORM.Data.Database.Schema.Structure;

namespace Paradigm.ORM.Data.MySql.Converters
{
    /// <summary>
    /// Provides an interface to convert from database types or database schema tables to .net types.
    /// </summary>
    /// <seealso cref="IDbStringTypeConverter" />
    public class MySqlDbStringTypeConverter : IDbStringTypeConverter
    {
        /// <summary>
        /// Gets the equivalent .net type from a database type.
        /// </summary>
        /// <param name="dbType">A database type.</param>
        /// <param name="isNullable">True if the value is nullable; false otherwise.</param>
        /// <returns>
        /// The equivalent .net type.
        /// </returns>
        public Type GetType(string dbType, bool isNullable = false)
        {
            var type = this.GetNativeType(dbType);

            if (!isNullable || type == typeof(string) || type == typeof(byte[]) || type == typeof(object))
                return type;

            return typeof(Nullable<>).MakeGenericType(type);
        }

        /// <summary>
        /// Gets the equivalent .net type from a parameter schema.
        /// </summary>
        /// <param name="parameter">The parameter schema.</param>
        /// <returns>
        /// The equivalent .net type.
        /// </returns>
        public Type GetType(IParameter parameter)
        {
            return this.GetType(parameter.DataType, true);
        }

        /// <summary>
        /// Gets the equivalent .net type from a column schema.
        /// </summary>
        /// <param name="column">The column schema.</param>
        /// <returns>
        /// The equivalent .net type.
        /// </returns>
        public Type GetType(IColumn column)
        {
            return this.GetType(column.DataType, column.IsNullable);
        }

        /// <summary>
        /// Gets the equivalent database type from a .net type.
        /// </summary>
        /// <param name="type">The .net type.</param>
        /// <returns>
        /// The equivalent database type.
        /// </returns>
        public string GetDbStringType(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.Boolean:
                    return "tinyint";

                case TypeCode.UInt16:
                case TypeCode.Int16:
                    return "smallint";

                case TypeCode.Int32:
                case TypeCode.UInt32:
                    return "int";

                case TypeCode.Int64:
                case TypeCode.UInt64:
                    return "bigint";

                case TypeCode.Single:
                    return "float";

                case TypeCode.Double:
                    return "double";

                case TypeCode.Decimal:
                    return "decimal";

                case TypeCode.DateTime:
                    return "datetime";

                case TypeCode.Char:
                case TypeCode.String:
                    return "text";

                case TypeCode.Object:
                    return "binary";
            }

            if (type == typeof(Guid))
                return "binary";

            if (type == typeof(byte[]))
                return "binary";

            if (type == typeof(TimeSpan))
                return "time";

            return null;
        }

        /// <summary>
        /// Gets the type of the native.
        /// </summary>
        /// <param name="dbType">Type of the database.</param>
        /// <returns></returns>
        private Type GetNativeType(string dbType)
        {
            /* 
            SQL Type of Received Value 	    buffer_type Value 	        Output Variable C Type
            TINYINT 	                    MYSQL_TYPE_TINY 	        signed char
            SMALLINT 	                    MYSQL_TYPE_SHORT 	        short int
            MEDIUMINT 	                    MYSQL_TYPE_INT24 	        int
            INT 	                        MYSQL_TYPE_LONG 	        int
            BIGINT 	                        MYSQL_TYPE_LONGLONG 	    long long int
            FLOAT 	                        MYSQL_TYPE_FLOAT 	        float
            DOUBLE 	                        MYSQL_TYPE_DOUBLE 	        double
            DECIMAL 	                    MYSQL_TYPE_NEWDECIMAL 	    char[]
            YEAR 	                        MYSQL_TYPE_SHORT 	        short int
            TIME 	                        MYSQL_TYPE_TIME 	        MYSQL_TIME
            DATE 	                        MYSQL_TYPE_DATE 	        MYSQL_TIME
            DATETIME 	                    MYSQL_TYPE_DATETIME 	    MYSQL_TIME
            TIMESTAMP 	                    MYSQL_TYPE_TIMESTAMP 	    MYSQL_TIME
            CHAR, BINARY 	                MYSQL_TYPE_STRING 	        char[]
            NVARCHAR,VARCHAR, VARBINARY     MYSQL_TYPE_VAR_STRING 	    char[]
            TINYBLOB, TINYTEXT 	            MYSQL_TYPE_TINY_BLOB 	    char[]
            BLOB, TEXT 	                    MYSQL_TYPE_BLOB 	        char[]
            MEDIUMBLOB, MEDIUMTEXT 	        MYSQL_TYPE_MEDIUM_BLOB 	    char[]
            LONGBLOB, LONGTEXT 	            MYSQL_TYPE_LONG_BLOB 	    char[]
            BIT 	                        MYSQL_TYPE_BIT 	            char[]
            */

            switch (dbType.ToLower())
            {
                case "boolean":
                case "tinyint":
                    return typeof(bool);

                case "smallint":
                    return typeof(short);

                case "mediumint":
                case "int":
                    return typeof(int);

                case "bigint":
                    return typeof(long);

                case "float":
                    return typeof(float);

                case "double":
                    return typeof(double);

                case "decimal":
                    return typeof(decimal);

                case "year":
                    return typeof(short);

                case "time":
                    return typeof(TimeSpan);
                case "date":
                case "datetime":
                case "timestamp":
                    return typeof(DateTime);

                case "bit":
                case "char":
                case "varchar":
                case "nvarchar":
                case "tinytext":
                case "text":
                case "mediumtext":
                case "longtext":
                    return typeof(string);

                case "binary":
                case "varbinary":
                case "tinyblob":
                case "blob":
                case "mediumblob":
                case "longblob":
                    return typeof(byte[]);

                default:
                    return typeof(object);
            }
        }
    }
}