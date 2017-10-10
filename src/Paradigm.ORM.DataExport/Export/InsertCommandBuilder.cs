using System;
using Paradigm.ORM.Data.CommandBuilders;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Descriptors;
using Paradigm.ORM.Data.Extensions;
using Paradigm.ORM.Data.MySql.CommandBuilders;
using Paradigm.ORM.Data.ValueProviders;

namespace Paradigm.ORM.DataExport.Export
{
    /// <summary>
    /// Provides an implementation for mysql insert command builder objects.
    /// </summary>
    /// <seealso cref="MySqlCommandBuilderBase" />
    /// <seealso cref="Paradigm.ORM.Data.CommandBuilders.IInsertCommandBuilder" />
    public class InsertCommandBuilder : MySqlCommandBuilderBase, IInsertCommandBuilder
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="InsertCommandBuilder"/> class.
        /// </summary>
        /// <param name="connector">A database connector.</param>
        /// <param name="descriptor">A table type descriptor.</param>
        public InsertCommandBuilder(IDatabaseConnector connector, ITableDescriptor descriptor): base(connector, descriptor)
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
            for (var i = 0; i < this.Descriptor.AllColumns.Count; i++)
            {
                this.Command.GetParameter(i).Value = valueProvider.GetValue(this.Descriptor.AllColumns[i]) ?? DBNull.Value;
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
            var columns = this.Descriptor.AllColumns;
            this.Command = this.Connector.CreateCommand($"INSERT INTO {this.GetTableName()} ({this.GetPropertyNames(columns)}) VALUES ({this.GetDbParameterNames(columns)})");
            this.PopulateParameters(columns);
        }

        #endregion
    }
}