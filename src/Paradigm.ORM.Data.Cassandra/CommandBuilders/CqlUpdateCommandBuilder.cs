using System.Collections.Generic;
using System.Linq;
using Paradigm.ORM.Data.CommandBuilders;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Descriptors;

namespace Paradigm.ORM.Data.Cassandra.CommandBuilders
{
    /// <summary>
    /// Provides an implementation for mysql update command builder objects.
    /// </summary>
    /// <seealso cref="UpdateCommandBuilderBase" />
    /// <seealso cref="IUpdateCommandBuilder" />
    public class CqlUpdateCommandBuilder : UpdateCommandBuilderBase
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CqlUpdateCommandBuilder"/> class.
        /// </summary>
        /// <param name="connector">A database connector.</param>
        /// <param name="descriptor">A table type descriptor.</param>
        public CqlUpdateCommandBuilder(IDatabaseConnector connector, ITableDescriptor descriptor): base(connector, descriptor)
        {
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Gets a list of column descriptors that must be used in the update statement.
        /// </summary>
        /// <returns>
        /// A list of column descriptors.
        /// </returns>
        /// <remarks>
        /// Some databases may impose restrictions or limitation to the columns that can be
        /// updated due to type or other rules. For ejample, TIMESTAMP type in sql server
        /// can not be inserted nor updated.
        /// </remarks>
        protected override List<IColumnDescriptor> GetUpdateableColumns()
        {
            return this.Descriptor.AllColumns.Where(x => !this.Descriptor.PrimaryKeyColumns.Contains(x)).ToList();
        }

        /// <summary>
        /// Gets a list of column descriptors used to populate the command parameters.
        /// </summary>
        /// <returns>
        /// A list of column descriptors.
        /// </returns>
        /// <remarks>
        /// Some databases may impose restrictions or limitation to the columns that can be
        /// updated due to type or other rules. For ejample, TIMESTAMP type in sql server
        /// can not be inserted nor updated.
        /// </remarks>
        protected override List<IColumnDescriptor> GetPopulableColumns()
        {
            return this.Descriptor.AllColumns;
        }

        #endregion
    }
}