using System.Collections.Generic;

namespace Paradigm.ORM.Data.Descriptors
{
    /// <summary>
    /// Provides an interface to describe the mapping relationship between a table or view and a .NET type.
    /// </summary>
    /// <seealso cref="IColumnPropertyDescriptorCollection"/>
    public interface ITableTypeDescriptor : IColumnPropertyDescriptorCollection, ITableDescriptor
    {
        #region Properties

        /// <summary>
        /// Gets the identity column property descriptor.
        /// </summary>
        IColumnPropertyDescriptor IdentityProperty { get; }

        /// <summary>
        /// Gets a list of column property descriptors for all the primary keys.
        /// </summary>
        List<IColumnPropertyDescriptor> PrimaryKeyProperties { get; }

        /// <summary>
        /// Gets a list of column property descriptors for all the simple properties.
        /// </summary>
        /// <remarks>
        /// Simple properties does not include the identity properties but will contain
        /// the primary keys.
        /// </remarks>
        List<IColumnPropertyDescriptor> SimpleProperties { get; }

        /// <summary>
        /// Gets a list of navigation property descriptors for all the navigation properties.
        /// </summary>
        List<INavigationPropertyDescriptor> NavigationProperties { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Determines whether the specified entity is new.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>
        ///   <c>true</c> if the specified entity is new; otherwise, <c>false</c>.
        /// </returns>
        bool IsNew(object entity);

        #endregion
    }
}