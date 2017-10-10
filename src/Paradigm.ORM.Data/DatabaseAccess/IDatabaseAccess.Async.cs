using System.Collections.Generic;
using System.Threading.Tasks;

namespace Paradigm.ORM.Data.DatabaseAccess
{
    public partial interface IDatabaseAccess
    {
        /// <summary>
        /// Selects one element from the database.
        /// </summary>
        /// <param name="ids">Array of id values.</param>
        /// <returns>Element with the same id values as the values provider; otherwise null.</returns>
        Task<object> SelectOneAsync(params object[] ids);

        /// <summary>
        /// Selects a list of all the elements in a table or view.
        /// </summary>
        /// <remarks>To select filtering results, use the overloaded method <see cref="Select(string)"/>.</remarks>
        /// <returns>A list of objects that belong to the table or view.</returns>
        Task<List<object>> SelectAsync();

        /// <summary>
        /// Selects a list of elements in a table or view.
        /// </summary>
        /// <param name="whereClause">A where filter clause. Do not add the "WHERE" keyword to it. If you need to pass parameters, pass using @1, @2, @3.</param>
        /// <param name="parameters">A list of parameter values.</param>
        /// <returns>A list of objects that belong to the table or view.</returns>
        Task<List<object>> SelectAsync(string whereClause, params object[] parameters);

        /// <summary>
        /// Inserts an object into the table or view.
        /// </summary>
        /// <remarks>
        /// If there are more than one element to insert, please use the overloaded method <see cref="Insert(IEnumerable{object})"/>
        /// because is prepared to batch the operation, and preventing unnecessary roundtrips to the database.
        /// </remarks>
        /// <param name="entity">Object to insert.</param>
        Task InsertAsync(object entity);

        /// <summary>
        /// Inserts a list of objects into the table or view.
        /// </summary>
        /// <remarks>
        /// This method utilizes batching to prevent unnecessary roundtrips to the database.
        /// </remarks>
        /// <param name="entities">List of entities to insert.</param>
        Task InsertAsync(IEnumerable<object> entities);

        /// <summary>
        /// Updates an object already stored in the table or view.
        /// </summary>
        /// <remarks>
        /// If there are more than one element to update, please use the overloaded method <see cref="Update(IEnumerable{object})"/>
        /// because is prepared to batch the operation, and preventing unnecessary roundtrips to the database.
        /// </remarks>
        /// <param name="entity">Object to insert.</param>
        Task UpdateAsync(object entity);

        /// <summary>
        /// Updates a list of objects stored in the table or view.
        /// </summary>
        /// <remarks>
        /// This method utilizes batching to prevent unnecessary roundtrips to the database.
        /// </remarks>
        /// <param name="entities">List of entities to update.</param>
        Task UpdateAsync(IEnumerable<object> entities);

        /// <summary>
        /// Deletes an object from the table or view.
        /// </summary>
        /// <remarks>
        /// If there are more than one element to delete, please use the overloaded method <see cref="Delete(IEnumerable{object})"/>
        /// because is prepared to batch the operation, and preventing unnecessary roundtrips to the database.
        /// </remarks>
        /// <param name="entity">Object to delete.</param>
        Task DeleteAsync(object entity);

        /// <summary>
        /// Deletes a list of objects from the table or view.
        /// </summary>
        /// <remarks>
        /// This method utilizes batching to prevent unnecessary roundtrips to the database.
        /// </remarks>
        /// <param name="entities">List of entities to delete.</param>
        Task DeleteAsync(IEnumerable<object> entities);
    }
}