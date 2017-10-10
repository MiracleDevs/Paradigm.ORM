using Paradigm.ORM.Data.Database;

namespace Paradigm.ORM.Data.CommandBuilders
{
    /// <summary>
    /// Provides an interface select command builder objects.
    /// </summary>
    /// <seealso cref="ICommandBuilder" />
    public interface ISelectCommandBuilder : ICommandBuilder
    {
        /// <summary>
        /// Gets the select command ready to execute the select query.
        /// </summary>
        /// <param name="whereClause">An optional where clause to add to the query. If the where contains parameters, they need to be named as @1 @2 @3 etc.</param>
        /// <param name="parameters">A list of parameter values.</param>
        /// <returns>A select command already parametrized to execute.</returns>
        IDatabaseCommand GetCommand(string whereClause = null, params object[] parameters);
    }
}