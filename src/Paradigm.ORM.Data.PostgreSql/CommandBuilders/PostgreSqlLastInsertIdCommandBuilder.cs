using Paradigm.ORM.Data.CommandBuilders;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Descriptors;

namespace Paradigm.ORM.Data.PostgreSql.CommandBuilders
{
    /// <summary>
    /// Provides an implementation for postgresql select last inserted id command builder objects.
    /// </summary>
    /// <seealso cref="LastInsertIdCommandBuilderBase" />
    /// <seealso cref="ILastInsertIdCommandBuilder" />
    public class PostgreSqlLastInsertIdCommandBuilder : LastInsertIdCommandBuilderBase
    {
        #region Properties

        /// <summary>
        /// Gets the command text.
        /// </summary>
        /// <value>
        /// The command text.
        /// </value>
        protected override string CommandText => "SELECT LASTVAL()";

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PostgreSqlLastInsertIdCommandBuilder"/> class.
        /// </summary>
        /// <param name="connector">The connector.</param>
        /// <param name="descriptor">The table descriptor.</param>
        public PostgreSqlLastInsertIdCommandBuilder(IDatabaseConnector connector, ITableDescriptor descriptor): base(connector, descriptor)
        {
        }

        #endregion
    }
}