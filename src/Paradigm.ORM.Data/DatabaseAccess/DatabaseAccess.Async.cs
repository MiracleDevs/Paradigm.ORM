using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Paradigm.ORM.Data.Batching;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Extensions;
using Paradigm.ORM.Data.ValueProviders;

namespace Paradigm.ORM.Data.DatabaseAccess
{
    public partial class DatabaseAccess
    {
        #region Public Methods

        /// <summary>
        /// Selects one element from the database.
        /// </summary>
        /// <param name="ids">Array of id values.</param>
        /// <returns>
        /// Element with the same id values as the values provider; otherwise null.
        /// </returns>
        public virtual async Task<object> SelectOneAsync(params object[] ids)
        {
            using var command = this.CommandBuilderManager.SelectOneCommandBuilder.GetCommand(ids);
            // 1. get current entity.
            var entity = await this.Connector.ExecuteReaderAsync(command, async reader => await this.Mapper.MapAsync(reader));

            // 2. get related entities.
            foreach (var x in this.NavigationDatabaseAccesses)
                await x.SelectAsync(entity);

            return entity.FirstOrDefault();
        }

        /// <summary>
        /// Selects a list of all the elements in a table or view.
        /// </summary>
        /// <returns>
        /// A list of objects that belong to the table or view.
        /// </returns>
        /// <remarks>
        /// To select filtering results, use the overloaded method <see cref="SelectAsync(string, object[])" />.
        /// </remarks>
        public virtual async Task<List<object>> SelectAsync()
        {
            return await this.SelectAsync(string.Empty);
        }

        /// <summary>
        /// Selects a list of elements in a table or view.
        /// </summary>
        /// <param name="whereClause">A where filter clause. Do not add the "WHERE" keyword to it. If you need to pass parameters, pass using @1, @2, @3.</param>
        /// <param name="parameters">A list of parameter values.</param>
        /// <returns>
        /// A list of objects that belong to the table or view.
        /// </returns>
        public virtual async Task<List<object>> SelectAsync(string whereClause, params object[] parameters)
        {
            using var command = this.CommandBuilderManager.SelectCommandBuilder.GetCommand(whereClause, parameters);
            // 1. get the current entities.
            var entities = await this.Connector.ExecuteReaderAsync(command, async reader => await this.Mapper.MapAsync(reader));

            // 2. get related entities.
            foreach (var x in this.NavigationDatabaseAccesses)
                await x.SelectAsync(entities);

            return entities;
        }

        /// <summary>
        /// Inserts an object into the table or view.
        /// </summary>
        /// <param name="entity">Object to insert.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">entity can not be null.</exception>
        /// <remarks>
        /// If there are more than one element to insert, please use the overloaded method <see cref="M:Paradigm.ORM.Data.DatabaseAccess.IDatabaseAccess.Insert(System.Collections.Generic.IEnumerable{System.Object})" />
        /// because is prepared to batch the operation, and preventing unnecessary round trips to the database.
        /// </remarks>
        public virtual async Task InsertAsync(object entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), $"{nameof(entity)} can not be null.");

