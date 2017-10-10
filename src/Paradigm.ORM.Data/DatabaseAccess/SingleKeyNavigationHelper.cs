using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Paradigm.ORM.Data.CommandBuilders;
using Paradigm.ORM.Data.Descriptors;

namespace Paradigm.ORM.Data.DatabaseAccess
{
    /// <summary>
    /// Provides helper methods to the navigation database access, for entities with a single foreign key.
    /// </summary>
    /// <seealso cref="Paradigm.ORM.Data.DatabaseAccess.INavigationHelper" />
    internal class SingleKeyNavigationHelper : INavigationHelper
    {
        /// <summary>
        /// Gets the navigation property descriptor.
        /// </summary>
        private INavigationPropertyDescriptor NavigationPropertyDescriptor { get; }

        /// <summary>
        /// Gets the format provider.
        /// </summary>
        private ICommandFormatProvider FormatProvider { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SingleKeyNavigationHelper"/> class.
        /// </summary>
        /// <param name="navigationPropertyDescriptor">The navigation property descriptor.</param>
        /// <param name="formatProvider">The format provider.</param>
        public SingleKeyNavigationHelper(INavigationPropertyDescriptor navigationPropertyDescriptor, ICommandFormatProvider formatProvider)
        {
            this.NavigationPropertyDescriptor = navigationPropertyDescriptor;
            this.FormatProvider = formatProvider;
        }

        /// <summary>
        /// Gets the where clause for a child relationship.
        /// </summary>
        /// <param name="entities">The parent entities.</param>
        /// <returns>
        /// Sql WHERE clause
        /// </returns>
        public string GetWhereClause(IEnumerable<object> entities)
        {
            var navigationDescriptor = this.NavigationPropertyDescriptor.NavigationKeyDescriptors.First();
            var propertyInfo = navigationDescriptor.FromPropertyDescriptor.PropertyInfo;

            var type = navigationDescriptor.FromPropertyDescriptor.PropertyInfo.PropertyType;
            var ids = entities.Select(x => propertyInfo.GetValue(x)).Where(x => x != null).Select(x => this.FormatProvider.GetColumnValue(x, type)).ToList();

            if (!ids.Any())
                return null;

            var builder = new StringBuilder();

            builder.AppendFormat(" {0} IN (", this.FormatProvider.GetEscapedName(navigationDescriptor.ToPropertyDescriptor.ColumnName));

            foreach (var id in ids)
            {
                builder.Append(id);
                builder.Append(",");
            }

            return builder.Remove(builder.Length - 1, 1).Append(")").ToString();
        }

        /// <summary>
        /// Populates the navigation property with the referenced entity.
        /// </summary>
        /// <param name="mainEntity">The main entity.</param>
        /// <param name="referencedEntities">The referenced entities.</param>
        /// <param name="referencerProperty">Property in the main entity that references the related entity.</param>
        public void PopulateReferenced(object mainEntity, IList referencedEntities, PropertyInfo referencerProperty)
        {
            var navigationDescriptor = this.NavigationPropertyDescriptor.NavigationKeyDescriptors.First();
            var entityId = navigationDescriptor.FromPropertyDescriptor.PropertyInfo.GetValue(mainEntity);
            var propertyInfo = navigationDescriptor.ToPropertyDescriptor.PropertyInfo;

            for (var i = 0; i < referencedEntities.Count; i++)
            {
                var referencedEntity = referencedEntities[i];

                if (!propertyInfo.GetValue(referencedEntity).Equals(entityId))
                    continue;

                referencedEntities.Remove(referencedEntity);
                referencerProperty.GetSetMethod(true).Invoke(mainEntity, new[] { referencedEntity });
                i--;
            }
        }

        /// <summary>
        /// Populates the navigation property with all the childs passed.
        /// </summary>
        /// <param name="mainEntity">The main entity.</param>
        /// <param name="childEntities">The child entities.</param>
        /// <param name="list">Reference to the list property on the main entity.</param>
        public void PopulateList(object mainEntity, IList childEntities, IList list)
        {
            var navigationDescriptor = this.NavigationPropertyDescriptor.NavigationKeyDescriptors.First();
            var entityId = navigationDescriptor.FromPropertyDescriptor.PropertyInfo.GetValue(mainEntity);
            var propertyInfo = navigationDescriptor.ToPropertyDescriptor.PropertyInfo;

            for (var i = 0; i < childEntities.Count; i++)
            {
                var childEntity = childEntities[i];

                if (!propertyInfo.GetValue(childEntity).Equals(entityId))
                    continue;

                childEntities.Remove(childEntity);
                list.Add(childEntity);
                i--;
            }
        }
    }
}