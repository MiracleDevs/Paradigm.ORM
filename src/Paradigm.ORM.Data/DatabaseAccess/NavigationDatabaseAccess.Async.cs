using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Paradigm.ORM.Data.DatabaseAccess
{
    public partial class NavigationDatabaseAccess
    {
        #region Public Methods

        /// <summary>
        /// Selects the child entities of the specified entities.
        /// </summary>
        /// <param name="entities">List of parent entities.</param>
        public async Task SelectAsync(IEnumerable<object> entities)
        {
            var entityList = entities as IList<object> ?? entities.ToList();

            // 1. construct a where class to look up for related entities.
            var whereClause = this.NavigationHelper.GetWhereClause(entityList);

            // 2. if no where clause was provided, ignore selection and return.
            if (string.IsNullOrWhiteSpace(whereClause))
                return;

            // 3. get all related entities.
            var relatedEntities = await this.DatabaseAccess.SelectAsync(whereClause);

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
        public async Task SaveBeforeAsync(IEnumerable<object> entities)
        {
            // only parent entities can be saved.
            if (this.NavigationPropertyDescriptor.IsAggregateRoot)
                return;

            var entityList = entities as IList<object> ?? entities.ToList();

            if (!entityList.Any())
                return;

            var related = this.GetRelatedToSave(entityList);

            if (related.Item1.Any())
                await this.DatabaseAccess.InsertAsync(related.Item1);

            if (related.Item2.Any())
                await this.DatabaseAccess.UpdateAsync(related.Item2);

            this.SetRelatedIds(entityList);
        }

        /// <summary>
        /// Saves the child entities of the specified entities.
        /// </summary>
        /// <param name="entities">List of parent entities.</param>
        public async Task SaveAfterAsync(IEnumerable<object> entities)
        {
            // only children entities can be saved.
            if (!this.NavigationPropertyDescriptor.IsAggregateRoot)
                return;

            var children = this.GetChildrenToSave(entities);

            if (children.Item1.Any())
                await this.DatabaseAccess.InsertAsync(children.Item1);

            if (children.Item2.Any())
                await this.DatabaseAccess.UpdateAsync(children.Item2);
        }

        /// <summary>
        /// Deletes one to one relation entities of the specified entities.
        /// </summary>
        /// <param name="entities">List of parent entities.</param>
        public async Task DeleteBeforeAsync(IEnumerable<object> entities)
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

            await this.DatabaseAccess.DeleteAsync(childrenToDelete);
        }

        /// <summary>
        /// Deletes the child entities of the specified entities.
        /// </summary>
        /// <param name="entities">List of parent entities.</param>
        public async Task DeleteAfterAsync(IEnumerable<object> entities)
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

            await this.DatabaseAccess.DeleteAsync(relatedToDelete);
        }

        #endregion
    }
}