using Paradigm.ORM.Data.CommandBuilders;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Extensions;

namespace Paradigm.ORM.Data.SqlServer.CommandBuilders
{
    /// <summary>
    /// Provides an implementation for sql select last inserted id command builder objects.
    /// </summary>
    /// <seealso cref="Paradigm.ORM.Data.CommandBuilders.ILastInsertIdCommandBuilder" />
    public class SqlLastInsertIdCommandBuilder: ILastInsertIdCommandBuilder
    {
        #region Columns

        /// <summary>
        /// Gets the database connector.
        /// </summary>
        /// <value>
        /// The connector.
        /// </value>
        private IDatabaseConnector Connector { get; set; }

        /// <summary>
        /// Gets or sets the command.
        /// </summary>
        /// <value>
        /// The command.
        /// </value>
        private IDatabaseCommand Command { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlLastInsertIdCommandBuilder"/> class.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        public SqlLastInsertIdCommandBuilder(IDatabaseConnector connector)
        {
            this.Connector = connector;
            this.Initialize();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Command?.Dispose(); 
            this.Command = null;
            this.Connector = null;
        }

        /// <summary>
        /// Gets a command to retrieve the last inserted id.
        /// </summary>
        /// <returns>
        /// a command already parametrized to retrieve the last inserted id.
        /// </returns>
        public IDatabaseCommand GetCommand()
        {
            return this.Command;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        private void Initialize()
        {
            this.Command = this.Connector.CreateCommand("SELECT SCOPE_IDENTITY()");
        }

        #endregion
    }
}