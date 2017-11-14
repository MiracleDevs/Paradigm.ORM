using System.Collections.Generic;

namespace Paradigm.ORM.Data.StoredProcedures
{
    /// <summary>
    /// Provides an interface to execute data reader stored procedures returning only 1 result set.
    /// </summary>
    /// <remarks>
    /// Instead of sending individual parameters to the procedure, the orm expects a  <see cref="TParameters"/> type
    /// containing or referencing the mapping information, where individual parameters will be mapped to properties.
    /// </remarks>
    /// <typeparam name="TParameters">The type of the parameters.</typeparam>
    /// <typeparam name="TResult1">The type of the result.</typeparam>
    /// <seealso cref="Paradigm.ORM.Data.StoredProcedures.IRoutine" />
    public partial interface IReaderStoredProcedure<in TParameters, TResult1>: IRoutine
    {
        /// <summary>
        /// Executes the stored procedure and return a list of <see cref="TResult1"/>.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>List of <see cref="TResult1"/></returns>
        List<TResult1> Execute(TParameters parameters);
    }
}