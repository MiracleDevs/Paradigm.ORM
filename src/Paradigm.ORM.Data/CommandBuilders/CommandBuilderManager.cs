using System;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Descriptors;
using Paradigm.ORM.Data.Extensions;

namespace Paradigm.ORM.Data.CommandBuilders
{
    /// <inheritdoc />
    /// <summary>
    /// Provides all the standard command builders for a given table type descriptor.
    /// </summary>
    /// <seealso cref="T:Paradigm.ORM.Data.CommandBuilders.ICommandBuilderManager" />
    public class CommandBuilderManager : ICommandBuilderManager
    {
        #region Properties

        /// <summary>
        /// Gets the command builder factory.
        /// </summary>
        private ICommandBuilderFactory Factory { get; }

        /// <summary>
        /// Gets the select one command builder.
        /// </summary>
        /// <value>
        /// The select one command builder.
        /// </value>
        private readonly Lazy<ISelectOneCommandBuilder> _selectOneCommandBuilder;

        /// <summary>
        /// Gets the select command builder.
        /// </summary>
        /// <value>
        /// The select command builder.
        /// </value>
        private readonly Lazy<ISelectCommandBuilder> _selectCommandBuilder;

        /// <summary>
        /// Gets the insert command builder.
        /// </summary>
        /// <value>
        /// The insert command builder.
        /// </value>
        private readonly Lazy<IInsertCommandBuilder> _insertCommandBuilder;

        /// <summary>
        /// Gets the update command builder.
        /// </summary>
        /// <value>
        /// The update command builder.
        /// </value>
        private readonly Lazy<IUpdateCommandBuilder> _updateCommandBuilder;

        /// <summary>
        /// Gets the delete command builder.
        /// </summary>
        /// <value>
        /// The delete command builder.
        /// </value>
        private readonly Lazy<IDeleteCommandBuilder> _deleteCommandBuilder;

        /// <summary>
        /// Gets the last insert identifier command builder.
        /// </summary>
        /// <value>
        /// The last insert identifier command builder.
        /// </value>
        private readonly Lazy<ILastInsertIdCommandBuilder> _lastInsertIdCommandBuilder;

        /// <inheritdoc />
        /// <summary>
        /// Gets the select one command builder.
        /// </summary>
        /// <value>
        /// The select one command builder.
        /// </value>
        public ISelectOneCommandBuilder SelectOneCommandBuilder => this._selectOneCommandBuilder.Value;

        /// <inheritdoc />
        /// <summary>
        /// Gets the select command builder.
        /// </summary>
        /// <value>
        /// The select command builder.
        /// </value>
        public ISelectCommandBuilder SelectCommandBuilder => this._selectCommandBuilder.Value;

        /// <inheritdoc />
        /// <summary>
        /// Gets the insert command builder.
        /// </summary>
        /// <value>
        /// The insert command builder.
        /// </value>
        public IInsertCommandBuilder InsertCommandBuilder => this._insertCommandBuilder.Value;

        /// <inheritdoc />
        /// <summary>
        /// Gets the update command builder.
        /// </summary>
        /// <value>
        /// The update command builder.
        /// </value>
        public IUpdateCommandBuilder UpdateCommandBuilder => this._updateCommandBuilder.Value;

        /// <inheritdoc />
        /// <summary>
        /// Gets the delete command builder.
        /// </summary>
        /// <value>
        /// The delete command builder.
        /// </value>
        public IDeleteCommandBuilder DeleteCommandBuilder => this._deleteCommandBuilder.Value;

        /// <inheritdoc />
        /// <summary>
        /// Gets the last insert identifier command builder.
        /// </summary>
        /// <value>
        /// The last insert identifier command builder.
        /// </value>
        public ILastInsertIdCommandBuilder LastInsertIdCommandBuilder => this._lastInsertIdCommandBuilder.Value;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandBuilderManager"/> class.
        /// </summary>
        /// <param name="serviceProvider">A reference to the currently scoped service provider.</param>
        /// <param name="connector">A reference to the current database connection.</param>
        /// <param name="descriptor">A table type descriptor.</param>
        /// <exception cref="Exception">Couldn't create a Command Builder Factory.</exception>
        public CommandBuilderManager(IServiceProvider serviceProvider, IDatabaseConnector connector, ITableTypeDescriptor descriptor)
        {
            this.Factory = serviceProvider.GetServiceIfAvailable(connector.GetCommandBuilderFactory) ?? throw new Exception("Couldn't create a Command Builder Factory.");

            this._selectOneCommandBuilder    = new Lazy<ISelectOneCommandBuilder>(()=> this.Factory.CreateSelectOneCommandBuilder(descriptor), true);
            this._selectCommandBuilder       = new Lazy<ISelectCommandBuilder>(()=> this.Factory.CreateSelectCommandBuilder(descriptor), true);
            this._insertCommandBuilder       = new Lazy<IInsertCommandBuilder>(()=> this.Factory.CreateInsertCommandBuilder(descriptor), true);
            this._updateCommandBuilder       = new Lazy<IUpdateCommandBuilder>(()=> this.Factory.CreateUpdateCommandBuilder(descriptor), true);
            this._deleteCommandBuilder       = new Lazy<IDeleteCommandBuilder>(()=> this.Factory.CreateDeleteCommandBuilder(descriptor), true);
            this._lastInsertIdCommandBuilder = new Lazy<ILastInsertIdCommandBuilder>(()=> this.Factory.CreateLastInsertIdCommandBuilder(descriptor), true);
        }

        #endregion
    }
}