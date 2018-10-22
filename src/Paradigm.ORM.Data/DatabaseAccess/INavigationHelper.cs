using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Paradigm.ORM.Data.DatabaseAccess
{
    /// <summary>
    /// Provides an interface of helper methods used by the <see cref="NavigationDatabaseAccess"/>.
    /// </summary>
    internal interface INavigationHelper
    {
        /// <summary>
        /// Gets the where clause for a child relationship.
        /// </summary>
        /// <param name="entities">The parent entities.</param>
        /// <returns>Sql WHERE clause</returns>
        string GetWhereClause(IEnumerable<object> entities);

        /// <summary>
        /// Populates the navigation property with the referenced entity.
        /// </summary>
        /// <param name="mainEntity">The main entity.</param>
        /// <param name="referencedEntities">The referenced entities.</param>
        /// <param name="referencerProperty">Property in the main entity that references the related entity.</param>
        void PopulateReferenced(object mainEntity, IList referencedEntities, PropertyInfo referencerProperty);

        /// <summary>
        /// Populates the navigation property with all the children passed.
        /// </summary>
        /// <param name="mainEntity">The main entity.</param>
        /// <param name="childEntities">The child entities.</param>
        /// <param name="list">Reference to the list property on the main entity.</param>
        void PopulateList(object mainEntity, IList childEntities, IList list);
    }
}