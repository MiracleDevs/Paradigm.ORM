using Paradigm.ORM.Data.CommandBuilders;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Descriptors;

namespace Paradigm.ORM.Data.MySql.CommandBuilders
{
    /// <summary>
    /// Provides an implementation for mysql select last inserted id command builder objects.
    /// </summary>
    /// <seealso cref="ILastInsertIdCommandBuilder" />
    public class MySqlLastInsertIdCommandBuilder: LastInsertIdCommandBuilderBase
    {
        #region Properties

        /// <summary>
        /// Gets the command text.
        /// </summary>
        /// <value>
        /// The command text.
        /// </value>
        protected override string CommandText => "SELECT LAST_INSERT_ID()";

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlLastInsertIdCommandBuilder"/> class.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        /// <param name="descriptor">The table descriptor.</param>
        public MySqlLastInsertIdCommandBuilder(IDatabaseConnector connector, ITableDescriptor descriptor): base(connector, descriptor)
        {
        }

        #endregion
    }
}