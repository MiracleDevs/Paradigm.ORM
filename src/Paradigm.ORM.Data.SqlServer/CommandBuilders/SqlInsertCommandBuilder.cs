using System;
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
    /// Provides an implementation for sql insert command builder objects.
    /// </summary>
    /// <seealso cref="Paradigm.ORM.Data.CommandBuilders.CommandBuilderBase" />
    /// <seealso cref="Paradigm.ORM.Data.CommandBuilders.IInsertCommandBuilder" />
    public class SqlInsertCommandBuilder : CommandBuilderBase, IInsertCommandBuilder
    {
        #region Columns

        /// <summary>
        /// Gets or sets the insertable properties.
        /// </summary>
        /// <value>
        /// The insertable properties.
        /// </value>
        private List<IColumnDescriptor> InsertableColumns { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlInsertCommandBuilder"/> class.
        /// </summary>
        /// <param name="connector">A database connector.</param>
        /// <param name="descriptor">A table type descriptor.</param>
        public SqlInsertCommandBuilder(IDatabaseConnector connector, ITableDescriptor descriptor): base(connector, descriptor)
        {
            this.Initialize();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets an insert command query ready to insert one entity.
        /// </summary>
        /// <param name="valueProvider"></param>
        /// <returns>
        /// An insert command already parametrized to insert the entity.
        /// </returns>
        public IDatabaseCommand GetCommand(IValueProvider valueProvider)
        {
            for (var i = 0; i < this.InsertableColumns.Count; i++)
            {
                this.Command.GetParameter(i).Value = valueProvider.GetValue(this.InsertableColumns[i]) ?? DBNull.Value;
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
            this.InsertableColumns = this.Descriptor.SimpleColumns.Where(x => x.DataType.ToLower() != "timestamp").ToList();
            this.Command = this.Connector.CreateCommand($"INSERT INTO {this.GetTableName()} ({this.GetPropertyNames(this.InsertableColumns)}) VALUES ({this.GetDbParameterNames(this.InsertableColumns)})");
            this.PopulateParameters(this.InsertableColumns);
        }

        #endregion
    }
}