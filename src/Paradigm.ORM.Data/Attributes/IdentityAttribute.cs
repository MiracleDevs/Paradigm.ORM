using System;

namespace Paradigm.ORM.Data.Attributes
{
    /// <summary>
    /// Indicates that the property is the identity of the table.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class IdentityAttribute: Attribute
    {

    }
}