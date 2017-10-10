using System;

namespace Paradigm.ORM.Data.Exceptions
{
    public class OrmMissingRoutineMappingException : Exception
    {
        public const string DefaultMessage = "The class or interface does not have routine mapping information.";

        public OrmMissingRoutineMappingException() : this((string) DefaultMessage)
        {           
        }

        public OrmMissingRoutineMappingException(string message) : base(message)
        {
        }

        public OrmMissingRoutineMappingException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}