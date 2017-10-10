using System;

namespace Paradigm.ORM.Data.Attributes
{
    /// <summary>
    /// Indicates that the property maps to a primary key column.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class PrimaryKeyAttribute : Attribute
    {

    }
}