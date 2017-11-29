using System.Collections.Generic;
using System.Threading.Tasks;

namespace Paradigm.ORM.Data.DatabaseAccess.Generic
{
    public partial interface IDatabaseAccess<TEntity>
    {
        /// <summary>
        /// Selects one element from the database.
        /// </summary>
        /// <param name="ids">Array of id values.</param>
        /// <returns><see cref="TEntity"/> with the same id values as the values provider; otherwise null.</returns>
        new Task<TEntity> SelectOneAsync(params object[] ids);

        /// <summary>
        /// Selects a list of all the elements in a table or view.
        /// </summary>
        /// <remarks>To select filtering results, use the overloaded method <see cref="SelectAsync(string, object[])"/>.</remarks>
        /// <returns>A list of <see cref="TEntity"/>.</returns>
        new Task<List<TEntity>> SelectAsync();

        /// <summary>
        /// Selects a list of elements in a table or view.
        /// </summary>
        /// <param name="whereClause">A where filter clause. Do not add the "WHERE" keyword to it. If you need to pass parameters, pass using @1, @2, @3.</param>
        /// <param name="parameters">A list of parameter values.</param>
        /// <returns>A list of <see cref="TEntity"/>.</returns>
        new Task<List<TEntity>> SelectAsync(string whereClause, params object[] parameters);

        /// <summary>
        /// Inserts a <see cref="TEntity"/> into the table or view.
        /// </summary>
        /// <remarks>
        /// If there are more than one element to insert, please use the overloaded method <see cref="Insert(IEnumerable{TEntity})"/>
        /// because is prepared to batch the operation, and preventing unnecessary roundtrips to the database.
        /// </remarks>
        /// <param name="entity"><see cref="TEntity"/> to insert.</param>
        Task InsertAsync(TEntity entity);

        /// <summary>
        /// Inserts a list of <see cref="TEntity"/> into the table or view.
        /// </summary>
        /// <remarks>
        /// This method utilizes batching to prevent unnecessary roundtrips to the database.
        /// </remarks>
        /// <param name="entities">List of <see cref="TEntity"/>.</param>
        Task InsertAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Updates a <see cref="TEntity"/> already stored in the table or view.
        /// </summary>
        /// <remarks>
        /// If there are more than one element to update, please use the overloaded method <see cref="Update(IEnumerable{TEntity})"/>
        /// because is prepared to batch the operation, and preventing unnecessary roundtrips to the database.
        /// </remarks>
        /// <param name="entity"><see cref="TEntity"/> to insert.</param>
        Task UpdateAsync(TEntity entity);

        /// <summary>
        /// Updates a list of <see cref="TEntity"/> stored in the table or view.
        /// </summary>
        /// <remarks>
        /// This method utilizes batching to prevent unnecessary roundtrips to the database.
        /// </remarks>
        /// <param name="entities">List of <see cref="TEntity"/> to update.</param>
        Task UpdateAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Deletes a <see cref="TEntity"/> from the table or view.
        /// </summary>
        /// <remarks>
        /// If there are more than one element to delete, please use the overloaded method <see cref="Delete(IEnumerable{TEntity})"/>
        /// because is prepared to batch the operation, and preventing unnecessary roundtrips to the database.
        /// </remarks>
        /// <param name="entity"><see cref="TEntity"/> to delete.</param>
        Task DeleteAsync(TEntity entity);

        /// <summary>
        /// Deletes a list of <see cref="TEntity"/> from the table or view.
        /// </summary>
        /// <remarks>
        /// This method utilizes batching to prevent unnecessary roundtrips to the database.
        /// </remarks>
        /// <param name="entities">List of <see cref="TEntity"/> to delete.</param>
        Task DeleteAsync(IEnumerable<TEntity> entities);
    }
}