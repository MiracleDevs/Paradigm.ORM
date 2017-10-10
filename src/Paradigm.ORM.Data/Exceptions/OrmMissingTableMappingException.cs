using System;

namespace Paradigm.ORM.Data.Exceptions
{
    public class OrmMissingTableMappingException : Exception
    {
        public const string DefaultMessage = "The class or interface does not have table mapping information.";

        public OrmMissingTableMappingException() : this((string) DefaultMessage)
        {           
        }

        public OrmMissingTableMappingException(string message) : base(message)
        {
        }

        public OrmMissingTableMappingException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}