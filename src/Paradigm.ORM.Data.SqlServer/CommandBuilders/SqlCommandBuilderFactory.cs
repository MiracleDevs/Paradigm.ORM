using Paradigm.ORM.Data.CommandBuilders;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Descriptors;

namespace Paradigm.ORM.Data.SqlServer.CommandBuilders
{
    /// <summary>
    /// Provides an implementation to instantiate all the standard command builders.
    /// </summary>
    /// <seealso cref="ICommandBuilderFactory" />
    public class SqlCommandBuilderFactory : ICommandBuilderFactory
    {
        /// <summary>
        /// Gets or sets the database connector.
        /// </summary>
        private IDatabaseConnector Connector { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlCommandBuilderFactory"/> class.
        /// </summary>
        internal SqlCommandBuilderFactory(IDatabaseConnector connector)
        {
            this.Connector = connector;
        }

        /// <summary>
        /// Creates the select one command builder.
        /// </summary>
        /// <param name="descriptor">The table descriptor.</param>
        /// <returns></returns>
        public ISelectOneCommandBuilder CreateSelectOneCommandBuilder(ITableDescriptor descriptor)
        {
            return new SqlSelectOneCommandBuilder(this.Connector, descriptor);
        }

        /// <summary>
        /// Creates the select command builder.
        /// </summary>
        /// <param name="descriptor">The table descriptor.</param>
        /// <returns></returns>
        public ISelectCommandBuilder CreateSelectCommandBuilder(ITableDescriptor descriptor)
        {
            return new SqlSelectCommandBuilder(this.Connector, descriptor);
        }

        /// <summary>
        /// Creates the insert command builder.
        /// </summary>
        /// <param name="descriptor">The table descriptor.</param>
        /// <returns></returns>
        public IInsertCommandBuilder CreateInsertCommandBuilder(ITableDescriptor descriptor)
        {
            return new SqlInsertCommandBuilder(this.Connector, descriptor);
        }

        /// <summary>
        /// Creates the update command builder.
        /// </summary>
        /// <param name="descriptor">The table descriptor.</param>
        /// <returns></returns>
        public IUpdateCommandBuilder CreateUpdateCommandBuilder(ITableDescriptor descriptor)
        {
            return new SqlUpdateCommandBuilder(this.Connector, descriptor);
        }

        /// <summary>
        /// Creates the delete command builder.
        /// </summary>
        /// <param name="descriptor">The table descriptor.</param>
        /// <returns></returns>
        public IDeleteCommandBuilder CreateDeleteCommandBuilder(ITableDescriptor descriptor)
        {
            return new SqlDeleteCommandBuilder(this.Connector, descriptor);
        }

        /// <summary>
        /// Creates the last insert identifier command builder.
        /// </summary>
        /// <param name="descriptor">The table descriptor.</param>
        /// <returns></returns>
        public ILastInsertIdCommandBuilder CreateLastInsertIdCommandBuilder(ITableDescriptor descriptor)
        {
            return new SqlLastInsertIdCommandBuilder(this.Connector, descriptor);
        }
    }
}