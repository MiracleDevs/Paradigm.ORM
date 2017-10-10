using System;
using System.Collections.Generic;

namespace Paradigm.ORM.Data.Querying
{
    /// <summary>
    /// Provides an interface for query objects.
    /// </summary>
    /// <typeparam name="TResultType">The type containing or referencing the mapping information, that will be returned after executing the query.</typeparam>
    /// <seealso cref="System.IDisposable" />
    public partial interface IQuery<TResultType> : IDisposable
        where TResultType : class, new()
    {
        /// <summary>
        /// Executes the specified query and returns a list of <see cref="TResultType"/>.
        /// </summary>
        /// <param name="whereClause">A where filter clause. Do not add the "WHERE" keyword to it. If you need to pass parameters, pass using @1, @2, @3.</param>
        /// <param name="parameters">A list of parameter values.</param>
        /// <returns>A list of <see cref="TResultType"/>.</returns>
        List<TResultType> Execute(string whereClause = null, params object[] parameters);
    }
}