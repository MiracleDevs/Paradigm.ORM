using Paradigm.ORM.Data.Database;

namespace Paradigm.ORM.Data.CommandBuilders
{
    /// <summary>
    /// Provides an interface select one command builder objects.
    /// </summary>
    /// <seealso cref="ICommandBuilder" />
    public interface ISelectOneCommandBuilder: ICommandBuilder
    {
        /// <summary>
        /// Gets the select one command ready to execute the select query.
        /// </summary>
        /// <param name="ids">The id values of the entity that will be selected from the database.</param>
        /// <returns>A select command already parametrized to execute.</returns>
        IDatabaseCommand GetCommand(params object[] ids);
    }
}