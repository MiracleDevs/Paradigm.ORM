using System;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Descriptors;
using Paradigm.ORM.Data.Extensions;

namespace Paradigm.ORM.Data.CommandBuilders
{
    /// <summary>
    /// Provides all the standard command builders for a given table type descriptor.
    /// </summary>
    /// <seealso cref="Paradigm.ORM.Data.CommandBuilders.ICommandBuilderManager" />
    public class CommandBuilderManager : ICommandBuilderManager
    {
        #region Properties

        /// <summary>
        /// Gets the select one command builder.
        /// </summary>
        /// <value>
        /// The select one command builder.
        /// </value>
        public ISelectOneCommandBuilder SelectOneCommandBuilder { get; private set; }

        /// <summary>
        /// Gets the select command builder.
        /// </summary>
        /// <value>
        /// The select command builder.
        /// </value>
        public ISelectCommandBuilder SelectCommandBuilder { get; private set; }

        /// <summary>
        /// Gets the insert command builder.
        /// </summary>
        /// <value>
        /// The insert command builder.
        /// </value>
        public IInsertCommandBuilder InsertCommandBuilder { get; private set; }

        /// <summary>
        /// Gets the update command builder.
        /// </summary>
        /// <value>
        /// The update command builder.
        /// </value>
        public IUpdateCommandBuilder UpdateCommandBuilder { get; private set; }

        /// <summary>
        /// Gets the delete command builder.
        /// </summary>
        /// <value>
        /// The delete command builder.
        /// </value>
        public IDeleteCommandBuilder DeleteCommandBuilder { get; private set; }

        /// <summary>
        /// Gets the last insert identifier command builder.
        /// </summary>
        /// <value>
        /// The last insert identifier command builder.
        /// </value>
        public ILastInsertIdCommandBuilder LastInsertIdCommandBuilder { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandBuilderManager"/> class.
        /// </summary>
        /// <param name="serviceProvider">A reference to the currently scoped service provider.</param>
        /// <param name="connector">A reference to the current database connection.</param>
        /// <param name="descriptor">A table type descriptor.</param>
        /// <exception cref="System.Exception">Couldn't create a Command Builder Factory.</exception>
        public CommandBuilderManager(IServiceProvider serviceProvider, IDatabaseConnector connector, ITableTypeDescriptor descriptor)
        {
            var factory = serviceProvider.GetServiceIfAvailable(connector.GetCommandBuilderFactory) ?? throw new Exception("Couldn't create a Command Builder Factory.");

            this.SelectOneCommandBuilder = factory.CreateSelectOneCommandBuilder(descriptor);
            this.SelectCommandBuilder = factory.CreateSelectCommandBuilder(descriptor);
            this.InsertCommandBuilder = factory.CreateInsertCommandBuilder(descriptor);
            this.UpdateCommandBuilder = factory.CreateUpdateCommandBuilder(descriptor);
            this.DeleteCommandBuilder = factory.CreateDeleteCommandBuilder(descriptor);
            this.LastInsertIdCommandBuilder = factory.CreateLastInsertIdCommandBuilder();
        }

        #endregion

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.SelectOneCommandBuilder?.Dispose();
            this.SelectCommandBuilder?.Dispose();
            this.InsertCommandBuilder?.Dispose();
            this.UpdateCommandBuilder?.Dispose();
            this.DeleteCommandBuilder?.Dispose();
            this.LastInsertIdCommandBuilder?.Dispose();

            this.SelectOneCommandBuilder = null;
            this.SelectCommandBuilder = null;
            this.InsertCommandBuilder = null;
            this.UpdateCommandBuilder = null;
            this.DeleteCommandBuilder = null;
            this.LastInsertIdCommandBuilder = null;
        }
    }
}