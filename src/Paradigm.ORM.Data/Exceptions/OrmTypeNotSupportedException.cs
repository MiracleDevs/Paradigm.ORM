using System;

namespace Paradigm.ORM.Data.Exceptions
{
    public class OrmTypeNotSupportedException : Exception
    {
        public Type Type { get; }

        public OrmTypeNotSupportedException(Type type, string message) : base(message)
        {
            this.Type = type;
        }

        public OrmTypeNotSupportedException(Type type, string message, Exception innerException) : base(message, innerException)
        {
            this.Type = type;
        }
    }
}