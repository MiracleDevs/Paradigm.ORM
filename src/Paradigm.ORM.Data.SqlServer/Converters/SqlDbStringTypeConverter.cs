using System;
using Paradigm.ORM.Data.Converters;
using Paradigm.ORM.Data.Database.Schema.Structure;

namespace Paradigm.ORM.Data.SqlServer.Converters
{
    /// <summary>
    /// Provides an interface to convert from database types or database schema tables to .net types.
    /// </summary>
    /// <seealso cref="Paradigm.ORM.Data.Converters.IDbStringTypeConverter" />
    public class SqlDbStringTypeConverter : IDbStringTypeConverter
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
                case TypeCode.Boolean:
                    return "bit";

                case TypeCode.SByte:
                case TypeCode.Byte:
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
                    return "real";

                case TypeCode.Double:
                    return "float";

                case TypeCode.Decimal:
                    return "decimal";

                case TypeCode.DateTime:
                    return "DateTime";

                case TypeCode.Char:
                case TypeCode.String:
                    return "text";

                case TypeCode.Object:
                    return "binary";
            }

            if (type == typeof(DateTimeOffset))
                return "datetimeoffset";

            if (type == typeof(Guid))
                return "uniqueidentifier";

            if (type == typeof(byte[]))
                return "binary";

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
                SQL Type		    .NET Type
                                    
                bit				    Boolean
                tinyint			    Byte
                smallint		    Int16	
                int				    Int32
                bigint			    Int64
                                    
                real			    Single	
                float			    Double
                money			    Decimal
                numeric			    Decimal
                decimal			    Decimal
                smallmoney		    Decimal
                                    
                date			    DateTime
                smalldatetime	    DateTime
                datetime		    DateTime
                datetime2		    DateTime
                datetimeoffset	    DateTimeOffset	
                time			    TimeSpan   (The documentation is wrong, and will throw a 
                                               'Failed to convert parameter value from a TimeSpan to a DateTime.'. 
                                                The correct type is DateTime).
                                    
                char			    String
                text			    String
                varchar			    String
                nchar			    String
                ntext			    String
                nvarchar		    String
                                    
                binary			    Byte[]
                varbinary		    Byte[]
                FILESTREAM  	    Byte[]	
                image			    Byte[]
                rowversion		    Byte[]
                timestamp		    Byte[]
                                    
                sql_variant		    Object 
                uniqueidentifier    Guid
                xml				    Xml		
            */

            switch (dbType.ToLower())
            {
                case "bit":
                    return typeof(bool);

                case "tinyint":
                    return typeof(byte);

                case "smallint":
                    return typeof(short);

                case "int":
                    return typeof(int);

                case "bigint":
                    return typeof(long);

                case "real":
                    return typeof(float);

                case "float":
                    return typeof(double);

                case "money":
                case "smallmoney":
                case "numeric":
                case "decimal":
                    return typeof(decimal);

                // NOTE: Do not use timespan. It has a conversion bug.
                // If we set the type of time as TimeSpan we get the error
                // 'Failed to convert parameter value from a TimeSpan to a DateTime.'
                // If we set the type of time as DateTime, the SqlDataReader maps
                // a Sql time field to a TimeSpan object and we can't cast it back to
                // DateTime
                /*case "time":
                    return typeof(TimeSpan);*/

                case "date":
                case "datetime":
                case "datetime2":
                case "smalldatetime":
                    return typeof(DateTime);

                case "datetimeoffset":
                    return typeof(DateTimeOffset);

                case "char":
                case "text":
                case "varchar":
                case "nchar":
                case "ntext":
                case "nvarchar":
                case "xml":
                    return typeof(string);

                case "binary":
                case "varbinary":
                case "filestream":
                case "image":
                case "rowversion":
                case "timestamp":
                    return typeof(byte[]);

                case "sql_variant":
                    return typeof(object);

                default:
                    return typeof(object);
            }
        }
    }
}