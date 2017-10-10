using Paradigm.ORM.Data.Database;

namespace Paradigm.ORM.Data.CommandBuilders
{
    /// <summary>
    /// Provides an interface for last inserted id command builder objects.
    /// </summary>
    /// <seealso cref="ICommandBuilder" />
    public interface ILastInsertIdCommandBuilder: ICommandBuilder
    {
        /// <summary>
        /// Gets a command to retrieve the last inserted id.
        /// </summary>
        /// <returns>a command already parametrized to retrieve the last inserted id.</returns>
        IDatabaseCommand GetCommand();
    }
}