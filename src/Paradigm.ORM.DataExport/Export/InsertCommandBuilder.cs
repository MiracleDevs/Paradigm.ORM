using System.Collections.Generic;
using Paradigm.ORM.Data.CommandBuilders;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Descriptors;

namespace Paradigm.ORM.DataExport.Export
{
    /// <summary>
    /// Provides an implementation for mysql insert command builder objects.
    /// </summary>
    /// <remarks>
    /// This implementation is unique for the data export tool, and the difference
    /// from the specific insert command builders for each database type, this will
    /// try to insert all the columns. It wont work in every case, but the objective
    /// of this tool is an easy and quick data export tool, not a tool to cover every
    /// possible escenario.
    /// </remarks>
    /// <seealso cref="InsertCommandBuilderBase" />
    /// <seealso cref="IInsertCommandBuilder" />
    public class InsertCommandBuilder : InsertCommandBuilderBase
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="InsertCommandBuilder"/> class.
        /// </summary>
        /// <param name="connector">A database connector.</param>
        /// <param name="descriptor">A table type descriptor.</param>
        public InsertCommandBuilder(IDatabaseConnector connector, ITableDescriptor descriptor): base(connector, descriptor)
        {
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Gets a list of column descriptors that must be used in the insert statement.
        /// </summary>
        /// <remarks>
        /// Some databases may impose restrictions or limitation to the columns that can be
        /// inserted due to type or other rules. For ejample, TIMESTAMP type in sql server
        /// can not be inserted nor updated.
        /// </remarks>
        /// <returns>A list of column descriptors.</returns>
        protected override List<IColumnDescriptor> GetInsertColumns()
        {
            return this.Descriptor.AllColumns;
        }

        /// <summary>
        /// Gets a list of column descriptors used to populate the command parameters.
        /// </summary>
        /// <remarks>
        /// Some databases may impose restrictions or limitation to the columns that can be
        /// inserted due to type or other rules. For ejample, TIMESTAMP type in sql server
        /// can not be inserted nor updated.
        /// </remarks>
        /// <returns>A list of column descriptors.</returns>
        protected override List<IColumnDescriptor> GetPopulableColumns()
        {
            return this.Descriptor.AllColumns;
        }

        #endregion

    }
}