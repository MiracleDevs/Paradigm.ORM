using System;

namespace Paradigm.ORM.Data.Exceptions
{
    public class OrmTransactionStackEmptyException : Exception
    {
        public const string DefaultMessage = "The transaction stack is empty.";

        public OrmTransactionStackEmptyException() : this(DefaultMessage)
        {
        }

        public OrmTransactionStackEmptyException(string message) : base(message)
        {
        }

        public OrmTransactionStackEmptyException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}