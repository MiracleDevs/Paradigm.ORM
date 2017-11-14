using System;
using System.Collections.Generic;

namespace Paradigm.ORM.Data.StoredProcedures
{
    /// <summary>
    /// Provides an interface to execute data reader stored procedures returning only 5 result sets.
    /// </summary>
    /// <remarks>
    /// Instead of sending individual parameters to the procedure, the orm expects a  <see cref="TParameters"/> type
    /// containing or referencing the mapping information, where individual parameters will be mapped to properties.
    /// </remarks>
    /// <typeparam name="TParameters">The type of the parameters.</typeparam>
    /// <typeparam name="TResult1">The type of the first result.</typeparam>
    /// <typeparam name="TResult2">The type of the second result.</typeparam>
    /// <typeparam name="TResult3">The type of the third result.</typeparam>
    /// <typeparam name="TResult4">The type of the fourth result.</typeparam>
    /// <typeparam name="TResult5">The type of the fifth result.</typeparam>
    /// <seealso cref="Paradigm.ORM.Data.StoredProcedures.IRoutine" />
    public partial interface IReaderStoredProcedure<in TParameters, TResult1, TResult2, TResult3, TResult4, TResult5> : IRoutine
    {
        /// <summary>
        /// Executes the stored procedure and return a list of tuples.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>List of tuples.</returns>
        Tuple<List<TResult1>, List<TResult2>, List<TResult3>, List<TResult4>, List<TResult5>> Execute(TParameters parameters);
    }
}