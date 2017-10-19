using System;
using System.Linq;
using System.Text;
using Paradigm.ORM.Data.CommandBuilders;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Descriptors;
using Paradigm.ORM.Data.Extensions;

namespace Paradigm.ORM.Data.Cassandra.CommandBuilders
{
    /// <summary>
    /// Provides an implementation for mysql select one command builder objects.
    /// </summary>
    /// <seealso cref="CqlCommandBuilderBase" />
    /// <seealso cref="Paradigm.ORM.Data.CommandBuilders.ISelectOneCommandBuilder" />
    public class CqlSelectOneCommandBuilder : CqlCommandBuilderBase, ISelectOneCommandBuilder
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CqlSelectOneCommandBuilder"/> class.
        /// </summary>
        /// <param name="connector">A database connector.</param>
        /// <param name="descriptor">A table type descriptor.</param>
        public CqlSelectOneCommandBuilder(IDatabaseConnector connector, ITableDescriptor descriptor) : base(connector, descriptor)
        {
            this.Initialize();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the select one command ready to execute the select query.
        /// </summary>
        /// <param name="ids">The id values of the entity that will be selected from the database.</param>
        /// <returns>
        /// A select command already parametrized to execute.
        /// </returns>
        public IDatabaseCommand GetCommand(params object[] ids)
        {
            if (ids == null)
                throw new ArgumentNullException(nameof(ids), "You should at least provide one Id.");

            var primaryKeysCount = this.Descriptor.PrimaryKeyColumns.Count;

            if (ids.Length != primaryKeysCount)
                throw new ArgumentException($"The id count does not match the entity primary key count (the entity has {primaryKeysCount} keys).", nameof(ids));

            for (var i = 0; i < primaryKeysCount; i++)
            {
                this.Command.GetParameter(i).Value = ids[i];
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
            var builder = new StringBuilder();

            builder.AppendFormat("SELECT {0} FROM {1}", this.GetPropertyNames(), this.GetTableName());

            if (this.Descriptor.PrimaryKeyColumns.Any())
                builder.Append(" WHERE ");

            foreach (var primaryKey in this.Descriptor.PrimaryKeyColumns)
                builder.AppendFormat("{0}=:{1} AND", this.FormatProvider.GetEscapedName(primaryKey.ColumnName), primaryKey.ColumnName);

            this.Command = this.Connector.CreateCommand(builder.Remove(builder.Length - 4, 4).ToString());
            this.PopulateParameters(this.Descriptor.PrimaryKeyColumns);
        }

        #endregion
    }
}