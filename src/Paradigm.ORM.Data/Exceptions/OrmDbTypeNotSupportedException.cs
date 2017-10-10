using System;
using System.Data;

namespace Paradigm.ORM.Data.Exceptions
{
    public class OrmDbTypeNotSupportedException : Exception
    {
        public DbType Type { get; }

        public OrmDbTypeNotSupportedException(DbType type, string message) : base(message)
        {
            this.Type = type;
        }

        public OrmDbTypeNotSupportedException(DbType type, string message, Exception innerException) : base(message, innerException)
        {
            this.Type = type;
        }
    }
}