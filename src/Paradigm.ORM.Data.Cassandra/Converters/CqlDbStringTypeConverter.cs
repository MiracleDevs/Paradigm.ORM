using System;
using Cassandra;
using Paradigm.ORM.Data.Converters;
using Paradigm.ORM.Data.Database.Schema.Structure;

namespace Paradigm.ORM.Data.Cassandra.Converters
{
    /// <summary>
    /// Provides an interface to convert from database types or database schema tables to .net types.
    /// </summary>
    /// <seealso cref="Paradigm.ORM.Data.Converters.IDbStringTypeConverter" />
    public class CqlDbStringTypeConverter : IDbStringTypeConverter
    {
        public static string ValidatorToDbType(string validator)
        {
            switch(validator)
            {
                case "org.apache.cassandra.db.marshal.BooleanType":
                    return "boolean";

                case "org.apache.cassandra.db.marshal.ByteType":
                    return "tinyint";

                case "org.apache.cassandra.db.marshal.ShortType":
                    return "smallint";

                case "org.apache.cassandra.db.marshal.Int32Type":
                    return "int";

                case "org.apache.cassandra.db.marshal.LongType":
                    return "bigint";

                case "org.apache.cassandra.db.marshal.IntegerType":
                    return "varint";

                case "org.apache.cassandra.db.marshal.FloatType":
                    return "float";

                case "org.apache.cassandra.db.marshal.DoubleType":
                    return "double";

                case "org.apache.cassandra.db.marshal.DecimalType":
                    return "decimal";

                case "org.apache.cassandra.db.marshal.SimpleDateType":
                    return "date";

                case "org.apache.cassandra.db.marshal.TimeType":
                    return "time";

                case "org.apache.cassandra.db.marshal.TimestampType":
                    return "timestamp";

                case "org.apache.cassandra.db.marshal.TimeUUIDType":
                    return "timeuuid";

                case "org.apache.cassandra.db.marshal.UUIDType":
                    return "uuid";

                case "org.apache.cassandra.db.marshal.UTF8Type":
                    return "text";

                case "org.apache.cassandra.db.marshal.AsciiType":
                    return "ascii";

                case "org.apache.cassandra.db.marshal.BytesType":
                    return "blob";

                case "org.apache.cassandra.db.marshal.InetAddressType":
                    return "inet";
                
                default:
                    throw new Exception("CQL Validator not recognized.");
            }
        }

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
            /*************************************************************************** 
            * boolean   | org.apache.cassandra.db.marshal.BooleanType
            * 
            * tinyint   | org.apache.cassandra.db.marshal.ByteType
            * smallint  | org.apache.cassandra.db.marshal.ShortType
            * int       | org.apache.cassandra.db.marshal.Int32Type
            * bigint    | org.apache.cassandra.db.marshal.LongType
            * varint    | org.apache.cassandra.db.marshal.IntegerType
            * 
            * float     | org.apache.cassandra.db.marshal.FloatType
            * double    | org.apache.cassandra.db.marshal.DoubleType
            * decimal   | org.apache.cassandra.db.marshal.DecimalType
            * 
            * date      | org.apache.cassandra.db.marshal.SimpleDateType
            * time      | org.apache.cassandra.db.marshal.TimeType
            * timestamp | org.apache.cassandra.db.marshal.TimestampType
            * timeuuid  | org.apache.cassandra.db.marshal.TimeUUIDType
            * 
            * uuid      | org.apache.cassandra.db.marshal.UUIDType               
            * text      | org.apache.cassandra.db.marshal.UTF8Type
            * varchar   | org.apache.cassandra.db.marshal.UTF8Type
            * ascii     | org.apache.cassandra.db.marshal.AsciiType
            * blob      | org.apache.cassandra.db.marshal.BytesType
            * inet      | org.apache.cassandra.db.marshal.InetAddressType
            ****************************************************************************/

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Boolean:
                    return "boolean";

                case TypeCode.Byte:
                case TypeCode.SByte:               
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

                case TypeCode.Char:
                case TypeCode.String:
                    return "text";

                case TypeCode.Object:
                    return "blob";
            }

            if (type == typeof(Guid))
                return "uuid";

            if (type == typeof(byte[]))
                return "blob";

            if (type == typeof(LocalTime))
                return "time";

            if (type == typeof(LocalDate))
                return "date";

            if (type == typeof(DateTimeOffset))
                return "timestamp";
                       
            return null;
        }

        /// <summary>
        /// Gets the type of the native.
        /// </summary>
        /// <param name="dbType">Type of the database.</param>
        /// <returns></returns>
        private Type GetNativeType(string dbType)
        {
            switch (dbType.ToLower())
            {
                case "boolean":
                    return typeof(bool);

                case "tinyint":
                    return typeof(sbyte);

                case "smallint":
                    return typeof(short);

                case "int":
                    return typeof(int);

                case "bigint":
                    return typeof(long);

                case "varint":
                    return typeof(long);

                case "float":
                    return typeof(float);

                case "double":
                    return typeof(double);

                case "decimal":
                    return typeof(decimal);

                case "date":
                    return typeof(LocalDate);

                case "time":
                    return typeof(LocalTime);

                case "timestamp":
                    return typeof(DateTimeOffset);

                case "timeuuid":
                    throw new NotImplementedException();

                case "uuid":
                    return typeof(Guid);

                case "text":
                    return typeof(string);

                case "varchar":
                    return typeof(string);

                case "ascii":
                    return typeof(string);

                case "blob":
                    return typeof(byte[]);

                case "inet":
                    return typeof(string);

                default:
                    return typeof(object);
            }
        }
    }
}