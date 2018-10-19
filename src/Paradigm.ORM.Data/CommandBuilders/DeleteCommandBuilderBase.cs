using System.Linq;
using System.Text;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Descriptors;
using Paradigm.ORM.Data.Exceptions;
using Paradigm.ORM.Data.Extensions;
using Paradigm.ORM.Data.ValueProviders;

namespace Paradigm.ORM.Data.CommandBuilders
{
    /// <summary>
    /// Provides an implementation for mysql delete command builder objects.
    /// </summary>
    /// <seealso cref="CommandBuilderBase" />
    /// <seealso cref="IDeleteCommandBuilder" />
    public abstract class DeleteCommandBuilderBase : CommandBuilderBase, IDeleteCommandBuilder
    {
        #region Columns

        /// <summary>
        /// Gets or sets the command text.
        /// </summary>
        /// <value>
        /// The command text.
        /// </value>
        protected string CommandText { get; private set; }

        /// <summary>
        /// Gets or sets the where clause.
        /// </summary>
        /// <value>
        /// The where clause.
        /// </value>
        protected string WhereClause { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteCommandBuilderBase"/> class.
        /// </summary>
        /// <param name="connector">A database connector.</param>
        /// <param name="descriptor">A table type descriptor.</param>
        protected DeleteCommandBuilderBase(IDatabaseConnector connector, ITableDescriptor descriptor): base(connector, descriptor)
        {
            this.Initialize();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets a delete command query ready to delete one or more entities.
        /// </summary>
        /// <param name="valueProvider"></param>
        /// <returns>
        /// A delete command already parametrized to delete the entities.
        /// </returns>
        public IDatabaseCommand GetCommand(IValueProvider valueProvider)
        {
            return this.Connector.CreateCommand(this.Descriptor.PrimaryKeyColumns.Count == 1 ? GetSingleKeyCommand(valueProvider) : GetMultipleKeyCommand(valueProvider));
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Gets the delete command for single key tables.
        /// </summary>
        /// <param name="valueProvider">The value provider.</param>
        protected string GetSingleKeyCommand(IValueProvider valueProvider)
        {
            var builder = new StringBuilder();

            builder.Append(this.CommandText);
            builder.Append(" WHERE ");

            var primaryKey = this.Descriptor.PrimaryKeyColumns[0];

            builder.Append(this.FormatProvider.GetEscapedName(primaryKey.ColumnName));
            builder.Append(" IN (");

            while (valueProvider.MoveNext())
            {
                builder.Append(this.FormatProvider.GetColumnValue(valueProvider.GetValue(primaryKey), primaryKey.DataType));
                builder.Append(",");
            }

            builder.Remove(builder.Length - 1, 1);
            builder.Append(")");

            return builder.ToString();
        }


        /// <summary>
        /// Gets the delete command for multiple key tables.
        /// </summary>
        /// <param name="valueProvider">The value provider.</param>
        protected virtual string GetMultipleKeyCommand(IValueProvider valueProvider)
        {
            var builder = new StringBuilder();

            builder.Append(this.CommandText);
            builder.Append(" WHERE ");

            while (valueProvider.MoveNext())
            {
                builder.Append("(");
                builder.AppendFormat(this.WhereClause, this.Descriptor.PrimaryKeyColumns.Select(x => this.FormatProvider.GetColumnValue(valueProvider.GetValue(x), x.DataType)).Cast<object>().ToArray());
                builder.Append(") OR ");
            }

            builder.Remove(builder.Length - 4, 4);

            return builder.ToString();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Initializes the command builder.
        /// </summary>
        private void Initialize()
        {
            this.CommandText = $"DELETE FROM {this.FormatProvider.GetTableName(this.Descriptor)}";

            if (!this.Descriptor.PrimaryKeyColumns.Any())
                throw new OrmNoPrimaryKeysException(this.Descriptor.TableName);

            var builder = new StringBuilder();

            for (var i = 0; i < this.Descriptor.PrimaryKeyColumns.Count; i++)
            {
                var primaryKey = this.Descriptor.PrimaryKeyColumns[i];
                builder.AppendFormat("{0}={{{1}}} AND ", this.FormatProvider.GetEscapedName(primaryKey.ColumnName), i);
            }

            this.WhereClause = builder.Remove(builder.Length - 5, 5).ToString();
        }

        #endregion
    }
}