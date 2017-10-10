using System;
using System.Collections;
using Paradigm.ORM.Data.Converters;
using Paradigm.ORM.Data.Database.Schema.Structure;

namespace Paradigm.ORM.Data.PostgreSql.Converters
{
    /// <summary>
    /// Provides an interface to convert from database types or database schema tables to .net types.
    /// </summary>
    /// <seealso cref="Paradigm.ORM.Data.Converters.IDbStringTypeConverter" />
    public class PostgreSqlDbStringTypeConverter : IDbStringTypeConverter
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
                case TypeCode.Boolean:
                    return "boolean";

                case TypeCode.Byte:
                case TypeCode.SByte:
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
                    return "date";

                case TypeCode.Char:
                case TypeCode.String:
                    return "text";

                case TypeCode.Object:
                    return "bytea";
            }

            if (type == typeof(Guid))
                return "varchar";

            if (type == typeof(byte[]))
                return "bytea";

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
            http://www.npgsql.org/doc/types/basic.html#type-mappings-when-reading-values-sent-from-the-backend

            PostgreSQL type	  |  Default .NET type	          | Provider-specific type	  | Other .NET types
            ------------------+-------------------------------+---------------------------+-------------------------------------------------------
            bool	          |  bool	                      |  -	                      |  -
            int2	          |  short	                      |  -	                      |  byte, sbyte, int, long, float, double, decimal, string
            int4	          |  int	                      |  -	                      |  byte, short, long, float, double, decimal, string
            int8	          |  long	                      |  -	                      |  long, byte, short, int, float, double, decimal, string
            float4	          |  float	                      |  -	                      |  double
            float8	          |  double	                      |  -	                      |  -
            numeric	          |  decimal	                  |  -	                      |  byte, short, int, long, float, double, string
            money	          |  decimal	                  |  -	                      |  -
            text	          |  string	                      |  -	                      |  char[]
            varchar	          |  string	                      |  -	                      |  char[]
            bpchar	          |  string	                      |  -	                      |  char[]
            citext	          |  string	                      |  -	                      |  char[]
            json	          |  string	                      |  -	                      |  char[]
            jsonb	          |  string	                      |  -	                      |  char[]
            xml	              |  string	                      |  -	                      |  char[]
            point	          |  NpgsqlPoint	              |  -	                      |  string
            lseg	          |  NpgsqlLSeg	                  |  -	                      |  string
            path	          |  NpgsqlPath	                  |  -	                      |  -
            polygon	          |  NpgsqlPolygon	              |  -	                      |  -
            line	          |  NpgsqlLine	                  |  -	                      |  string
            circle	          |  NpgsqlCircle	              |  -	                      |  string
            box	              |  NpgsqlBox	                  |  -	                      |  string
            bit(1)	          |  bool	                      |  -	                      |  BitArray
            bit(n)	          |  BitArray	                  |  -	                      |  -
            varbit	          |  BitArray	                  |  -	                      |  -
            hstore	          |  IDictionary	              |  -	                      |  string
            uuid	          |  Guid	                      |  -	                      |  string
            cidr	          |  NpgsqlInet	                  |  -	                      |  string
            inet	          |  IPAddress	                  |  NpgsqlInet	              |  string
            macaddr	          |  PhysicalAddress	          |  -	                      |  string
            tsquery	          |  NpgsqlTsQuery	              |  -	                      |  -
            tsvector	      |  NpgsqlTsVector	              |  -	                      |  -
            date	          |  DateTime	                  |  NpgsqlDate	              |  -
            interval	      |  TimeSpan	                  |  NpgsqlTimeSpan	          |  -
            timestamp	      |  DateTime	                  |  NpgsqlDateTime	          |  -
            timestamptz	      |  DateTime	                  |  NpgsqlDateTime	          |  DateTimeOffset
            time	          |  TimeSpan	                  |  -	                      |  -
            timetz	          |  DateTimeOffset	              |  -	                      |  DateTimeOffset, DateTime, TimeSpan
            bytea	          |  byte[]	                      |  -	                      |  -
            oid	              |  uint	                      |  -	                      |  -
            xid	              |  uint	                      |  -	                      |  -
            cid	              |  uint	                      |  -	                      |  -
            oidvector	      |  uint[]	                      |  -	                      |  -
            name	          |  string	                      |  -	                      |  char[]
            (internal) char	  |  char	                      |  -	                      |  byte, short, int, long
            geometry (PostGIS)|	 PostgisGeometry	          |  -	                      |  -
            record	          |  object[]	                  |  -	                      |  -
            composite         |  types	T	                  |  -	                      |  -
            range subtypes	  |  NpgsqlRange	              |  -	                      |  -
            enum types	      |  TEnum	                      |  -	                      |  -
            array types	      |  Array (of child element type)|	 -	                      |  -

            */

            switch (dbType.ToLower())
            {
                case "bigint":
                case "int8":
                case "bigserial":
                case "serial8":
                    return typeof(long);

                // ADO.NET does not have support for BitArray type
                case "boolean":
                case "bool":
                case "bit": 
                    return typeof(bool);
               
                case "varbit":
                    return typeof(BitArray);

                case "bytea":
                    return typeof(byte[]);

                case "int":
                case "integer":
                case "smallint":
                case "serial4":
                    return typeof(int);                

                case "float":
                    return typeof(float);

                case "double":
                case "double precision":
                    return typeof(double);

                case "money":
                case "decimal":
                case "numeric":
                    return typeof(decimal);

                case "real":
                    return typeof(float);
                
                case "date":
                case "timestamp":
                case "timestamp without time zone":
                case "timestamp with time zone":
                    return typeof(DateTime);

                case "time":
                case "time without time zone":
                case "time with time zone":
                case "interval":
                    return typeof(TimeSpan);

                case "char":
                case "character":
                case "varchar":
                case "character varying":
                case "text":
                    return typeof(string);

                default:
                    return typeof(object);
            }
        }
    }
}