            await this.InsertAsync(new List<object> { entity });
        }

        /// <summary>
        /// Inserts a list of objects into the table or view.
        /// </summary>
        /// <param name="entities">List of entities to insert.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">entities can not be null.</exception>
        /// <remarks>
        /// This method utilizes batching to prevent unnecessary round trips to the database.
        /// </remarks>
        public virtual async Task InsertAsync(IEnumerable<object> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities), $"{nameof(entities)} can not be null.");

            var entityList = entities as List<object> ?? entities.ToList();

            if (!entityList.Any())
                return;

            // 1. Save 1-1 relationships first, as we'll need the ids of the related entities
            //    for the main entities to be stored later.
            foreach (var navigationDatabaseAccess in this.NavigationDatabaseAccesses)
                await navigationDatabaseAccess.SaveBeforeAsync(entityList);

            // 2. Use a batch manager to save the main entities
            using (var batchManager = this.CreateBatchManager())
            {
                var valueProvider = new ClassValueProvider(this.Connector, entityList);

                // don't use the async move next because the class provider
                // is not really async right now.
                while (await valueProvider.MoveNextAsync())
                {
                    using (var command = this.CommandBuilderManager.InsertCommandBuilder.GetCommand(valueProvider))
                    {
                        batchManager.Add(new CommandBatchStep(command));
                    }

                    // if the entity has an auto incremental property,
                    // queue a command to retrieve the id from the last insertion.
                    if (this.Descriptor.IdentityProperty != null)
                    {
                        var entity = valueProvider.CurrentEntity;
                        using var command = this.CommandBuilderManager.LastInsertIdCommandBuilder.GetCommand();
                        batchManager.Add(new CommandBatchStep(command, async reader => await this.SetEntityIdAsync(entity, reader)));
                    }
                }

                await batchManager.ExecuteAsync();
            }

            // 3. Save the 1-Many relationship at last, as they'll need the
            //    main entity id before being stored.
            foreach (var navigationDatabaseAccess in this.NavigationDatabaseAccesses)
                await navigationDatabaseAccess.SaveAfterAsync(entityList);
        }

        /// <summary>
        /// Updates an object already stored in the table or view.
        /// </summary>
        /// <param name="entity">Object to insert.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">entity can not be null.</exception>
        /// <remarks>
        /// If there are more than one element to update, please use the overloaded method <see cref="M:Paradigm.ORM.Data.DatabaseAccess.IDatabaseAccess.Update(System.Collections.Generic.IEnumerable{System.Object})" />
        /// because is prepared to batch the operation, and preventing unnecessary round trips to the database.
        /// </remarks>
        public virtual async Task UpdateAsync(object entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), $"{nameof(entity)} can not be null.");

            await this.UpdateAsync(new List<object> { entity });
        }

        /// <summary>
        /// Updates a list of objects stored in the table or view.
        /// </summary>
        /// <param name="entities">List of entities to update.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">entities can not be null.</exception>
        /// <remarks>
        /// This method utilizes batching to prevent unnecessary round trips to the database.
        /// </remarks>
        public virtual async Task UpdateAsync(IEnumerable<object> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities), $"{nameof(entities)} can not be null.");

            var entityList = entities as List<object> ?? entities.ToList();

            if (!entityList.Any())
                return;

            // 1. Save 1-1 relationships first, as we'll need the ids of the related entities
            //    for the main entities to be stored later.
            foreach (var navigationDatabaseAccess in this.NavigationDatabaseAccesses)
                await navigationDatabaseAccess.SaveBeforeAsync(entityList);

            // 2. Use a batch manager to save the main entities
            using (var batchManager = this.CreateBatchManager())
            {
                var valueProvider = new ClassValueProvider(this.Connector, entityList);

                while (await valueProvider.MoveNextAsync())
                {
                    using var command = this.CommandBuilderManager.UpdateCommandBuilder.GetCommand(valueProvider);
                    batchManager.Add(new CommandBatchStep(command));
                }

                await batchManager.ExecuteAsync();
            }

            // 3. Save the 1-Many relationship at last, as they'll need the
            //    main entity id before being stored.
            foreach (var navigationDatabaseAccess in this.NavigationDatabaseAccesses)
                await navigationDatabaseAccess.SaveAfterAsync(entityList);
        }

        /// <summary>
        /// Deletes an object from the table or view.
        /// </summary>
        /// <param name="entity">Object to delete.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">entity can not be null.</exception>
        /// <remarks>
        /// If there are more than one element to delete, please use the overloaded method <see cref="M:Paradigm.ORM.Data.DatabaseAccess.IDatabaseAccess.Delete(System.Collections.Generic.IEnumerable{System.Object})" />
        /// because is prepared to batch the operation, and preventing unnecessary round trips to the database.
        /// </remarks>
        public virtual async Task DeleteAsync(object entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), $"{nameof(entity)} can not be null.");

            await this.DeleteAsync(new List<object> { entity });
        }

        /// <summary>
        /// Deletes a list of objects from the table or view.
        /// </summary>
        /// <param name="entities">List of entities to delete.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">entities can not be null.</exception>
        /// <remarks>
        /// This method utilizes batching to prevent unnecessary round trips to the database.
        /// </remarks>
        public virtual async Task DeleteAsync(IEnumerable<object> entities)
        {
            if (entities == null)
                throw new ArgumentException(nameof(entities), $"{nameof(entities)} can not be null.");

            var entityList = entities as List<object> ?? entities.ToList();

            if (!entityList.Any())
                return;

            // 1. Delete the children entities.
            foreach (var x in this.NavigationDatabaseAccesses)
                await x.DeleteBeforeAsync(entityList);

            // 2. Use a batch manager to save the main entities
            using (var batchManager = this.CreateBatchManager())
            {
                var valueProvider = new ClassValueProvider(this.Connector, entityList);

                while (await valueProvider.MoveNextAsync())
                {
                    using var command = this.CommandBuilderManager.DeleteCommandBuilder.GetCommand(valueProvider);
                    batchManager.Add(new CommandBatchStep(command));
                }

                await batchManager.ExecuteAsync();
            }

            // 3. Delete any parent entity if any.
            foreach (var x in this.NavigationDatabaseAccesses)
                await x.DeleteAfterAsync(entityList);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Sets the entity identifier asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Couldn't retrieve entity auto increment property.</exception>
        private async Task SetEntityIdAsync(object entity, IDatabaseReader reader)
        {
            if (!await reader.ReadAsync())
                throw new Exception("Couldn't retrieve entity auto increment property.");

            this.Descriptor.IdentityProperty.PropertyInfo.SetValue(entity, Convert.ToInt32(reader.GetValue(0)));
        }

        #endregion
    }
}