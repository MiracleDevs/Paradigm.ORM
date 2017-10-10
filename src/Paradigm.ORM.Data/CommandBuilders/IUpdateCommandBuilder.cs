using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.ValueProviders;

namespace Paradigm.ORM.Data.CommandBuilders
{
    /// <summary>
    /// Provides an interface for update command builder objects.
    /// </summary>
    /// <seealso cref="ICommandBuilder" />
    public interface IUpdateCommandBuilder: ICommandBuilder
    {
        /// <summary>
        /// Gets an update command query ready to update one entity.
        /// </summary>
        /// <param name="valueProvider">A value provider to extract the value from the source.</param>
        /// <returns>
        /// An update command already parametrized to update the entity.
        /// </returns>
        IDatabaseCommand GetCommand(IValueProvider valueProvider);
    }
}