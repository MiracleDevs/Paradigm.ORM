using Paradigm.ORM.Data.CommandBuilders;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Descriptors;

namespace Paradigm.ORM.Data.SqlServer.CommandBuilders
{
    /// <summary>
    /// Provides an implementation for sql select last inserted id command builder objects.
    /// </summary>
    /// <seealso cref="LastInsertIdCommandBuilderBase" />
    /// <seealso cref="ILastInsertIdCommandBuilder" />
    public class SqlLastInsertIdCommandBuilder: LastInsertIdCommandBuilderBase
    {
        #region Properties

        /// <summary>
        /// Gets the command text.
        /// </summary>
        /// <value>
        /// The command text.
        /// </value>
        protected override string CommandText => "SELECT SCOPE_IDENTITY()";

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlLastInsertIdCommandBuilder"/> class.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        /// <param name="descriptor">The database descriptor.</param>
        public SqlLastInsertIdCommandBuilder(IDatabaseConnector connector, ITableDescriptor descriptor): base(connector, descriptor)
        {
        }

        #endregion
    }
}