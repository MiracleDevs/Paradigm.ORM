using System;

namespace Paradigm.ORM.Data.Exceptions
{
    public class OrmCanNotCloseConnectionException : Exception
    {
        public const string DefaultMessage = "The connection to the database can not be closed.";

        public OrmCanNotCloseConnectionException(): this(DefaultMessage)
        {
        }

        public OrmCanNotCloseConnectionException(string message) : base(message)
        {
        }

        public OrmCanNotCloseConnectionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}