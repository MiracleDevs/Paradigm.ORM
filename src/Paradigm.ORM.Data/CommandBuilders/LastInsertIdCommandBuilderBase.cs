using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Descriptors;
using Paradigm.ORM.Data.Extensions;

namespace Paradigm.ORM.Data.CommandBuilders
{
    /// <summary>
    /// Provides an implementation for  select last inserted id command builder objects.
    /// </summary>
    /// <seealso cref="Paradigm.ORM.Data.CommandBuilders.ILastInsertIdCommandBuilder" />
    public abstract class LastInsertIdCommandBuilderBase: CommandBuilderBase, ILastInsertIdCommandBuilder
    {
        #region Properties

        protected abstract string CommandText { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="LastInsertIdCommandBuilderBase"/> class.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        /// <param name="descriptor">The table descriptor.</param>
        protected LastInsertIdCommandBuilderBase(IDatabaseConnector connector, ITableDescriptor descriptor) : base(connector, descriptor)
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets a command to retrieve the last inserted id.
        /// </summary>
        /// <returns>
        /// a command already parametrized to retrieve the last inserted id.
        /// </returns>
        public IDatabaseCommand GetCommand()
        {
            return this.Connector.CreateCommand(this.CommandText);
        }

        #endregion
    }
}