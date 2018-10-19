using System;
using System.Linq;
using System.Text;
using Paradigm.ORM.Data.CommandBuilders;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Descriptors;
using Paradigm.ORM.Data.ValueProviders;

namespace Paradigm.ORM.Data.Cassandra.CommandBuilders
{
    /// <summary>
    /// Provides an implementation for mysql delete command builder objects.
    /// </summary>
    /// <seealso cref="DeleteCommandBuilderBase" />
    /// <seealso cref="IDeleteCommandBuilder" />
    public class CqlDeleteCommandBuilder : DeleteCommandBuilderBase
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CqlDeleteCommandBuilder"/> class.
        /// </summary>
        /// <param name="connector">A database connector.</param>
        /// <param name="descriptor">A table type descriptor.</param>
        public CqlDeleteCommandBuilder(IDatabaseConnector connector, ITableDescriptor descriptor) : base(connector, descriptor)
        {
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Gets the delete command for multiple key tables.
        /// </summary>
        /// <param name="valueProvider">The value provider.</param>
        /// <returns></returns>
        protected override string GetMultipleKeyCommand(IValueProvider valueProvider)
        {
            var builder = new StringBuilder();

            builder.Append(this.CommandText);
            builder.Append(" WHERE ");

            valueProvider.MoveNext();
            builder.AppendFormat(this.WhereClause, this.Descriptor.PrimaryKeyColumns.Select(x => this.FormatProvider.GetColumnValue(valueProvider.GetValue(x), x.DataType)).Cast<object>().ToArray());

            if (valueProvider.MoveNext())
                throw new NotSupportedException("Can not delete more than one entity at a time do to cql constrains.");

            return builder.ToString();
        }

        #endregion
    }
}