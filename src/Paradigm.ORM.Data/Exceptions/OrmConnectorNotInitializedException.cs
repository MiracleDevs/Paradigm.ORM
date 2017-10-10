using System;

namespace Paradigm.ORM.Data.Exceptions
{
    public class OrmConnectorNotInitializedException : Exception
    {
        public const string DefaultMessage = "The connector has not been initialized.";

        public OrmConnectorNotInitializedException() : this(DefaultMessage)
        {
        }

        public OrmConnectorNotInitializedException(string message) : base(message)
        {
        }

        public OrmConnectorNotInitializedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}