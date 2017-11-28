using System.Collections.Generic;
using System.Linq;
using Paradigm.ORM.Data.CommandBuilders;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Descriptors;

namespace Paradigm.ORM.Data.SqlServer.CommandBuilders
{
    /// <summary>
    /// Provides an implementation for sql insert command builder objects.
    /// </summary>
    /// <seealso cref="CommandBuilderBase" />
    /// <seealso cref="IInsertCommandBuilder" />
    public class SqlInsertCommandBuilder : InsertCommandBuilderBase, IInsertCommandBuilder
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlInsertCommandBuilder"/> class.
        /// </summary>
        /// <param name="connector">A database connector.</param>
        /// <param name="descriptor">A table type descriptor.</param>
        public SqlInsertCommandBuilder(IDatabaseConnector connector, ITableDescriptor descriptor): base(connector, descriptor)
        {
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Gets a list of column descriptors that must be used in the insert statement.
        /// </summary>
        /// <returns>
        /// A list of column descriptors.
        /// </returns>
        /// <remarks>
        /// Some databases may impose restrictions or limitation to the columns that can be
        /// inserted due to type or other rules. For ejample, TIMESTAMP type in sql server
        /// can not be inserted nor updated.
        /// </remarks>
        protected override List<IColumnDescriptor> GetInsertColumns()
        {
            return this.Descriptor.SimpleColumns.Where(x => x.DataType.ToLower() != "timestamp").ToList();
        }

        #endregion
    }
}