using System.Collections.Generic;
using System.Linq;
using Paradigm.ORM.Data.CommandBuilders;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Descriptors;
using Paradigm.ORM.Data.Extensions;
using Paradigm.ORM.Data.ValueProviders;

namespace Paradigm.ORM.Data.SqlServer.CommandBuilders
{
    /// <summary>
    /// Provides an implementation for sql update command builder objects.
    /// </summary>
    /// <seealso cref="Paradigm.ORM.Data.CommandBuilders.CommandBuilderBase" />
    /// <seealso cref="Paradigm.ORM.Data.CommandBuilders.IUpdateCommandBuilder" />
    public class SqlUpdateCommandBuilder : CommandBuilderBase, IUpdateCommandBuilder
    {
        #region Columns

        /// <summary>
        /// Gets or sets the updateable properties.
        /// </summary>
        /// <value>
        /// The updateable properties.
        /// </value>
        private List<IColumnDescriptor> UpdateableColumns { get; set; }

        /// <summary>
        /// Gets or sets the populable properties.
        /// </summary>
        /// <value>
        /// The populable properties.
        /// </value>
        private List<IColumnDescriptor> PopulableColumns { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlUpdateCommandBuilder"/> class.
        /// </summary>
        /// <param name="connector">A database connector.</param>
        /// <param name="descriptor">A table type descriptor.</param>
        public SqlUpdateCommandBuilder(IDatabaseConnector connector, ITableDescriptor descriptor): base(connector, descriptor)
        {
            this.Initialize();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets an update command query ready to update one entity.
        /// </summary>
        /// <param name="valueProvider"></param>
        /// <returns>
        /// An update command already parametrized to update the entity.
        /// </returns>
        public IDatabaseCommand GetCommand(IValueProvider valueProvider)
        {
            for (var i = 0; i < this.PopulableColumns.Count; i++)
            {
                this.Command.GetParameter(i).Value = valueProvider.GetValue(this.PopulableColumns[i]);
            }

            return this.Command;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Initializes the command builder.
        /// </summary>
        private void Initialize()
        {
            this.UpdateableColumns = this.Descriptor.SimpleColumns.Where(x => x.DataType.ToLower() != "timestamp").ToList();
            this.PopulableColumns = this.Descriptor.AllColumns.Where(x => x.DataType.ToLower() != "timestamp").ToList();

            this.Command = this.Connector.CreateCommand($"UPDATE {this.GetTableName()} SET {this.GetDbParameterNamesAndValues(this.UpdateableColumns)} WHERE {this.GetDbParameterNamesAndValues(this.Descriptor.PrimaryKeyColumns, " AND ")}");
            this.PopulateParameters(this.PopulableColumns);
        }

        #endregion
    }
}