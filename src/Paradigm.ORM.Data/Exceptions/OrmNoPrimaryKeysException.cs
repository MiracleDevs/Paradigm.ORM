using System;

namespace Paradigm.ORM.Data.Exceptions
{
    public class OrmNoPrimaryKeysException : Exception
    {
        public const string DefaultMessage = "The table '{0}' does not have primary keys defined.";

        public string Table { get; }

        public OrmNoPrimaryKeysException(string table): this(table, string.Format(DefaultMessage, table))
        {
        }

        public OrmNoPrimaryKeysException(string table, string message) : base(message)
        {
            this.Table = table;
        }

        public OrmNoPrimaryKeysException(string table, string message, Exception innerException) : base(message, innerException)
        {
            this.Table = table;
        }
    }
}