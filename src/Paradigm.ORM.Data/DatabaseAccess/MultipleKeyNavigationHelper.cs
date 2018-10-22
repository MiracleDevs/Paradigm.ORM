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
    /// Provides helper methods to the navigation database access, for entities with a multiple foreign keys.
    /// </summary>
    /// <seealso cref="Paradigm.ORM.Data.DatabaseAccess.INavigationHelper" />
    internal class MultipleKeyNavigationHelper : INavigationHelper
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
        /// Initializes a new instance of the <see cref="MultipleKeyNavigationHelper"/> class.
        /// </summary>
        /// <param name="navigationPropertyDescriptor">The navigation property descriptor.</param>
        /// <param name="formatProvider">The format provider.</param>
        public MultipleKeyNavigationHelper(INavigationPropertyDescriptor navigationPropertyDescriptor, ICommandFormatProvider formatProvider)
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
            var builder = new StringBuilder();
            var navigationDescriptors = this.NavigationPropertyDescriptor.NavigationKeyDescriptors;

            foreach (var entity in entities)
            {
                builder.Append("(");

                foreach (var navigationDescriptor in navigationDescriptors)
                {
                    var type = navigationDescriptor.FromPropertyDescriptor.PropertyInfo.PropertyType;
                    var value = navigationDescriptor.FromPropertyDescriptor.PropertyInfo.GetValue(entity);

                    builder.AppendFormat(" {0}={1} AND",
                        this.FormatProvider.GetEscapedName(navigationDescriptor.ToPropertyDescriptor.ColumnName),
                        this.FormatProvider.GetColumnValue(value, type));
                }

                builder.Remove(builder.Length - 4, 4).Append(") OR");
            }

            return builder.Remove(builder.Length - 3, 3).ToString();
        }

        /// <summary>
        /// Populates the navigation property with the referenced entity.
        /// </summary>
        /// <param name="mainEntity">The main entity.</param>
        /// <param name="referencedEntities">The referenced entities.</param>
        /// <param name="referencerProperty">Property in the main entity that references the related entity.</param>
        public void PopulateReferenced(object mainEntity, IList referencedEntities, PropertyInfo referencerProperty)
        {
            var navigationDescriptors = this.NavigationPropertyDescriptor.NavigationKeyDescriptors.ToList();
            var ids = new object[navigationDescriptors.Count];

            for (var i = 0; i < referencedEntities.Count; i++)
            {
                var referencedEntity = referencedEntities[i];
                var allEqual = true;

                for (var j = 0; j < navigationDescriptors.Count; j++)
                {
                    var navigationDescriptor = navigationDescriptors[j];
                    var fromValue = ids[j] ?? (ids[j] = navigationDescriptor.FromPropertyDescriptor.PropertyInfo.GetValue(mainEntity));
                    var toValue = navigationDescriptor.ToPropertyDescriptor.PropertyInfo.GetValue(referencedEntity);

                    if (fromValue.Equals(toValue))
                        continue;

                    allEqual = false;
                    break;
                }

                if (!allEqual)
                    continue;

                referencedEntities.Remove(referencedEntity);
                referencerProperty.GetSetMethod(true).Invoke(mainEntity, new[] { referencedEntity });
            }
        }

        /// <summary>
        /// Populates the navigation property with all the children passed.
        /// </summary>
        /// <param name="mainEntity">The main entity.</param>
        /// <param name="childEntities">The child entities.</param>
        /// <param name="list">Reference to the list property on the main entity.</param>
        public void PopulateList(object mainEntity, IList childEntities, IList list)
        {
            var navigationDescriptors = this.NavigationPropertyDescriptor.NavigationKeyDescriptors.ToList();
            var ids = new object[navigationDescriptors.Count];

            for (var i = 0; i < childEntities.Count; i++)
            {
                var childEntity = childEntities[i];
                var allEqual = true;

                for (var j = 0; j < navigationDescriptors.Count; j++)
                {
                    var navigationDescriptor = navigationDescriptors[j];
                    var fromValue = ids[j] ?? (ids[j] = navigationDescriptor.FromPropertyDescriptor.PropertyInfo.GetValue(mainEntity));
                    var toValue = navigationDescriptor.ToPropertyDescriptor.PropertyInfo.GetValue(childEntity);

                    if (fromValue.Equals(toValue))
                        continue;

                    allEqual = false;
                    break;
                }

                if (!allEqual)
                    continue;

                childEntities.Remove(childEntity);
                list.Add(childEntity);
            }
        }
    }
}