using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.ValueProviders;

namespace Paradigm.ORM.Data.CommandBuilders
{
    /// <summary>
    /// Provides an interface for insert command builder objects.
    /// </summary>
    /// <seealso cref="ICommandBuilder" />
    public interface IInsertCommandBuilder: ICommandBuilder
    {
        /// <summary>
        /// Gets an insert command query ready to insert one entity.
        /// </summary>
        /// <param name="valueProvider">A value provider to extract the value from the source.</param>
        /// <returns>
        /// An insert command already parametrized to insert the entity.
        /// </returns>
        IDatabaseCommand GetCommand(IValueProvider valueProvider);
    }
}