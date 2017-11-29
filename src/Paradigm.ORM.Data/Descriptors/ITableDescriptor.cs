using System.Collections.Generic;

namespace Paradigm.ORM.Data.Descriptors
{
    /// <summary>
    /// Provides an interface to describe a database table.
    /// </summary>
    /// <seealso cref="IColumnPropertyDescriptorCollection"/>
    public interface ITableDescriptor
    {
        #region Properties

        /// <summary>
        /// Gets the name of the database catalog.
        /// </summary>
        string CatalogName { get; }

        /// <summary>
        /// Gets the name of the database schema.
        /// </summary>
        string SchemaName { get; }

        /// <summary>
        /// Gets the name of the table.
        /// </summary>
        string TableName { get; }

        /// <summary>
        /// Gets the identity column descriptor.
        /// </summary>
        IColumnDescriptor IdentityColumn { get; }

        /// <summary>
        /// Gets a list of column descriptors for all the primary keys.
        /// </summary>
        List<IColumnDescriptor> PrimaryKeyColumns { get; }

        /// <summary>
        /// Gets a list of all the columns that aren't identities.
        /// </summary>
        /// <remarks>
        /// Simple columns does not include the identity properties but will contain
        /// the primary keys.
        /// </remarks>
        List<IColumnDescriptor> SimpleColumns { get; }

        /// <summary>
        /// Gets a list of all the columns.
        /// </summary>
        List<IColumnDescriptor> AllColumns { get; }

        #endregion
    }
}