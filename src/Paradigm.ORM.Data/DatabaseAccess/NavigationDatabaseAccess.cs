using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.DatabaseAccess.Generic;
using Paradigm.ORM.Data.Descriptors;
using Paradigm.ORM.Data.Extensions;

namespace Paradigm.ORM.Data.DatabaseAccess
{
    /// <summary>
    /// Handles the navigation relations between a type and other child types.
    /// </summary>
    /// <seealso cref="Paradigm.ORM.Data.DatabaseAccess.INavigationDatabaseAccess" />
    public partial class NavigationDatabaseAccess : INavigationDatabaseAccess
    {
        #region Properties

        /// <summary>
        /// Gets the service provider.
        /// </summary>
        private IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// Gets or sets the navigation helper.
        /// </summary>
        private INavigationHelper NavigationHelper { get; set; }

        /// <summary>
        /// Gets or sets the connector.
        /// </summary>
        private IDatabaseConnector Connector { get; }

        /// <summary>
        /// Gets the end database access in the navigation relationships.
        /// </summary>
        public IDatabaseAccess DatabaseAccess { get; private set; }

        /// <summary>
        /// Gets the navigation property descriptor.
        /// </summary>
        public INavigationPropertyDescriptor NavigationPropertyDescriptor { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationDatabaseAccess"/> class.
        /// </summary>
        /// <param name="serviceProvider">Reference to the scoped service provider.</param>
        /// <param name="connector">Reference to the scoped connector.</param>
        /// <param name="navigationPropertyDescriptor">The navigation property descriptor.</param>
        internal NavigationDatabaseAccess(IServiceProvider serviceProvider, IDatabaseConnector connector, INavigationPropertyDescriptor navigationPropertyDescriptor)
        {
            this.ServiceProvider = serviceProvider;
            this.Connector = connector;
            this.NavigationPropertyDescriptor = navigationPropertyDescriptor;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Initializes the navigation database access.
        /// </summary>
        public void Initialize()
        {
            this.DatabaseAccess = this.ServiceProvider.GetServiceIfAvailable(
                typeof(IDatabaseAccess<>).MakeGenericType(this.NavigationPropertyDescriptor.ToDescriptor.Type),
                () => new DatabaseAccess(this.ServiceProvider, this.Connector, this.NavigationPropertyDescriptor.ToDescriptor.Type)) as IDatabaseAccess;

            this.NavigationHelper = this.NavigationPropertyDescriptor.NavigationAttributes.Count > 1
                ? (INavigationHelper)new MultipleKeyNavigationHelper(this.NavigationPropertyDescriptor, this.Connector.GetCommandFormatProvider())
                : new SingleKeyNavigationHelper(this.NavigationPropertyDescriptor, this.Connector.GetCommandFormatProvider());
        }

        /// <summary>
        /// Selects the child entities of the specified entities.
        /// </summary>
        /// <param name="entities">List of parent entities.</param>
        public void Select(IEnumerable<object> entities)
        {
            var entityList = entities as IList<object> ?? entities.ToList();

            if (!entityList.Any())
                return;

            // 1. construct a where class to look up for related entities.
            var whereClause = this.NavigationHelper.GetWhereClause(entityList);

            // 2. if no where clause was provided, ignore selection and return.
            if (string.IsNullOrWhiteSpace(whereClause))
                return;

            // 3. get all related entities.
            var relatedEntities = this.DatabaseAccess.Select(whereClause);

            // 4. populate child lists or related fields.
            foreach (var entity in entityList)
            {
                if (this.NavigationPropertyDescriptor.IsAggregateRoot)
                    this.NavigationHelper.PopulateList(entity, relatedEntities, this.GetListInstance(entity));
                else
                    this.NavigationHelper.PopulateReferenced(entity, relatedEntities, this.NavigationPropertyDescriptor.PropertyInfo);
            }
        }

        /// <summary>
        /// Saves one to one related entities.
        /// </summary>
        /// <param name="entities">List of parent entities.</param>
        public void SaveBefore(IEnumerable<object> entities)
        {
            // only parent entities can be saved.
            if (this.NavigationPropertyDescriptor.IsAggregateRoot)
                return;

            var entityList = entities as IList<object> ?? entities.ToList();

            if (!entityList.Any())
                return;

            var related = this.GetRelatedToSave(entityList);

            if (related.Item1.Any())
                this.DatabaseAccess.Insert(related.Item1);

            if (related.Item2.Any())
                this.DatabaseAccess.Update(related.Item2);

            this.SetRelatedIds(entityList);
        }

        /// <summary>
        /// Saves the child entities of the specified entities.
        /// </summary>
        /// <param name="entities">List of parent entities.</param>
        public void SaveAfter(IEnumerable<object> entities)
        {
            // only children entities can be saved.
            if (!this.NavigationPropertyDescriptor.IsAggregateRoot)
                return;

            var children = this.GetChildrenToSave(entities);

            if (children.Item1.Any())
                this.DatabaseAccess.Insert(children.Item1);

            if (children.Item2.Any())
                this.DatabaseAccess.Update(children.Item2);
        }

        /// <summary>
        /// Deletes one to one relation entities of the specified entities.
        /// </summary>
        /// <param name="entities">List of parent entities.</param>
        public void DeleteBefore(IEnumerable<object> entities)
        {
            // only children entities can be deleted here.
            if (!this.NavigationPropertyDescriptor.IsAggregateRoot)
                return;

            var entityList = entities as IList<object> ?? entities.ToList();

            if (!entityList.Any())
                return;

            var childrenToDelete = this.GetChildrenToDelete(entityList);

            if (!childrenToDelete.Any())
                return;

            this.DatabaseAccess.Delete(childrenToDelete);
        }

        /// <summary>
        /// Deletes the child entities of the specified entities.
        /// </summary>
        /// <param name="entities">List of parent entities.</param>
        public void DeleteAfter(IEnumerable<object> entities)
        {
            // only parent entities can be deleted here.
            if (this.NavigationPropertyDescriptor.IsAggregateRoot)
                return;

            var entityList = entities as IList<object> ?? entities.ToList();

            if (!entityList.Any())
                return;

            var relatedToDelete = this.GetRelatedToDelete(entityList);

            if (!relatedToDelete.Any())
                return;

            this.DatabaseAccess.Delete(relatedToDelete);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets a list reference if the navigation property type is a list.
        /// </summary>
        /// <param name="entity">The parent entity.</param>
        /// <returns>A reference to the property list.</returns>
        /// <exception cref="System.Exception">Couldn't create a list for property 'PropertyName' in type 'TypeName'.</exception>
        private IList GetListInstance(object entity)
        {
            if (this.NavigationPropertyDescriptor.PropertyInfo.GetValue(entity) is IList list)
                return list;

            list = Activator.CreateInstance(this.NavigationPropertyDescriptor.PropertyType) as IList;

            if (list == null)
                throw new Exception($"Couldn't create a list for property '{this.NavigationPropertyDescriptor.PropertyName}' in type '{this.NavigationPropertyDescriptor.FromDescriptor.TypeName}'.");

            this.NavigationPropertyDescriptor.PropertyInfo.SetValue(entity, list);

            return list;
        }

        /// <summary>
        /// Gets the children to save.
        /// </summary>
        /// <param name="entities">The parent entities.</param>
        /// <returns>List of children to save.</returns>
        private Tuple<List<object>, List<object>> GetChildrenToSave(IEnumerable entities)
        {
            var childrenToInsert = new List<object>();
            var childrenToUpdate = new List<object>();

            var entityKeyPropertyInfo = this.NavigationPropertyDescriptor.FromDescriptor.IdentityProperty?.PropertyInfo;

            var childForeignKeyPropertyInfo = this.NavigationPropertyDescriptor
                .NavigationKeyDescriptors
                .FirstOrDefault(x => x.FromPropertyDescriptor.ColumnName == this.NavigationPropertyDescriptor.FromDescriptor.IdentityProperty?.ColumnName)?
                .ToPropertyDescriptor?.PropertyInfo;

            var needsToSetId = entityKeyPropertyInfo != null && childForeignKeyPropertyInfo != null;

            foreach (var entity in entities)
            {
                var allChildren = this.GetListInstance(entity);
                var entityId = needsToSetId ? entityKeyPropertyInfo.GetValue(entity) : null;

                foreach (var child in allChildren)
                {
                    if (this.NavigationPropertyDescriptor.ToDescriptor.IsNew(child))
                    {
                        childForeignKeyPropertyInfo?.SetValue(child, entityId);
                        childrenToInsert.Add(child);
                    }
                    else
                    {
                        childrenToUpdate.Add(child);
                    }
                }
            }

            return new Tuple<List<object>, List<object>>(childrenToInsert, childrenToUpdate);
        }

        /// <summary>
        /// Gets the related entities to save.
        /// </summary>
        /// <param name="entities">The parent entities.</param>
        /// <returns>List of related entities to save.</returns>
        private Tuple<List<object>, List<object>> GetRelatedToSave(IEnumerable entities)
        {
            var relatedToInsert = new List<object>();
            var relatedToUpdate = new List<object>();

            foreach (var entity in entities)
            {
                var relatedEntity = this.NavigationPropertyDescriptor.PropertyInfo.GetValue(entity);

                if (relatedEntity == null)
                    continue;

                if (this.NavigationPropertyDescriptor.ToDescriptor.IsNew(relatedEntity))
                    relatedToInsert.Add(relatedEntity);
                else
                    relatedToUpdate.Add(relatedEntity);
            }

            return new Tuple<List<object>, List<object>>(relatedToInsert, relatedToUpdate);
        }

        private void SetRelatedIds(IEnumerable<object> entities)
        {
            var entityKeyPropertyInfo = this.NavigationPropertyDescriptor.ToDescriptor.IdentityProperty?.PropertyInfo;

            var entityForeignKeyProperty = this.NavigationPropertyDescriptor.NavigationKeyDescriptors.FirstOrDefault()?.FromPropertyDescriptor?.PropertyInfo;

            var needsToSetId = entityKeyPropertyInfo != null && entityForeignKeyProperty != null;

            foreach (var entity in entities)
            {
                var relatedEntity = this.NavigationPropertyDescriptor.PropertyInfo.GetValue(entity);

                if (relatedEntity == null)
                    continue;

                var entityId = needsToSetId ? entityKeyPropertyInfo.GetValue(relatedEntity) : null;
                entityForeignKeyProperty?.SetValue(entity, entityId);
            }
        }

        /// <summary>
        /// Gets the children to delete.
        /// </summary>
        /// <param name="entities">The parent entities.</param>
        /// <returns>List of children to delete.</returns>
        private List<object> GetChildrenToDelete(IEnumerable entities)
        {
            var childrenToDelete = new List<object>();

            foreach (var entity in entities)
            {
                var children = this.GetListInstance(entity);
                childrenToDelete.AddRange(children.Cast<object>().Where(child => child != null));
            }

            return childrenToDelete;
        }

        /// <summary>
        /// Gets the related entities to delete.
        /// </summary>
        /// <param name="entities">The parent entities.</param>
        /// <returns>List of related entities to delete.</returns>
        private List<object> GetRelatedToDelete(IEnumerable entities)
        {
            return entities.Cast<object>().Select(entity => this.NavigationPropertyDescriptor.PropertyInfo.GetValue(entity)).Where(relatedEntity => relatedEntity != null).ToList();
        }

        #endregion
    }
}