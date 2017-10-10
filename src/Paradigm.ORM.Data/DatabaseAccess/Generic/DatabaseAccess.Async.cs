using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Paradigm.ORM.Data.DatabaseAccess.Generic
{
    public partial class DatabaseAccess<TEntity>
    {
        #region Public Methods

        /// <summary>
        /// Selects one element from the database.
        /// </summary>
        /// <param name="ids">Array of id values.</param>
        /// <returns>
        ///   <see cref="TEntity" /> with the same id values as the values provider; otherwise null.
        /// </returns>
        public new async Task<TEntity> SelectOneAsync(params object[] ids)
        {
            return await base.SelectOneAsync(ids) as TEntity;
        }

        /// <summary>
        /// Selects a list of all the elements in a table or view.
        /// </summary>
        /// <returns>
        /// A list of <see cref="TEntity" />.
        /// </returns>
        /// <remarks>
        /// To select filtering results, use the overloaded method <see cref="M:Paradigm.ORM.Data.DatabaseAccess.Generic.IDatabaseAccess`1.Select(System.String)" />.
        /// </remarks>
        public new async Task<List<TEntity>> SelectAsync()
        {
            return (await base.SelectAsync()).Cast<TEntity>().ToList();
        }

        /// <summary>
        /// Selects a list of elements in a table or view.
        /// </summary>
        /// <param name="whereClause">A where filter clause. Do not add the "WHERE" keyword to it. If you need to pass parameters, pass using @1, @2, @3.</param>
        /// <param name="parameters">A list of parameter values.</param>
        /// <returns>
        /// A list of <see cref="TEntity" />.
        /// </returns>
        public new async Task<List<TEntity>> SelectAsync(string whereClause, params object[] parameters)
        {
            return (await base.SelectAsync(whereClause, parameters)).Cast<TEntity>().ToList();
        }

        /// <summary>
        /// Inserts a <see cref="TEntity" /> into the table or view.
        /// </summary>
        /// <param name="entity"><see cref="TEntity" /> to insert.</param>
        /// <returns></returns>
        /// <remarks>
        /// If there are more than one element to insert, please use the overloaded method <see cref="M:Paradigm.ORM.Data.DatabaseAccess.Generic.IDatabaseAccess`1.Insert(System.Collections.Generic.IEnumerable{`0})" />
        /// because is prepared to batch the operation, and preventing unnecessary roundtrips to the database.
        /// </remarks>
        public virtual async Task InsertAsync(TEntity entity)
        {
            await base.InsertAsync(entity);
        }

        /// <summary>
        /// Inserts a list of <see cref="TEntity" /> into the table or view.
        /// </summary>
        /// <param name="entities">List of <see cref="TEntity" />.</param>
        /// <returns></returns>
        /// <remarks>
        /// This method utilizes batching to prevent unnecessary roundtrips to the database.
        /// </remarks>
        public virtual async Task InsertAsync(IEnumerable<TEntity> entities)
        {
            await base.InsertAsync(entities.Cast<object>().ToList());
        }

        /// <summary>
        /// Updates a <see cref="TEntity" /> already stored in the table or view.
        /// </summary>
        /// <param name="entity"><see cref="TEntity" /> to insert.</param>
        /// <returns></returns>
        /// <remarks>
        /// If there are more than one element to update, please use the overloaded method <see cref="M:Paradigm.ORM.Data.DatabaseAccess.Generic.IDatabaseAccess`1.Update(System.Collections.Generic.IEnumerable{`0})" />
        /// because is prepared to batch the operation, and preventing unnecessary roundtrips to the database.
        /// </remarks>
        public virtual async Task UpdateAsync(TEntity entity)
        {
            await base.UpdateAsync(entity);
        }

        /// <summary>
        /// Updates a list of <see cref="TEntity" /> stored in the table or view.
        /// </summary>
        /// <param name="entities">List of <see cref="TEntity" /> to update.</param>
        /// <returns></returns>
        /// <remarks>
        /// This method utilizes batching to prevent unnecessary roundtrips to the database.
        /// </remarks>
        public virtual async Task UpdateAsync(IEnumerable<TEntity> entities)
        {
            await base.UpdateAsync(entities.Cast<object>().ToList());
        }

        /// <summary>
        /// Deletes a <see cref="TEntity" /> from the table or view.
        /// </summary>
        /// <param name="entity"><see cref="TEntity" /> to delete.</param>
        /// <returns></returns>
        /// <remarks>
        /// If there are more than one element to delete, please use the overloaded method <see cref="M:Paradigm.ORM.Data.DatabaseAccess.Generic.IDatabaseAccess`1.Delete(System.Collections.Generic.IEnumerable{`0})" />
        /// because is prepared to batch the operation, and preventing unnecessary roundtrips to the database.
        /// </remarks>
        public virtual async Task DeleteAsync(TEntity entity)
        {
            await base.DeleteAsync(entity);
        }

        /// <summary>
        /// Deletes a list of <see cref="TEntity" /> from the table or view.
        /// </summary>
        /// <param name="entities">List of <see cref="TEntity" /> to delete.</param>
        /// <returns></returns>
        /// <remarks>
        /// This method utilizes batching to prevent unnecessary roundtrips to the database.
        /// </remarks>
        public virtual async Task DeleteAsync(IEnumerable<TEntity> entities)
        {
            await base.DeleteAsync(entities.Cast<object>().ToList());
        }

        #endregion
    }
}