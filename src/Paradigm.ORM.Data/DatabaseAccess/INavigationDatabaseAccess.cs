using System.Collections.Generic;
using Paradigm.ORM.Data.Descriptors;

namespace Paradigm.ORM.Data.DatabaseAccess
{
    /// <summary>
    /// Provides an interface to manage navigation relationships between entities.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public partial interface INavigationDatabaseAccess
    {
        /// <summary>
        /// Gets the end database access in the navigation relationships.
        /// </summary>
        IDatabaseAccess DatabaseAccess { get; }

        /// <summary>
        /// Gets the navigation property descriptor.
        /// </summary>
        INavigationPropertyDescriptor NavigationPropertyDescriptor { get; }

        /// <summary>
        /// Initializes the navigation database access.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Selects the child entities of the specified entities.
        /// </summary>
        /// <param name="entities">List of parent entities.</param>
        void Select(IEnumerable<object> entities);

        /// <summary>
        /// Saves one to one related entities.
        /// </summary>
        /// <param name="entities">List of parent entities.</param>
        void SaveBefore(IEnumerable<object> entities);

        /// <summary>
        /// Saves the child entities of the specified entities.
        /// </summary>
        /// <param name="entities">List of parent entities.</param>
        void SaveAfter(IEnumerable<object> entities);

        /// <summary>
        /// Deletes one to one relation entities of the specified entities.
        /// </summary>
        /// <param name="entities">List of parent entities.</param>
        void DeleteBefore(IEnumerable<object> entities);

        /// <summary>
        /// Deletes the child entities of the specified entities.
        /// </summary>
        /// <param name="entities">List of parent entities.</param>
        void DeleteAfter(IEnumerable<object> entities);
    }
}