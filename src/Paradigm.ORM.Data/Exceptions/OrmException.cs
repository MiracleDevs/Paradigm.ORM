using System;

namespace Paradigm.ORM.Data.Exceptions
{
    public class OrmException : Exception
    {
        public OrmException(string message) : base(message)
        {
        }

        public OrmException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}