using System;
using System.Collections.Generic;

namespace Paradigm.ORM.Data.DatabaseAccess
{
    /// <summary>
    /// Provides an interface to access a database table or view.
    /// </summary>
    /// <seealso cref="IDisposable"/>
    public partial interface IDatabaseAccess
    {
        /// <summary>
        /// Selects one element from the database.
        /// </summary>
        /// <param name="ids">Array of id values.</param>
        /// <returns>Element with the same id values as the values provider; otherwise null.</returns>
        object SelectOne(params object[] ids);

        /// <summary>
        /// Selects a list of all the elements in a table or view.
        /// </summary>
        /// <remarks>To select filtering results, use the overloaded method <see cref="Select(string,object[])"/>.</remarks>
        /// <returns>A list of objects that belong to the table or view.</returns>
        List<object> Select();

        /// <summary>
        /// Selects a list of elements in a table or view.
        /// </summary>
        /// <param name="whereClause">A where filter clause. Do not add the "WHERE" keyword to it. If you need to pass parameters, pass using @1, @2, @3.</param>
        /// <param name="parameters">A list of parameter values.</param>
        /// <returns>A list of objects that belong to the table or view.</returns>
        List<object> Select(string whereClause, params object[] parameters);

        /// <summary>
        /// Inserts an object into the table or view.
        /// </summary>
        /// <remarks>
        /// If there are more than one element to insert, please use the overloaded method <see cref="Insert(IEnumerable{object})"/>
        /// because is prepared to batch the operation, and preventing unnecessary roundtrips to the database.
        /// </remarks>
        /// <param name="entity">Object to insert.</param>
        void Insert(object entity);

        /// <summary>
        /// Inserts a list of objects into the table or view.
        /// </summary>
        /// <remarks>
        /// This method utilizes batching to prevent unnecessary roundtrips to the database.
        /// </remarks>
        /// <param name="entities">List of entities to insert.</param>
        void Insert(IEnumerable<object> entities);

        /// <summary>
        /// Updates an object already stored in the table or view.
        /// </summary>
        /// <remarks>
        /// If there are more than one element to update, please use the overloaded method <see cref="Update(IEnumerable{object})"/>
        /// because is prepared to batch the operation, and preventing unnecessary roundtrips to the database.
        /// </remarks>
        /// <param name="entity">Object to insert.</param>
        void Update(object entity);

        /// <summary>
        /// Updates a list of objects stored in the table or view.
        /// </summary>
        /// <remarks>
        /// This method utilizes batching to prevent unnecessary roundtrips to the database.
        /// </remarks>
        /// <param name="entities">List of entities to update.</param>
        void Update(IEnumerable<object> entities);

        /// <summary>
        /// Deletes an object from the table or view.
        /// </summary>
        /// <remarks>
        /// If there are more than one element to delete, please use the overloaded method <see cref="Delete(IEnumerable{object})"/>
        /// because is prepared to batch the operation, and preventing unnecessary roundtrips to the database.
        /// </remarks>
        /// <param name="entity">Object to delete.</param>
        void Delete(object entity);

        /// <summary>
        /// Deletes a list of objects from the table or view.
        /// </summary>
        /// <remarks>
        /// This method utilizes batching to prevent unnecessary roundtrips to the database.
        /// </remarks>
        /// <param name="entities">List of entities to delete.</param>
        void Delete(IEnumerable<object> entities);
    }
}