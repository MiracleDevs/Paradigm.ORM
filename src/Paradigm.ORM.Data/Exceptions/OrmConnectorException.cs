using System;

namespace Paradigm.ORM.Data.Exceptions
{
    public class OrmConnectorException : Exception
    {
        public OrmConnectorException(string message) : base(message)
        {
        }

        public OrmConnectorException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}