using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.ValueProviders;

namespace Paradigm.ORM.Data.CommandBuilders
{
    /// <summary>
    /// Provides an interface for delete command builder objects.
    /// </summary>
    /// <seealso cref="ICommandBuilder" />
    public interface IDeleteCommandBuilder: ICommandBuilder
    {
        /// <summary>
        /// Gets a delete command query ready to delete one or more entities.
        /// </summary>
        /// <param name="valueProvider">A value provider to extract the value from the source.</param>
        /// <returns>
        /// A delete command already parametrized to delete the entities.
        /// </returns>
        IDatabaseCommand GetCommand(IValueProvider valueProvider);
    }
}