using System;
using System.Collections.Generic;
using System.Data;
using Paradigm.ORM.Data.Exceptions;

namespace Paradigm.ORM.Data.Converters
{
    /// <summary>
    /// Provides methods to convert between .NET types and DbTypes.
    /// </summary>
    public static class DbTypeConverter
    {
        /// <summary>
        /// Gets the dictionary with mappings from type to dbtypes.
        /// </summary>
        private static Dictionary<Type, DbType> TypeToDbType { get; }

        /// <summary>
        /// Gets the dictionary with mappings from dbtype to types.
        /// </summary>
        private static Dictionary<DbType, Type> DbTypeToType { get; }

        /// <summary>
        /// Initializes the <see cref="DbTypeConverter"/> class.
        /// </summary>
        static DbTypeConverter()
        {
            TypeToDbType = new Dictionary<Type, DbType>
            {
                [typeof(byte)] = DbType.Byte,
                [typeof(sbyte)] = DbType.SByte,
                [typeof(short)] = DbType.Int16,
                [typeof(ushort)] = DbType.UInt16,
                [typeof(int)] = DbType.Int32,
                [typeof(uint)] = DbType.UInt32,
                [typeof(long)] = DbType.Int64,
                [typeof(ulong)] = DbType.UInt64,
                [typeof(float)] = DbType.Single,
                [typeof(double)] = DbType.Double,
                [typeof(decimal)] = DbType.Decimal,
                [typeof(bool)] = DbType.Boolean,
                [typeof(string)] = DbType.String,
                [typeof(char)] = DbType.StringFixedLength,
                [typeof(Guid)] = DbType.Guid,
                [typeof(DateTime)] = DbType.DateTime,
                [typeof(DateTimeOffset)] = DbType.DateTimeOffset,
                [typeof(byte[])] = DbType.Binary,
                [typeof(byte?)] = DbType.Byte,
                [typeof(sbyte?)] = DbType.SByte,
                [typeof(short?)] = DbType.Int16,
                [typeof(ushort?)] = DbType.UInt16,
                [typeof(int?)] = DbType.Int32,
                [typeof(uint?)] = DbType.UInt32,
                [typeof(long?)] = DbType.Int64,
                [typeof(ulong?)] = DbType.UInt64,
                [typeof(float?)] = DbType.Single,
                [typeof(double?)] = DbType.Double,
                [typeof(decimal?)] = DbType.Decimal,
                [typeof(bool?)] = DbType.Boolean,
                [typeof(char?)] = DbType.StringFixedLength,
                [typeof(Guid?)] = DbType.Guid,
                [typeof(TimeSpan)] = DbType.Time,
                [typeof(DateTime?)] = DbType.DateTime,
                [typeof(DateTimeOffset?)] = DbType.DateTimeOffset
            };

            DbTypeToType = new Dictionary<DbType, Type>
            {
                [DbType.Byte] = typeof(byte),
                [DbType.SByte] = typeof(sbyte),
                [DbType.Int16] = typeof(short),
                [DbType.UInt16] = typeof(ushort),
                [DbType.Int32] = typeof(int),
                [DbType.UInt32] = typeof(uint),
                [DbType.Int64] = typeof(long),
                [DbType.UInt64] = typeof(ulong),
                [DbType.Single] = typeof(float),
                [DbType.Double] = typeof(double),
                [DbType.Decimal] = typeof(decimal),
                [DbType.Boolean] = typeof(bool),
                [DbType.String] = typeof(string),
                [DbType.StringFixedLength] = typeof(char),
                [DbType.Guid] = typeof(Guid),
                [DbType.DateTime] = typeof(DateTime),
                [DbType.DateTimeOffset] = typeof(DateTimeOffset),
                [DbType.Binary] = typeof(byte[]),
                [DbType.Byte] = typeof(byte?),
                [DbType.SByte] = typeof(sbyte?),
                [DbType.Int16] = typeof(short?),
                [DbType.UInt16] = typeof(ushort?),
                [DbType.Int32] = typeof(int?),
                [DbType.UInt32] = typeof(uint?),
                [DbType.Int64] = typeof(long?),
                [DbType.UInt64] = typeof(ulong?),
                [DbType.Single] = typeof(float?),
                [DbType.Double] = typeof(double?),
                [DbType.Decimal] = typeof(decimal?),
                [DbType.Boolean] = typeof(bool?),
                [DbType.StringFixedLength] = typeof(char?),
                [DbType.Guid] = typeof(Guid?),
                [DbType.Time] = typeof(TimeSpan),
                [DbType.DateTime] = typeof(DateTime?),
                [DbType.DateTimeOffset] = typeof(DateTimeOffset?)
            };
        }

        /// <summary>
        /// Converts from a type to a db type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>A dbtype equivalent to the type provided.</returns>
        public static DbType FromType(Type type)
        {
            if (!TypeToDbType.ContainsKey(type))
                throw new OrmTypeNotSupportedException(type, $"Type '{type}' not supported.");

            return TypeToDbType[type];
        }

        /// <summary>
        /// Converts from a dbtype to a .net type.
        /// </summary>
        /// <param name="type">The db type.</param>
        /// <returns>A .net type equivalent to the db type.</returns>
        public static Type FromDbType(DbType type)
        {
            if (!DbTypeToType.ContainsKey(type))
                throw new OrmDbTypeNotSupportedException(type, $"Type '{type}' not supported.");

            return DbTypeToType[type];
        }
    }
}