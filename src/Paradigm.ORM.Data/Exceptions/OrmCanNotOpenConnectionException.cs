using System;

namespace Paradigm.ORM.Data.Exceptions
{
    public class OrmCanNotOpenConnectionException : Exception
    {
        public const string DefaultMessage = "The connection to the database can not be opened.";

        public OrmCanNotOpenConnectionException(): this(DefaultMessage)
        {
        }

        public OrmCanNotOpenConnectionException(string message) : base(message)
        {
        }

        public OrmCanNotOpenConnectionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}