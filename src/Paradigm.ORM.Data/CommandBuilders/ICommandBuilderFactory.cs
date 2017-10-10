using Paradigm.ORM.Data.Descriptors;

namespace Paradigm.ORM.Data.CommandBuilders
{
    /// <summary>
    /// Provides an interface for a command builder factory.
    /// </summary>
    public interface ICommandBuilderFactory
    {
        /// <summary>
        /// Creates the select one command builder.
        /// </summary>
        /// <param name="descriptor">The table descriptor.</param>
        /// <returns></returns>
        ISelectOneCommandBuilder CreateSelectOneCommandBuilder(ITableDescriptor descriptor);

        /// <summary>
        /// Creates the select command builder.
        /// </summary>
        /// <param name="descriptor">The table descriptor.</param>
        /// <returns></returns>
        ISelectCommandBuilder CreateSelectCommandBuilder(ITableDescriptor descriptor);

        /// <summary>
        /// Creates the insert command builder.
        /// </summary>
        /// <param name="descriptor">The table descriptor.</param>
        /// <returns></returns>
        IInsertCommandBuilder CreateInsertCommandBuilder(ITableDescriptor descriptor);

        /// <summary>
        /// Creates the update command builder.
        /// </summary>
        /// <param name="descriptor">The table descriptor.</param>
        /// <returns></returns>
        IUpdateCommandBuilder CreateUpdateCommandBuilder(ITableDescriptor descriptor);

        /// <summary>
        /// Creates the delete command builder.
        /// </summary>
        /// <param name="descriptor">The table descriptor.</param>
        /// <returns></returns>
        IDeleteCommandBuilder CreateDeleteCommandBuilder(ITableDescriptor descriptor);

        /// <summary>
        /// Creates the last insert identifier command builder.
        /// </summary>
        /// <returns></returns>
        ILastInsertIdCommandBuilder CreateLastInsertIdCommandBuilder();
    }
}