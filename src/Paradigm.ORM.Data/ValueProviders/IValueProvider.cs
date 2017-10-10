using Paradigm.ORM.Data.CommandBuilders;
using Paradigm.ORM.Data.Descriptors;

namespace Paradigm.ORM.Data.ValueProviders
{
    /// <summary>
    /// Provides an interface for objects that provide values from different source, offering one
    /// unique interface for doing so.
    /// </summary>
    /// <seealso cref="IDeleteCommandBuilder"/>
    /// <seealso cref="IInsertCommandBuilder"/>
    /// <seealso cref="IUpdateCommandBuilder"/>
    public partial interface IValueProvider
    {
        /// <summary>
        /// Moves the reading cursor to the next entity or row.
        /// </summary>
        bool MoveNext();

        /// <summary>
        /// Gets the value related to the descriptor.
        /// </summary>
        /// <param name="descriptor">The descriptor.</param>
        /// <returns>Column descriptor value.</returns>
        object GetValue(IColumnDescriptor descriptor);
    }
}